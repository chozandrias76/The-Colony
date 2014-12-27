#region License

// // PlayerCharacter.cs
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
using System;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour
{
	public int iD;
	public Attributes playerAttributes = new Attributes ();
	public Inventory playerInventory = new Inventory ();
	private string playerJob;
	private string playerName;
	
	bool[] handSelected =
	{
		true,
		false
	};
	
	public int HandSelected 
	{
		get{
			
			if(handSelected[0] == true)
			{
				return 0;
			}
			else if(handSelected[1] == true)
			{
				return 1;
			}
			else
			{
				return -1;
			}
			
		}
		set{
			if(value == 0)
			{
				if(handSelected[value] == true)
				{
				}
				else
				{
					handSelected[value] = true;
					handSelected[value+1] = false;
				}
			}
			else if(value == 1)
			{
				if(handSelected[value] == true)
				{
				}
				else
				{
					handSelected[value] = true;
					handSelected[value-1] = false;
				}
			}
		}
	}

	public PlayerCharacter (string name, string job)
	{
		playerName = name;
		playerJob = job;
		playerInventory.thisBadge = new IDBadge (playerName, playerJob, playerAttributes.gender);
		PlayerTracker.Singleton.AddPlayer (this);
	}

	private void Awake ()
	{
		
//        playerInventory.inventoryItems.Add(Inventory.BodyLocations.ID.ToString(),
//            new IDBadge(playerName, playerJob, playerGender));
	}

	private void OnDestroy ()
	{
		PlayerTracker.Singleton.RemovePlayer (this);
	}

	private void Start ()
	{
	}

	// Update is called once per frame
	private void Update ()
	{
		#region disease propigation
		foreach (Item playerItem in playerInventory.InventoryItems) {//Check every item the player has
			if ((this.playerAttributes.ourDiseases.Count > 0 || playerItem.contractedDiseases.Count > 0) && playerItem.contractedDiseases != playerAttributes.ourDiseases) {//Do something if the player or item has any number of diseases and they don't have exactly the same diseases
				List<BaseDisease> playerDiseases = playerAttributes.ourDiseases;//Create a temp collection of the players diseases for comparison
				
				for (int pDiseaseCnt = 0; pDiseaseCnt < playerDiseases.Count; pDiseaseCnt++) {//Go through every disease this player has
					bool foundPlayerDiseaseOnItem = false;//Checker if compared disease is found on both item and player
					for (int iDiseaseCnt = 0; iDiseaseCnt < playerItem.contractedDiseases.Count; iDiseaseCnt++) {//Go through every disease on this item
						if (playerDiseases [pDiseaseCnt].GetType () == playerItem.contractedDiseases [iDiseaseCnt].GetType ()) {//If the player and the item have the same type of disease
							foundPlayerDiseaseOnItem = true;
							playerItem.contractedDiseases [iDiseaseCnt] = playerDiseases [pDiseaseCnt];//Update the items disease to be current
							break;
						}
					
						if (iDiseaseCnt == playerItem.contractedDiseases.Count && !foundPlayerDiseaseOnItem) {//If we have checked every disease and it isn't found on the item
							playerItem.contractedDiseases.Add (playerDiseases [pDiseaseCnt]);//Add the players disease to the item
						}
					}
					
					bool foundPlayerDiseaseInAir = false;//Checker if atmosphere has the airborne disease
					for(int aDiseaseCnt = 0; aDiseaseCnt < ManagePlayerCellData.playerData.diseases.Count; aDiseaseCnt++)
					{
						if(ManagePlayerCellData.playerData.diseases[aDiseaseCnt].spreadTypes.Contains(Enum.GetName(typeof(BaseDisease.InfectionSpreadType), BaseDisease.InfectionSpreadType.Airborne)))
						{
							if(playerDiseases[pDiseaseCnt].GetType() == ManagePlayerCellData.playerData.diseases[aDiseaseCnt].GetType())
							{
								foundPlayerDiseaseInAir = true;
								break;
							}
							if(aDiseaseCnt == ManagePlayerCellData.playerData.diseases.Count && !foundPlayerDiseaseInAir)
							{
								ManagePlayerCellData.playerData.diseases.Add(playerDiseases[pDiseaseCnt]);
							}
						}
					}
				}
				
				for (int aDiseaseCnt = 0; aDiseaseCnt < ManagePlayerCellData.playerData.diseases.Count; aDiseaseCnt++) {//Go through every disease the air has
					bool foundAirborneDiseaseOnPlayer = false;//Checker if the player has the disease type of the air has
					for (int pDiseaseCnt = 0; pDiseaseCnt < playerDiseases.Count; pDiseaseCnt++) {//Go through every disease the player has
						if (playerDiseases [pDiseaseCnt].GetType () == ManagePlayerCellData.playerData.diseases [iDiseaseCnt].GetType ()) {//If the player and the air have the same type of disease
							foundItemDiseaseOnPlayer = true;//If the player has the same disease as the item
							break;//Do nothing
						}
					
						if (pDiseaseCnt == playerDiseases.Count && !foundAirborneDiseaseOnPlayer) {//If all the players diseases were compared and the player doesn't have the airs disease
							playerDiseases.Add (ManagePlayerCellData.playerData.diseases [aDiseaseCnt]);//Give the player the disease in the air
						}
					}
				}
			
				for (int iDiseaseCnt = 0; iDiseaseCnt < playerItem.contractedDiseases.Count; iDiseaseCnt++) {//Go through every disease this item has
					bool foundItemDiseaseOnPlayer = false;//Checker if the player has the disease type of the item has
					for (int pDiseaseCnt = 0; pDiseaseCnt < playerDiseases.Count; pDiseaseCnt++) {//Go through every disease the player has
						if (playerDiseases [pDiseaseCnt].GetType () == playerItem.contractedDiseases [iDiseaseCnt].GetType ()) {//If the player and the item have the same type of disease
							foundItemDiseaseOnPlayer = true;//If the player has the same disease as the item
							break;//Do nothing
						}
					
						if (pDiseaseCnt == playerDiseases.Count && !foundItemDiseaseOnPlayer) {//If all the players diseases were compared and the player doesn't have the items disease
							playerDiseases.Add (playerItem.contractedDiseases [iDiseaseCnt]);//Give the player the disease on the item
						}
					}
				}
				
				
				currentOwner.playerAttributes.ourDiseases = playerDiseases;//Sync the contracted diseases with the player
				foreach(BaseDisease contractedDisease in playerAttributes.ourDiseases)
				{
					if(contractedDisease.diseaseIsAlive)
					{
						foreach(Symptom diseaseSymptom in contractedDisease.symptoms)
						{
							diseaseSymptom.OnEffectPlayer(this);
						}
					}
				}
			}
		}
	}
	#endregion
}