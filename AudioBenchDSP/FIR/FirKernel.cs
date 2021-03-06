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
using AudioBenchDSP.Funcs;
using AudioBenchDSP.Types;

namespace AudioBenchDSP.FIR {
  class FirKernel {
    private DataBuffer tapsBuffer;
    private unsafe float* taps;
    private int tapsLength;

    public FirKernel(float[] taps) {
      SetTaps(taps);
    }

    public void SetTaps(float[] taps) {
      tapsBuffer = DataBuffer.Create(taps.Length, sizeof(float));
      CopyTapsToBuffer(taps);
      tapsLength = taps.Length;
    }

    public void Filter(Complex[] input, Complex[] output, int length) {
      _Filter(input, output, length);
    }

    public void FilterDecimating(Complex[] input, Complex[] output, int length, int decimate) {
      _FilterDecimating(input, output, length, decimate);
    }

    #region Unsafe Functions
    private unsafe void CopyTapsToBuffer(float[] taps) {
      this.taps = (float*)tapsBuffer;
      for (int i = 0; i < taps.Length; i++) {
        this.taps[i] = taps[i];
      }
    }

    public Complex Filter(Complex[] input) {
      return _Filter(input);
    }

    public void FilterDecimating<T>(T[] input, T[] output, int length, int decimate) where T : struct {
      if (typeof(T) == typeof(float)) {
        _FilterDecimating((float[])(object)input, (float[])(object)output, length, decimate);
      } else if (typeof(T) == typeof(Complex)) {
        _FilterDecimating((Complex[])(object)input, (Complex[])(object)output, length, decimate);
      } else {
        throw new NotImplementedException();
      }
    }

    internal void Filter<T>(T[] samples, T[] output, int length) where T : struct {
      if (typeof(T) == typeof(float)) {
        Filter((float[])(object)samples, (float[])(object)output, length);
      } else if (typeof(T) == typeof(Complex)) {
        Filter((Complex[])(object)samples, (Complex[])(object)output, length);
      } else {
        throw new NotImplementedException();
      }
    }

    private unsafe void _Filter(Complex[] input, Complex[] output, int length) {
      fixed (Complex* iPtr = &input[0]) {
        for (uint i = 0; i < length; i++) {
          output[i] = _Filter(&iPtr[i]);
        }
      }
    }
    public unsafe void _FilterDecimating(Complex[] input, Complex[] output, int length, int decimate) {
      int j = 0;
      fixed (Complex* iPtr = &input[0]) {
        for (uint i = 0; i < length; i++) {
          output[i] = _Filter(&iPtr[j]);
          j += decimate;
        }
      }
    }
    public unsafe void _FilterDecimating(float[] input, float[] output, int length, int decimate) {
      int j = 0;
      fixed (float* iPtr = &input[0]) {
        for (uint i = 0; i < length; i++) {
          output[i] = _Filter(&iPtr[j]);
          j += decimate;
        }
      }
    }
    private unsafe Complex _Filter(Complex* input) {
      Complex o;
      VMath._DotProduct(out o, input, taps, tapsLength);
      return o;
    }
    private unsafe float _Filter(float* input) {
      float o;
      VMath._DotProduct(out o, input, taps, tapsLength);
      return o;
    }
    private unsafe Complex _Filter(Complex[] input) {
      fixed (Complex* iPtr = &input[0]) {
        return _Filter(iPtr);
      }
    }
    private unsafe float _Filter(float[] input) {
      fixed (float* iPtr = &input[0]) {
        return _Filter(iPtr);
      }
    }
    #endregion
  }
}
