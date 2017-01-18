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
    public static void DotProduct(out Complex result, Complex[] input, float[] taps, int length) {
      DotProduct(out result, input, taps, length);
    }

    public static unsafe void DotProduct(out Complex result, Complex *input, float *taps, int length) {
      float[] res = { 0, 0 };

      float* iPtr = (float*)input;
      float* tPtr = taps;

      for (int i = 0; i < length; i++) {
        res[0] += ((*iPtr++) * (*tPtr));
        res[1] += ((*iPtr++) * (*tPtr++));
      }

      result = new Complex(res[0], res[1]);
    }
  }
}
