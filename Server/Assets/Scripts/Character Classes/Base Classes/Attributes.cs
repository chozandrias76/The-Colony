#region License

// // Attributes.cs
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
using System.Collections.Generic;
using System;

public class Attributes
{

    public enum genders
    {
        Male,
        Female,
        Transgendered
    }
	public int genderSelect = 0;
	public string gender {
		get{
			return Enum.GetName(typeof(Attributes.genders), genderSelect);
		}
		set{
			if(Enum.IsDefined(typeof(Attributes.genders), value))
			{
				string[] genderStrings = Enum.GetNames(typeof(Attributes.genders));
				for(int i = 0; i < genderStrings.Length; i++)
				{
					if(genderStrings[i] == value)
					{
						genderSelect = i;
						break;
					}
				}
			}
		}
	}
    public enum mutations
    {
        Xray,
        Telekenesis,
        Hulk,
    }

    public enum races
    {
        Human,
        Surrogate,
        Xenomorph,
        Dog,
        Cat,
        Monkey
    }
	public int raceSelect = 0;
	public string race {
		get{
			return Enum.GetName(typeof(Attributes.races), raceSelect);
		}
		set{
			if(Enum.IsDefined(typeof(Attributes.races), value))
			{
				string[] raceStrings = Enum.GetNames(typeof(Attributes.races));
				for(int i = 0; i < raceStrings.Length; i++)
				{
					if(raceStrings[i] == (string)value)
					{
						raceSelect = i;
						break;
					}
				}
			}
		}
	}

	public Blood ourBlood;
	public Item chestImplant;
	public bool brainAttached = true;

    public CharacterPhysicalAttributes ourPhysicals = new CharacterPhysicalAttributes();
    public CharacterVitals ourVitals = new CharacterVitals();
	public List<BaseDisease> ourDiseases = new List<BaseDisease>();


    private void Start()
    {

    }

    public void GiveDisease(int disease)
    {
    }

    public void RemoveDisease(int disease)
    {
    }
    // Update is called once per frame
    private void Update()
    {
    }
}