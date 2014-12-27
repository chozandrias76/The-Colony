#region License

// // BaseTerminal.cs
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
using System.Collections;

public class BaseTerminal : HeavyElectronic
{    
	protected IDBadge playersInsertedBadge;
	protected string terminalName = "Base";
	protected int requiredClearance;
	protected int tertiaryClearance;
    public bool isaTerminal;
	//protected int possibleAttachableDevices;
	protected List<HeavyElectronic> possibleAttachableDevices = new List<HeavyElectronic>();
	protected string baseMenu;
	protected int baseMenuChoices;
	
	protected BitArray dynamicMenuIndex
	{
		get
		{
			BitArray objectsToArray = new BitArray (attachedDevices.Count, true);
			return objectsToArray;
		}
		set
		{
		}
	}
	
	protected string objectMenu
	{
		get{
			string _objectMenu = "";
			for(int i = 0; i < attachedDevices.Count; i++)
			{
				_objectMenu += String.Format("Access {0}/n",attachedDevices[i].name);
			}
			return _objectMenu;
		}
		set{}
	}
	
	protected virtual void BaseMenuChoice0()
	{
	}
	
	protected virtual void BaseMenuChoice1()
	{
	}
	
	protected virtual void BaseMenuChoice2()
	{
	}
	
	protected virtual void BaseMenuChoice3()
	{
	}
	
	protected virtual void BaseMenuChoice4()
	{
	}
	
	protected virtual void BaseMenuChoice5()
	{
	}
	
	protected virtual void BaseMenuChoice6()
	{
	}
	
	protected virtual void BaseMenuChoice7()
	{
	}
	
	protected virtual void BaseMenuChoice8()
	{
	}
	
	protected virtual void BaseMenuChoice9()
	{
	}
	
    // Use this for initialization
    private void Start()
    {
    }

	public override void Main()
	{
		deviceScreen.ClearScreen();
		computerText = String.Format(
            "Welcome to the Main Menu for your {0} Terminal. Please navigate by pressing the corrisponding number to access a sub menu./n", terminalName);
		computerText += baseMenu;
		computerText += objectMenu;
		deviceScreen.WriteExternalString(computerText);
		bool inMainMenu = true;
		while (inMainMenu) {
			if (Input.anyKeyDown) {
				deviceScreen.ClearScreen ();
			}
			string input = Input.inputString;
			int inputValue = Convert.ToInt32 (input [0]);
			if (inputValue >= 48 && inputValue <= 57) {
				if (inputValue - 48 < baseMenuChoices) {//For basemenu
					if (inputValue - 48 == 0) {
						BaseMenuChoice0 ();
					}
					
					if (inputValue - 48 == 1) {
						BaseMenuChoice1 ();
					}
					
					if (inputValue - 48 == 2) {
						BaseMenuChoice2 ();
					}
					
					if (inputValue - 48 == 3) {
						BaseMenuChoice3 ();
					}
					
					if (inputValue - 48 == 4) {
						BaseMenuChoice4 ();
					}
					
					if (inputValue - 48 == 5) {
						BaseMenuChoice5 ();
					}
					
					if (inputValue - 48 == 6) {
						BaseMenuChoice6 ();
					}
				
					if (inputValue - 48 == 7) {
						BaseMenuChoice7 ();
					}
					
					if (inputValue - 48 == 8) {
						BaseMenuChoice8 ();
					}
					
					if (inputValue - 48 == 9) {
						BaseMenuChoice9 ();
					}
					
				}
				if (inputValue - 48 >= baseMenuChoices) {//For objectmenu 0 through 5 max
					if (dynamicMenuIndex.Get (inputValue - 48)) {
						deviceScreen.WriteExternalString (String.Format ("Interfacing with the {0}", attachedDevices [inputValue - 48].ToString ()));
						attachedDevices [inputValue - 48].Main ();
					}
				}
			}
		}
	}
	
	public virtual void OnPlayerUsing(PlayerCharacter pc)
	{
		if(pc.playerInventory.thisHandItems[0].GetType() != typeof(IDBadge))
		{
			if(pc.playerInventory.thisHandItems[1].GetType() != typeof(IDBadge))
			{
				computerText = "Please have your card in a hand to start using this terminal";
				deviceScreen.WriteExternalString(computerText);
			}
			else if(pc.playerInventory.thisHandItems[1].GetType() == typeof(IDBadge))
			{
				playersInsertedBadge = pc.playerInventory.thisHandItems[1] as IDBadge;
			}
		}
		else if(pc.playerInventory.thisHandItems[0].GetType() == typeof(IDBadge))
		{
			playersInsertedBadge = pc.playerInventory.thisHandItems[0] as IDBadge;
		}
	}
	
	protected virtual void CheckPrivledge()
	{
		if(playersInsertedBadge.badgeClearance[0] == requiredClearance || playersInsertedBadge.badgeClearance[1] >= tertiaryClearance)
		{
			Main();
		}
	}
	
	public override void OnOtherUsingThis (Item usedWith)
	{
		//base.OnOtherUsingThis (usedWith);
		
	}
	
	public void StopUsingTerminal()
	{

	}

    // Update is called once per frame
    private void Update()
    {
    }
}