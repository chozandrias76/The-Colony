#region License

// // CellData.cs
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
using UnityEngine;

public class CellData
{
    //public IDictionary<Vector3, string> cellContents;
    public Connection[] connections = new Connection[4];
    public List<string> contents;
    public List<BaseDisease> diseases;
    public float kPh = 100.0f;
    public Vector3 location;
    public float n02 = 0.8f;
    public float oxy = 0.2f;
    public float plasma = 0.0f;
    public float rad = 0.0f;
    public float scale = 0.0f;
    public float temp = 293.15f;
    public bool transmissive = true;
	
	//public System.Collections.BitArray powerDirections = new System.Collections.BitArray(4,false);
	public PowerData thisPower
	{
		get
		{
			return ObjectDataTracker.Singleton.GetPower(location);
		}
	}

    public override string ToString()
    {
        string s = location.ToString();
        return "The location of this cell is: " + s;
    }
}

public struct Connection
{
    public float breachedScale;
    public bool isDoor;
    public bool isDuct;
    public bool isWall;
    public bool isWire;
}