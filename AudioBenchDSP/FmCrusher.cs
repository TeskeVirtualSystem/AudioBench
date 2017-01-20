﻿///////////////////////////////////////////////////////////////////////////////////
///    Audio Bench                                                              ///
///    Copyright(C) 2017 Teske Virtual System                                   ///
///                                                                             ///
///    This program is free software: you can redistribute it and/or modify     ///
///    it under the terms of the GNU General Public License as published by     ///
///    the Free Software Foundation, either version 3 of the License, or        ///
///    any later version.                                                       ///
///                                                                             ///
///    This program is distributed in the hope that it will be useful,          ///
///    but WITHOUT ANY WARRANTY; without even the implied warranty of           ///
///    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the              ///
///    GNU General Public License for more details.                             ///
///                                                                             ///
///    You should have received a copy of the GNU General Public license        ///
///    along with this program.If not, see<http://www.gnu.org/licenses/>.       ///
///////////////////////////////////////////////////////////////////////////////////

using System;
using AudioBenchDSP.FIR;
using AudioBenchDSP.Funcs;
using AudioBenchDSP.Types;
using System.Collections.Generic;
using static AudioBenchDSP.Callbacks.ManagedCallbacks;
using System.Threading;

namespace AudioBenchDSP {
  public class FmCrusher {
    private FirFilter<Complex> decimator;
    private FirFilter<float> audioDecimator;
    private QuadratureDemodulator quadDemod;
    private Pll pll;

    private int decimation;
    private int audioDecimation;
    private int inputRate;
    private int outputRate;
    private Queue<Complex> samplesFifo;
    private object queueLock;

    private Complex[] buffer1;
    private Complex[] buffer2;
    private float[] buffer3;
    private float[] buffer4;
    private Thread worker;

    public event FmCrusherAudioEvent AudioEvent;

    public FmCrusher(int inputRate, int outputRate) {
      this.inputRate = inputRate;
      this.outputRate = outputRate;
      samplesFifo = new Queue<Complex>();
      queueLock = new object();
      buffer1 = new Complex[0];
      buffer2 = new Complex[0];

      // We need 160kHz output band for Demod.
      // Let's see if we have an integer divider
      decimation = inputRate / 160000;
      if (decimation * 160000 != inputRate) {
        throw new ArgumentException("Input sample rate is not multiple of 160000");
      }

      audioDecimation = 160000 / outputRate;

      float[] taps = Filters.simpleLowPass(160000, outputRate / 2.0f, 63);
      audioDecimator = new FirFilter<float>(audioDecimation, taps);

      taps = Filters.simpleLowPass(inputRate, inputRate / (decimation * 2), 63);
      decimator = new FirFilter<Complex>(decimation, taps);
      pll = new Pll((float)Math.PI / 100, (float)Math.PI * 2 / 100, (float)Math.PI / 200);
      quadDemod = new QuadratureDemodulator(1);

      worker = new Thread(new ThreadStart(WorkCycle));
      worker.Priority = ThreadPriority.Highest;
      worker.Start();
    }

    public void Feed(Complex[] samples) {
      lock (queueLock) {
        for (int i = 0; i < samples.Length; i++) {
          samplesFifo.Enqueue(samples[i]);
        }
      }
    }

    private void WorkCycle() {
      while (true) {
        int length = 0;
        // Fetch Samples
        lock (queueLock) {
          length = samplesFifo.Count;
          if (buffer1.Length < length) {
            buffer1 = new Complex[length];
            buffer2 = new Complex[length];
            buffer3 = new float[length];
            buffer4 = new float[length];
          }
          for (int i = 0; i < length; i++) {
            buffer1[i] = samplesFifo.Dequeue();
          }
        }

        if (length == 0) {
          continue;
        }

        // Low Pass / Decimate
        length /= decimation;
        decimator.Work(buffer1, ref buffer2, length);

        // Sync
        pll.Work(buffer2, ref buffer1, length);

        // Demodulate
        quadDemod.Work(buffer1, ref buffer3, length);

        // Filter output
        length /= audioDecimation;
        audioDecimator.Work(buffer3, ref buffer4, length);

        float[] outdata = new float[length];
        for (int i=0; i<length; i++) {
          outdata[i] = buffer4[i];
        }
        // Send output
        AudioEvent(new Callbacks.FmCrusherAudioData(outdata, outputRate));
        Thread.Sleep(1);
      }
    }
  }
}
