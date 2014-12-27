#region License

// // CharacterVitals.cs
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
//using System.Data.SqlTypes;

public class CharacterVitals
{
    public enum DamageLocations
    {
        Head,
        Eyes,
        Mouth,
        Ears,
        Throat,
        Chest,
        LArm,
        RArm,
        LHand,
        RHand,
        Groin,
        Legs,
        Feet
    }

    public enum DamageTypes
    {
        Radiation,
        Brute,
        Burn,
        Genetic,
        Sharp,
        Oxygen,
        Freeze,
    }

    //public float[][] damageValues = new float[13][7];
	public float[][] damageValues = new float[13][];

    private void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            damageValues[i] = new[]
            {
                0.0f, //Rad
                0.0f, //Brute
                0.0f, //Burn
                0.0f, //Genetic
                0.0f, //Sharp
                0.0f, //Oxygen
                0.0f //Freeze
            };
        }
    }

    private void Update()
    {
    }
}