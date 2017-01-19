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

using AudioBenchDSP.Types;
using System;

namespace AudioBenchDSP {
  public class Pll : ControlLoop {
    private readonly static float M_TWOPI = (float)Math.PI * 2;

    private float lockSignal;

    public bool Squelch { get; set; }
    public float LockThreshold { get; set; }

    public bool IsLocked {
      get {
        return Math.Abs(lockSignal) > LockThreshold;
      }
    }

    public Pll(float loopBw, float maxFreq, float minFreq) : base(loopBw, maxFreq, minFreq) { }

    private float modulus2pi(float input) {
      if (input > Math.PI) {
        return input - M_TWOPI;
      } else if (input < -Math.PI) {
        return input + M_TWOPI;
      } else {
        return input;
      }
    }
    private float phaseDetector(Complex sample, float refPhase) {
      float samplePhase = (float)Math.Atan2(sample.imag, sample.real);
      return modulus2pi(samplePhase - refPhase);
    }

    public void Work(Complex[] input, ref Complex[] output, int length) {
      float error, imag, real;

      for (int i = 0; i < length; i++) {
        imag = (float)Math.Sin(phase);
        real = (float)Math.Cos(phase);

        output[i] = input[i] * new Complex(real, -imag);

        error = phaseDetector(input[i], phase);

        AdvanceLoop(error);
        PhaseWrap();
        FrequencyLimit();

        lockSignal = lockSignal * (1 - Alpha) + Alpha * (input[i].real * real + input[i].imag * imag);

        if (Squelch && !IsLocked) {
          output[i] = 0;
        }
      }
    }
  }
}
