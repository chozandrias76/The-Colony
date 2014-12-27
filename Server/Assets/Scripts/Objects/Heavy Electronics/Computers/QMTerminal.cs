#region License

// // QMTerminal.cs
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
using System.Collections.Generic;
using UnityEngine;

public class QMTerminal : BaseTerminal
{
	public QMTerminal ()
	{
		requiredClearance = (int)SecurityStatus.JobSecurityClearance.CargoHead;
		tertiaryClearance = (int)SecurityStatus.GeneralSecurityClearance.Tir2;
		terminalName = "Quarter Master";
		baseMenu = "0)Query current status of your section/n" +
                        "1)Current status of shuttle/n";
		baseMenuChoices = 2;
	}
	
	public void OnShuttleIncomming ()
	{
		QMData.shuttleStatusCurrent = "Shuttle now inbound to loading dock.";
		QMData.shuttleCalledTime = QMData.currentTime;
	}

	private void Awake ()
	{
	}

	private void Start ()
	{
	}
	
	protected override void BaseMenuChoice0 ()
	{
		List<string> qmDataStrings = StationData.RestrictedData01.GetData (19);

		foreach (string qmDataString in qmDataStrings) {
			computerText += qmDataString;
		}
		computerText += "Press any key to return to the main menu...";
		deviceScreen.WriteExternalString (computerText);

		if (Input.anyKeyDown) {
			deviceScreen.ClearScreen ();
			base.Main ();
		}
	}
	
	protected override void BaseMenuChoice1 ()
	{
		GetShuttleStatus ();
		if (Input.anyKeyDown) {
			deviceScreen.ClearScreen ();
			base.Main ();
		}
	}
	
	public override void Main ()
	{
		base.Main();
	}

	void GetShuttleStatus ()
	{
		if (QMData.currentStatusIndex == 0) {
			deviceScreen.ClearScreen ();
			computerText = String.Format ("The shuttle has already departed. There is {0}ms until arrival",
                    QMData.TimeLeft);
			deviceScreen.WriteExternalString (computerText);
			deviceScreen.WriteExternalString ("/nPress any key to go back");

			while (!Input.anyKeyDown) {

			}
			Start ();

		} else if (QMData.currentStatusIndex == 1) {
			deviceScreen.ClearScreen ();
			computerText = String.Format ("The shuttle is on it's way. It will arrive in {0}ms.",
                    QMData.TimeLeft);
			deviceScreen.WriteExternalString (computerText);
			deviceScreen.WriteExternalString ("/nPress any key to go back");

			while (!Input.anyKeyDown) {

			}
			Start ();
		}
	}

	private void Update ()
	{
       
	}
}