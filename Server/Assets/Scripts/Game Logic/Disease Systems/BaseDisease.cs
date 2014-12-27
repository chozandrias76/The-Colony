#region License

// // BaseDisease.cs
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

public class BaseDisease
{
	public enum InfectionSpreadType
	{
		None,
		Contact,
		Airborne,
		Fluids
	}

	public enum diseases
	{
		Alopecia,
		ABMetabolism,
		EternalYouth,
		FacialHypertrichosis,
		Longevity,
		NecrotizingFasciitis,
		SelfRespiration,
		ToxicCompensation,
		ToxinFilter,
		VoiceChange,
		WeightEven,
		Myopia
	}
	
	public enum Symptoms
	{
		Choking,
		Coughing,
		Confusion,
		Deafness,
		Exhaustion,
		Itching,
		Dizziness,
		Fever,
		Hallucinogen,
		Headache,
		Shivering,
		SoreThroat,
		Sneezing,
		Stimulant,
		Stuffy,
		Thirst,
		Vomiting,
		VomitingBlood,
		WeightGain,
		WeightLoss,
		Lethargy,
	}
	

	public bool deadly = false;
	public bool diseaseIsAlive = true;
	public float diseaseLife;
	public float infectionDistance; //In unity units
	public float infectionHalfLife; //In Seconds
	public float infectionRate; //In seconds
	public List<string> spreadTypes = new List<string> ();
	//public List<string> symptoms = new List<string> ();
	public List<Symptom> symptoms = new List<Symptom>();

	private void Start ()
	{
	}

	private void Update ()
	{
		if (Time.time % 1.0f == 0) {
			if (diseaseLife > 0) {
				diseaseLife--;
			} else if (diseaseLife <= 0) {
				diseaseIsAlive = false;
			}
		}
	}

	public void OnDiseaseSpreadIn (string spreadType, BaseDisease disease, CharacterVitals playerVitals)
	{
	}

	public void OnDiseaseSpreadOut (string spreadType, BaseDisease disease, CharacterVitals playerVitals)
	{
	}
}