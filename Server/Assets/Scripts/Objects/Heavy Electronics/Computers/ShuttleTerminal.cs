#region License

// // ShuttleTerminal.cs
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
using UnityEngine;

public class ShuttleTerminal : BaseTerminal
{
	//private string computerText;
	private int shuttleLaunchActivations = 3;
	//public ConsoleScreen terminalScreen = new ConsoleScreen();
	
	public ShuttleTerminal ()
	{
		requiredClearance = (int)SecurityStatus.JobSecurityClearance.None;
		tertiaryClearance = (int)SecurityStatus.GeneralSecurityClearance.Tir1;
		terminalName = "Shuttle";
		baseMenu = "0)Schedule immediate departure/n " +
                        "1)Current status of station/n " +
                        "2)Next scheduled shuttle departure";
		baseMenuChoices = 3;
	}
	
	protected override void BaseMenuChoice0 ()
	{
			
		if (playersInsertedBadge.badgeClearance[1] >= (int)SecurityStatus.GeneralSecurityClearance.Tir2) {
			shuttleLaunchActivations--;
			if (shuttleLaunchActivations >= 0) {
				computerText =
                                String.Format ("Access granted. {0} Activations required until shuttle launch",
                                    shuttleLaunchActivations);
				deviceScreen.WriteExternalString (computerText);
			} else if (shuttleLaunchActivations == 0) {
				computerText = "Access granted. Shuttle will launch in T-10 seconds and counting.";
				deviceScreen.WriteExternalString (computerText);
				shuttleLaunchActivations = 3;
			}
		}
			
		
	}
	
	protected override void BaseMenuChoice1 ()
	{
		computerText = StationData.stationStatusCurrent;
		deviceScreen.WriteExternalString (computerText);
		if (Input.anyKeyDown) {
			base.Main ();
		}
	}
	
	protected override void BaseMenuChoice2 ()
	{
		if (StationData.currentStatus == 3) {
			deviceScreen.ClearScreen ();
			computerText = String.Format ("The shuttle has already departed. There is {0}ms until arrival",
                    StationData.TimeLeft);
			deviceScreen.WriteExternalString (computerText);
		} else if (StationData.currentStatus == 4) {
			deviceScreen.ClearScreen ();
			computerText = String.Format ("The shuttle is on it's way. It will arrive in {0}ms.",
                    StationData.TimeLeft);
			deviceScreen.WriteExternalString (computerText);
		}
		base.Main ();
	}
	
	public override void Main ()
	{
		base.Main ();
	}

	private void Start ()
	{
	}

	private void Update ()
	{
	}
}