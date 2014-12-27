#region License

// // EngineeringTerminal.cs
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
using System.Collections;
using UnityEngine;

public class EngineeringTerminal : BaseTerminal
{
	public EngineeringTerminal ()
	{
		requiredClearance = (int)SecurityStatus.JobSecurityClearance.Engineering;
		tertiaryClearance = (int)SecurityStatus.GeneralSecurityClearance.Tir2;
		terminalName = "Enginering";
		baseMenu = "0)Query current status of your section/n" +
                        "1)Current status of station power/n " +
                        "2)Current status of reactor/n";
		baseMenuChoices = 3;
	}

	public ReactorCore stationReactor;
	
	protected override void BaseMenuChoice0 ()
	{
		computerText = "";
		List<string> engineeringDataStrings = StationData.RestrictedData01.GetData (21);
		foreach (string engineeringDataString in engineeringDataStrings) {
			computerText += engineeringDataString;
		}
		computerText += "Press any key to return to the main menu...";
		deviceScreen.WriteExternalString (computerText);
	}
	
	protected override void BaseMenuChoice1 ()
	{
		computerText = "";
//				foreach(PowerConsole activeConsoles in ObjectDataTracker.Singleton.powerCords .activePowerConsoles)
//				{
//					computerText = System.String.Format("Lighting Online: {0} Atmospherics Online: {1} Electronics Online: {2} Power Usage: {3} Location: {4}/n"
//					                                    ,activeConsoles.subSystemsOnline[0]
//					                                    ,activeConsoles.subSystemsOnline[1]
//					                                    ,activeConsoles.subSystemsOnline[2]
//					                                    ,activeConsoles.powerUsage
//					                                    ,activeConsoles.areaLocation);
//					
//				}
				
		computerText += "Press any key to return to the main menu.../n";
		deviceScreen.WriteExternalString (computerText);
	}
	
	protected override void BaseMenuChoice2 ()
	{
		computerText = "";
		if (stationReactor != null) {
			computerText = stationReactor.status;
		} else {
			computerText = "Reactor is missing!";
		}
		computerText += "Press any key to return to the main menu.../n";
		deviceScreen.WriteExternalString (computerText);
	}
	
	public override void Main ()
	{
		base.Main ();
	}
	
	// Use this for initialization
	private void Start ()
	{
	}

	// Update is called once per frame
	private void Update ()
	{
	}
}