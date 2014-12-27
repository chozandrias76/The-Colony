#region License

// // MedicalTerminal.cs
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
using System.Collections.Generic;
using UnityEngine;

public class MedicalTerminal : BaseTerminal
{
	public MedicalTerminal ()
	{
		requiredClearance = (int)SecurityStatus.JobSecurityClearance.Medical;
		tertiaryClearance = (int)SecurityStatus.GeneralSecurityClearance.Tir2;
		terminalName = "Medical";
		baseMenu = "0)Query current status of your section/n" +
                        "1)Current status of shuttle/n " +
                        "2)Current health status of crew members/n";
		baseMenuChoices = 3;
	}


	// Use this for initialization
	private void Start ()
	{
	}
	
	protected override void BaseMenuChoice0 ()
	{
		List<string> medDataStrings = StationData.RestrictedData01.GetData (22);

		foreach (string medDataString in medDataStrings) {
			computerText += medDataString;
		}
		computerText += "Press any key to return to the main menu...";
		deviceScreen.WriteExternalString (computerText);

		if (Input.anyKeyDown) {
			base.Main ();
		}
	}
	
	protected override void BaseMenuChoice1 ()
	{
		var _stationData = new StationData ();
		computerText = _stationData.CurrentStatus;
		deviceScreen.WriteExternalString (computerText);
		if (Input.anyKeyDown) {
			base.Main ();
		}
	}
	
	protected override void BaseMenuChoice2 ()
	{
		//List<string> medicalRecords = StationData.StringsFromMedicalRecords(StationData.QueryForMedical (new SQLite.SQLiteConnection(Application.dataPath + "The Colony Server.db")));
		//foreach(string playerRecord in medicalRecords)
		//{
		//    computerText += playerRecord + "/n";
		//}
		deviceScreen.WriteExternalString (computerText);
		if (Input.anyKeyDown) {
			base.Main ();
		}
	}

	public override void Main ()
	{
		base.Main ();
	}

	// Update is called once per frame
	private void Update ()
	{
	}
}