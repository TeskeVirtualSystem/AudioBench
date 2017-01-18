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

namespace AudioBenchDSP.Funcs {
  public static class Filters {
    /// <summary>
    /// Simple Low Pass Filter
    /// </summary>
    /// <param name="sampleRate">The sampleRate</param>
    /// <param name="cutFreq">The Cut Frequency</param>
    /// <param name="length">Length of the Filter (Odd Number)</param>
    /// <returns></returns>
    public static float[] simpleLowPass(float sampleRate, float cutFreq, int length) {
      length += (length + 1) % 2;
      float[] taps = new float[length];
      var freq = cutFreq / sampleRate;
      var center = Math.Floor(length / 2.0);
      var sum = 0.0;

      for (var i = 0; i < length; ++i) {
        double val;
        if (i == center) {
          val = 2 * Math.PI * freq;
        } else {
          var angle = 2 * Math.PI * (i + 1) / (length + 1);
          val = Math.Sin(2 * Math.PI * freq * (i - center)) / (i - center);
          val *= 0.42 - 0.5 * Math.Cos(angle) + 0.08 * Math.Cos(2 * angle);
        }
        sum += val;
        taps[i] = (float)val;
      }

      for (var i = 0; i < length; ++i) {
        taps[i] /= (float)sum;
      }

      return taps;
    }
  }
}
