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

namespace AudioBenchDSP.Funcs {
  public static class VMath {
    /// <summary>
    /// Computes the DotProduct between Input and Taps and return the result as a Complex Number.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="input"></param>
    /// <param name="taps"></param>
    /// <param name="length"></param>
    public unsafe static void DotProduct(out Complex result, Complex[] input, float[] taps, int length) {
      fixed (Complex *iPtr = &input[0]) {
        fixed (float *tapPtr = &taps[0]) {
          _DotProduct(out result, iPtr, tapPtr, length);
        }
      }
    }

    public unsafe static void DotProduct(out float result, float[] input, float[] taps, int length) {
      fixed (float* iPtr = &input[0]) {
        fixed (float* tapPtr = &taps[0]) {
          _DotProduct(out result, iPtr, tapPtr, length);
        }
      }
    }

    public static unsafe void MultiplyByConjugate(ref Complex *output, Complex *inputA, Complex *inputB, int length) {
      for (int i=0; i<length; i++) {
        output[i] = inputA[i] * inputB[i].Conjugate();
      }
    }

    public static unsafe void _DotProduct(out Complex result, Complex* input, float* taps, int length) {
      float[] res = { 0, 0 };

      float* iPtr = (float*)input;
      float* tPtr = taps;

      for (int i = 0; i < length; i++) {
        res[0] += ((*iPtr++) * (*tPtr));
        res[1] += ((*iPtr++) * (*tPtr++));
      }

      result = new Complex(res[0], res[1]);
    }

    public static unsafe void _DotProduct(out float result, float* input, float* taps, int length) {
      float res = 0;

      float* iPtr = (float*)input;
      float* tPtr = taps;

      for (int i = 0; i < length; i++) {
        res += ((*iPtr++) * (*tPtr++));
      }

      result = res;
    }
  }
}
