#region License

// // CharacterPhysicalAttributes.cs
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

public class CharacterPhysicalAttributes
{
    public Color32 hairColor = new Color32(0, 0, 0, 255);
	public Color32 facialHairColor = new Color32(0, 0, 0, 255);
    public Color32 skinTone = new Color32(0, 0, 0, 255);
	public Color32 eyeColor = new Color32(0, 0, 0, 255);
	public int facialHair = 0;
	public int hair = 0;

    private void Start()
    {

    }

    private void Update()
    {
    }

    public enum FacialHair
    {
		
    };

    public enum HairType
    {
    };

    public enum Underwear
    {
    };
}