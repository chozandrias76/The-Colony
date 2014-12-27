#region License

// // ObjectData.cs
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

using UnityEngine;
using System;
using System.Collections;

public class ObjectData
{
//    public bool[] connections =
//    {
//        false,
//        false,
//        false,
//        false
//    };
	
	public BitArray connections = new BitArray(4,false);

    public Vector3 direction;
    public bool exists = false;
    public Vector3 location = new Vector3(0, 100000, 0);
}