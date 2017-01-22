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

using AudioBenchDSP.IIR;
using System;

namespace AudioBenchDSP {
  public class Deemphasis {
    private IIRFilter iir;
    public Deemphasis(float fs, float tau = 75e-6f) {
      double c = 1 / tau;
      double ca = (2 * fs * Math.Atan(c / (2 * fs)));

      double k = -ca / (2 * fs);
      double p1 = (1 + k) / (1 - k);
      double b0 = -k / (1 - k);
      double[] btaps = new double[] { b0 , b0 };
      double[] ataps = new double[] { 1.0, -p1 };

      iir = new IIRFilter(btaps, ataps);
    }

    public void Work(float[] input, ref float[] output, int length) {
      iir.Work(input, ref output, length);
    }
  }
}
