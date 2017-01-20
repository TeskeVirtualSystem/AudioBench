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

namespace AudioBenchDSP.FIR {
  public class FirFilter<T> where T : struct {
    private FirKernel kernel;
    private T[] samples;
    private int sampleHistory;
    private int decimation;

    public FirFilter(int decimation, float[] taps) {
      this.kernel = new FirKernel(taps);
      this.sampleHistory = taps.Length;
      this.decimation = decimation;
      samples = new T[sampleHistory];
      for (int i = 0; i < sampleHistory; i++) {
        samples[i] = new T();
      }
    }
    
    public int Work(T[] input, ref T[] output, int length) {
      if (samples.Length < length * decimation + sampleHistory) {
        T[] oldHistory = samples;
        samples = new T[length * decimation + sampleHistory];
        for (int i = 0; i < oldHistory.Length; i++) {
          samples[i] = oldHistory[i];
        }
      }

      for (int i = 0; i < length * decimation; i++) {
        samples[sampleHistory + i] = input[i];
      }

      if (decimation > 1) {
        kernel.FilterDecimating(samples, output, length, decimation);
      } else {
        kernel.Filter(samples, output, length);
      }

      for (int i = 0; i < sampleHistory; i++) {
        samples[i] = input[length * decimation - sampleHistory + i];
      }

      return length;
    }
  }
}
