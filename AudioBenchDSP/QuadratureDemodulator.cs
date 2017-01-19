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

using AudioBenchDSP.Funcs;
using AudioBenchDSP.Types;
using System;

namespace AudioBenchDSP {
  public class QuadratureDemodulator {
    private DataBuffer temporaryBuffer;
    private unsafe Complex* tempPtr;
    private int tempLength;

    public int Gain { get; set; }

    public QuadratureDemodulator(int gain) {
      this.Gain = gain;
      temporaryBuffer = null;
      tempLength = 0;
    }

    public unsafe void Work(Complex[] input, ref float[] output, int length) {
      fixed (float* oPtr = &output[0]) {
        fixed (Complex* iPtr = &input[0]) {
          _Work(iPtr, oPtr, length);
        }
      }
    }

    public unsafe void _Work(Complex* input, float* output, int length) {
      if (tempLength != length) {
        temporaryBuffer = DataBuffer.Create(length, sizeof(Complex));
        tempPtr = (Complex*)temporaryBuffer;
        tempLength = length;
      }

      VMath.MultiplyByConjugate(ref tempPtr, &input[1], input, length);
      for (int i = 0; i < length; i++) {
        output[i] = (float)(Gain * Math.Atan2(tempPtr[i].imag, tempPtr[i].real));
      }
    }
  }
}
