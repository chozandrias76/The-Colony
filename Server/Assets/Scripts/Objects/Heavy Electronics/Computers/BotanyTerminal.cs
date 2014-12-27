#region License

// // BotanyTerminal.cs
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
using System.Collections;
using System;

public class BotanyTerminal : BaseTerminal
{
	public BotanyTerminal()
	{
		requiredClearance = (int)SecurityStatus.JobSecurityClearance.GreenHouse;
		tertiaryClearance = (int)SecurityStatus.GeneralSecurityClearance.Tir2;
		terminalName = "Botany";
		baseMenu = "0)Query current status of your section/n" +
                        "1)Query health of plants/n " +
                        "2)Query current status of shuttle/n";
		baseMenuChoices = 3;
	}
	
    // Use this for initialization
    private void Start()
    {
    }
	
	public override void Main ()
	{
		base.Main ();
	}

    // Update is called once per frame
    private void Update()
    {
    }
}