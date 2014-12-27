#region License

// // Rhinovirus.cs
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
using System;
using System.Collections;
using UnityEngine;

public class RhinoVirus : BaseDisease
{	
	public RhinoVirus()
	{
//		
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Coughing));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Headache));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.SoreThroat));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Sneezing));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Stuffy));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Lethargy));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Exhaustion));
//		symptoms.Add(Enum.GetName(typeof(BaseDisease.Symptoms),BaseDisease.Symptoms.Thirst));
//	
		spreadTypes.Add(Enum.GetName(typeof(BaseDisease.InfectionSpreadType), BaseDisease.InfectionSpreadType.Airborne));
		spreadTypes.Add(Enum.GetName(typeof(BaseDisease.InfectionSpreadType), BaseDisease.InfectionSpreadType.Contact));
		spreadTypes.Add(Enum.GetName(typeof(BaseDisease.InfectionSpreadType), BaseDisease.InfectionSpreadType.Fluids));
		infectionDistance = 500.0f;
        infectionHalfLife = 180.0f;
        infectionRate = 60.0f;

	}

    private void Start()
    {
        
    }

    private void Update()
    {
    }
}