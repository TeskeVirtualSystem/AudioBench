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

using AudioBenchDSP.PortAudio;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AudioBench {
  public partial class Main : Form {
    public Main() {
      InitializeComponent();
      List<AudioDevice> audioDevices = AudioDevice.GetDevices(AudioDeviceDirection.Output);
      foreach (AudioDevice d in audioDevices) {
        Console.WriteLine(d.Name);
      }
    }
  }
}
