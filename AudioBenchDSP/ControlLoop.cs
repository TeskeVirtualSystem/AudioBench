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

namespace AudioBenchDSP {
  public class ControlLoop {
    private readonly static float M_TWOPI = (float)Math.PI * 2;
    protected float dampingFactor;
    protected float phase;
    protected float loopBw;
    #region Properties
    public float Phase {
      get {
        return phase;
      }
      set {
        phase = value;
        PhaseWrap();
      }
    }
    public float DampingFactor {
      get {
        return dampingFactor;
      }
      set {
        if (value <= 0) {
          throw new ArgumentException("Invalid value for Damping Factor \"{0}\": It should be higher than or equal to 0");
        }
        dampingFactor = value;
        UpdateGains();
      }
    }
    public float MaxFrequency { get; set; }
    public float MinFrequency { get; set; }
    public float Alpha { get; set; }
    public float Beta { get; set; }
    public float Frequency { get; set; }
    public float LoopBandwidth {
      get {
        return loopBw;
      }
      set {
        if (value < 0) {
          throw new ArgumentException("Invalid value for Loop Bandwidth \"{0}\": It should be higher than 0");
        }

        loopBw = value;
        UpdateGains();
      }
    }
    #endregion
    public ControlLoop(float loopBw, float maxFreq, float minFreq) {
      MaxFrequency = maxFreq;
      MinFrequency = minFreq;
      Phase = 0;
      Frequency = 0;
      DampingFactor = (float)(Math.Sqrt(2.0) / 2.0);
      LoopBandwidth = loopBw;
    }
    #region Methods
    public void FrequencyLimit() {
      if (Frequency > MaxFrequency) {
        Frequency = MaxFrequency;
      } else if (Frequency < MinFrequency) {
        Frequency = MinFrequency;
      }
    }

    public void PhaseWrap() {
      while (phase > M_TWOPI) {
        phase -= M_TWOPI;
      }
      while (Phase < -M_TWOPI) {
        phase += M_TWOPI;
      }
    }

    public void AdvanceLoop(float error) {
      Frequency = Frequency + Beta * error;
      phase = phase + Frequency + Alpha * error;
    }

    public void UpdateGains() {
      float denom = (1.0f + 2.0f * dampingFactor * loopBw + loopBw * loopBw);
      Alpha = (4 * dampingFactor * loopBw) / denom;
      Beta = (4 * loopBw * loopBw) / denom;
    }
    #endregion
  }
}
