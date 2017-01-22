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

using System;

namespace AudioBenchDSP.IIR {
  class IIRKernel {
    double[] ffTaps;
    double[] fbTaps;
    int latestF;
    int latestB;
    double[] latestOutput;
    double[] latestInput;

    public IIRKernel(double[] fftaps, double[] fbtaps) {
      SetTaps(fftaps, fbtaps);
      latestB = 0;
      latestF = 0;
    }
    public IIRKernel(float[] fftaps, float[] fbtaps) {
      SetTaps(fftaps, fbtaps);
      latestB = 0;
      latestF = 0;
    }

    public float Filter(float input) {
      double acc;
      uint i = 0;
      int n = ffTaps.Length;
      int m = fbTaps.Length;

      if (n == 0) {
        return 0;
      }

      int tmpLatestF = latestF;
      int tmpLatestB = latestB;

      acc = ffTaps[0] * input;
      for (i = 1; i < n; i++) {
        acc += (ffTaps[i] * latestInput[tmpLatestF + i]);
      }

      for (i = 1; i < m; i++) {
        acc += (fbTaps[i] * latestOutput[tmpLatestB + i]);
      }

      latestOutput[tmpLatestB] = acc;
      latestOutput[tmpLatestB + m] = acc;
      latestInput[tmpLatestF] = input;
      latestInput[tmpLatestF + n] = input;

      tmpLatestF--;
      tmpLatestB--;
      if (tmpLatestF < 0) {
        tmpLatestF += n;
      }

      if (tmpLatestB < 0) {
        tmpLatestB += m;
      }

      latestB = tmpLatestB;
      latestF = tmpLatestF;
      return (float)acc;
    }

    public void FilterN(ref float[] output, float[] input, int n) {
      for (int i = 0; i < n; i++) {
        output[i] = Filter(input[i]);
      }
    }

    public void SetTaps(float[] fftaps, float[] fbtaps) {
      double[] _ff = new double[fftaps.Length];
      double[] _fb = new double[fbtaps.Length];

      for (int i = 0; i < _ff.Length; i++) {
        _ff[i] = fftaps[i];
      }

      for (int i = 0; i < _fb.Length; i++) {
        _fb[i] = fbtaps[i];
      }

      SetTaps(_ff, _fb);
    }

    public void SetTaps(double[] fftaps, double[] fbtaps) {
      latestF = 0;
      latestB = 0;
      ffTaps = fftaps;

      Array.Resize(ref fbTaps, fbtaps.Length);
      fbTaps[0] = fbtaps[0];
      for (int i = 1; i < fbtaps.Length; i++) {
        fbTaps[i] = -fbtaps[i];
      }

      int n = fftaps.Length;
      int m = fbtaps.Length;
      latestInput = new double[2 * n];
      latestOutput = new double[2 * m];

      for (int i = 0; i < n; i++) {
        latestInput[i] = 0;
      }
      for (int i = 0; i < m; i++) {
        latestOutput[i] = 0;
      }
    }
  }
}
