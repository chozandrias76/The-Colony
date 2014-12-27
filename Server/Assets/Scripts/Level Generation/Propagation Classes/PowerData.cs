#region License

// // PowerData.cs
// //  
// //  Author:
// //        <colin.p.swensonh@gmail.com>
// // 
// //  Copyright (c) 2013 swensonhcp
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion
using System.Collections.Generic;
using System;
using UnityEngine;
public class PowerData : ObjectData
{
	//public System.Collections.BitArray powerDirections = new System.Collections.BitArray(4,false);
    public float amps;
	public List<PowerConsole> activePowerConsoles;
	public List<HeavyElectronic> electronicsAttached = new List<HeavyElectronic>();
}