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
using System;

namespace AudioBenchDSP.Callbacks {
  /// <summary>
  /// SamplesAvailable Event Argument
  /// </summary>
  public unsafe class UnsafeSamplesEventArgs : EventArgs {
    #region Constructors
    public UnsafeSamplesEventArgs() { }
    public UnsafeSamplesEventArgs(float* floatBuff, int length) {
      IsComplex = false;
      FloatBuffer = floatBuff;
      Length = length;
    }
    public UnsafeSamplesEventArgs(Complex* complexBuff, int length) {
      IsComplex = true;
      ComplexBuffer = complexBuff;
      Length = length;
    }
    #endregion
    #region Properties
    public bool IsComplex { get; set; }
    public unsafe Complex* ComplexBuffer { get; set; }
    public unsafe float* FloatBuffer { get; set; }
    public int Length { get; set; }
    #endregion
  }
}
