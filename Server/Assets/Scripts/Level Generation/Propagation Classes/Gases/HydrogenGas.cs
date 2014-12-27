#region License

// // Hydrogen.cs
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

public class HydrogenGas : MonoBehaviour
{
//    public DamageInfo Damage = new DamageInfo
//    {
//        bruteDamage = 0.0f,
//        oxyDamage = 0.3f,
//        toxiDamage = 0.0f,
//        heatDamage = 0.0f,
//        cloneDamage = 0.0f,
//        hallucDamage = 0.0f
//    };

    public InteractionInfo Interaction = new InteractionInfo
    {
        lungs = 1.0f,
        hands = 0.0f,
        feet = 0.0f,
        face = 0.0f,
        ingestion = 0.0f,
        injection = 0.0f
    };

    public float transferRate = 1.0f;
}