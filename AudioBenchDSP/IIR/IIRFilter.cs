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

namespace AudioBenchDSP.IIR {
  public class IIRFilter {
    private IIRKernel iir;

    public IIRFilter(float[] ffTaps, float[] fbTaps) {
      iir = new IIRKernel(ffTaps, fbTaps);
    }

    public IIRFilter(double[] ffTaps, double[] fbTaps) {
      iir = new IIRKernel(ffTaps, fbTaps);
    }

    public void SetTaps(float[] ffTaps, float[] fbTaps) {
      iir.SetTaps(ffTaps, fbTaps);
    }
    public void SetTaps(double[] ffTaps, double[] fbTaps) {
      iir.SetTaps(ffTaps, fbTaps);
    }

    public void Work(float[] input, ref float[] output, int length) {
      iir.FilterN(ref output, input, length);
    }
  }
}
