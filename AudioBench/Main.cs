///////////////////////////////////////////////////////////////////////////////////
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

using AudioBenchDSP;
using AudioBenchDSP.Callbacks;
using AudioBenchDSP.PortAudio;
using AudioBenchDSP.Types;
using SharpRTL.Types;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AudioBench {
  public partial class Main : Form {
    private int audioSampleRate = 40000;
    private int audioBufferInMs = 100;

    private object audioLock;
    private Queue<float> audioFifo;
    private AudioPlayer pl;
    private RtlDevice rtlDevice;
    private FmCrusher fmCrusher;

    private float[] rtlLut;

    public Main() {
      InitializeComponent();

      rtlLut = new float[256];
      for (int i = 0; i < 256; i++) {
        rtlLut[i] = (i - 128) * (1f / 127f);
      }

      List<AudioDevice> audioDevices = AudioDevice.GetDevices(AudioDeviceDirection.Output);
      AudioDevice df = new AudioDevice("", "", 0, AudioDeviceDirection.Output, true);
      foreach (AudioDevice d in audioDevices) {
        Console.WriteLine(d.Name);
        if (d.IsDefault) {
          df = d;
        }
      }

      audioFifo = new Queue<float>();
      audioLock = new object();

      pl = new AudioPlayer(df.Index, audioSampleRate, (uint)(audioBufferInMs * audioSampleRate / 1000), audioBufferNeeded);
      rtlDevice = new RtlDevice(0);
      rtlDevice.SamplesAvailable += RtlDevice_SamplesAvailable;
      rtlDevice.Frequency = 200600000;
      rtlDevice.SampleRate = 2560000;

      fmCrusher = new FmCrusher(2560000, audioSampleRate);
      fmCrusher.AudioEvent += FmCrusher_AudioEvent;
      rtlDevice.Start();
      
    }

    private void FmCrusher_AudioEvent(FmCrusherAudioData e) {
      int length = e.audioData.Length;
      float[] audioData = e.audioData;
      lock (audioLock) {
        for (int i = 0; i < length; i++) {
          audioFifo.Enqueue(audioData[i]);
        }
      }
    }

    private void RtlDevice_SamplesAvailable(ref byte[] data, int length) {
      length = length / 2;
      Complex[] dataC = new Complex[length];
      for (int i=0; i<length; i++) {
        dataC[i] = new Complex(rtlLut[data[i*2]], rtlLut[data[i*2+1]]);
      }
      fmCrusher.Feed(dataC);
    }

    private unsafe void audioBufferNeeded(UnsafeSamplesEventArgs e) {
      float* audio = e.FloatBuffer;
      int length = e.Length;
      lock(audioLock) {
        if (audioFifo.Count >= length) {
          for (int i = 0; i < length; i++) {
            float sample = audioFifo.Dequeue();
            audio[i * 2] = sample;
            audio[i * 2 + 1] = sample;
          }
        } else {
          for (int i = 0; i < length; i++) {
            audio[i * 2] = 0;
            audio[i * 2 + 1] = 0;
          }
        }
      }
    }

    private void Main_FormClosing(object sender, FormClosingEventArgs e) {
      rtlDevice.Stop();
      fmCrusher.Stop();
    }
  }
}
