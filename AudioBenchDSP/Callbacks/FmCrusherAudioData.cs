using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBenchDSP.Callbacks {
  public class FmCrusherAudioData {
    public float[] audioData;
    public int rate;

    public FmCrusherAudioData(float[] audioData, int rate) {
      this.audioData = audioData;
      this.rate = rate;
    }
  }
}
