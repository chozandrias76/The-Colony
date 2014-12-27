#region License

// // ComputerTerminal.cs
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
using uLink;

public class ComputerTerminal : Item
{
    public enum TerminalTypes
    {
        Generic = 0,
        Science,
        Medical,
        Engineering,
        Botany,
        Head,
        Captain,
        HoP,
        Admin
    };

    public int _terminalType = (int) TerminalTypes.Generic;

    public bool isaTerminal = false;

    public IDBadge terminalBadge;
    public ConsoleScreen terminalScreen = new ConsoleScreen();
    private string terminalString;

    public override string ToString()
    {
        return "Terminal";
    }

    public override void OnUsing(PlayerCharacter pc)
    {
		var pcBadge = new Item();
		if(pc.playerInventory.inventoryItems.TryGetValue(pc.playerInventory.bodyLocations[(int)Inventory.BodyLocations.ID], out pcBadge))
		{
			terminalBadge = pcBadge as IDBadge;
			CheckPrivledge();
		}
    }

    public void InsertBadge(IDBadge playerBadge)
    {
        terminalBadge = playerBadge;
        CheckPrivledge();
    }

    public void CheckPrivledge()
    {
        //int _terminalType = terminalType;
        if (_terminalType == 0)
        {
        }
        int[] playerSecurityStatus = SecurityStatus.GetJobClearance(terminalBadge.playerJob);

        switch (_terminalType)
        {
                #region Generic Terminal

            case 0:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel < 32 && clearanceLevel != 4)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Science Terminal

            case 1:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if ((clearanceLevel > 14
                         && clearanceLevel < 19)
                        || (clearanceLevel == 26
                            || clearanceLevel > 28))
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Medical Terminal

            case 2:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel == 13
                        || clearanceLevel == 14
                        || clearanceLevel == 25
                        || clearanceLevel > 28)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var medTerminal = new MedicalTerminal();

                        //GenericTerminal genTerminal = new GenericTerminal ();
                        //genTerminal.terminalScreen = this.terminalScreen;
                        //genTerminal.Main ();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Engineering Terminal

            case 3:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel == 11
                        || clearanceLevel == 24
                        || clearanceLevel > 28)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Botany Terminal

            case 4:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel == 4
                        || clearanceLevel > 28)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Heads Terminal

            case 5:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel > 21)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Captain Terminal

            case 6:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel > 29)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region HoP Terminal

            case 7:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel > 28)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

                #region Admin Terminal

            case 8:
                foreach (int clearanceLevel in playerSecurityStatus)
                {
                    if (clearanceLevel > 30)
                    {
                        terminalString = String.Format("Access Granted {0}", terminalBadge.playerName);
                        terminalScreen.WriteExternalString(terminalString);
                        var genTerminal = new GenericTerminal();
                        genTerminal.terminalScreen = terminalScreen;
                        genTerminal.Main();
                        break;
                    }
                    terminalString = "Error: Your privledge is not elevated high enough." +
                                     " Please contact your head if this is in error";
                }
                break;

                #endregion

            default:
                terminalString = "Error: Your privledge is not elevated high enough." +
                                 " Please contact your head if this is in error";
                break;
        }
    }

    private void Start()
    {
        terminalString = "Please insert ID Badge to continue.";
    }

    private void Update()
    {
    }
}