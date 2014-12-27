#region License

// // Item.cs
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
using UnityEngine;
using NetworkViewID = uLink.NetworkViewID;

public class Item : MonoBehaviour
{
    public enum BodyLocations
    {
        ID,
        Head,
        Torso,
        Legs,
        Feet,
        R_Hand,
        L_Hand,
        Wrist,
        Belt,
        R_Pocket,
        L_Pocket,
        Ex_Torso
    }

    public enum ObjectSizes
    {
        tiny = 0,
        small,
        normal,
        bulky,
        huge
    }

    public enum UsableDistances
    {
        melee,
        near,
        far,
        unlimited
    }
	public PlayerCharacter currentOwner;
    public List<PlayerCharacter> touchedByPlayers = new List<PlayerCharacter>();
    public List<BaseDisease> contractedDiseases = new List<BaseDisease>();
	public bool canPickUp = true;
    //private List<string> touchedByDNA = new List<string>();

    public bool[] equippableLocationValues =
    {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
    };

    public int itemID;
    public Mesh itemMesh;
    public int itemSize;
    public Texture2D itemTexture;
    public int itemUseDistance;

    public string[] usableOn =
    {
    };

    public override string ToString()
    {
        return string.Format("");
    }

    public virtual void OnPlayerUsingThis(PlayerCharacter pc)
    {
		
    }
	
//	public virtual void OnPlayerUsingThis(PlayerCharacter pc, RaycastHit usedOn)
//	{
//		
//	}

    public virtual void OnPlayerUsingThis()
    {
    }
	
	public virtual void OnOtherUsingThis(Item usedWith)
	{
	}

    public virtual void OnEquip(string bodyLocation)
    {
    }

    public virtual void OnUnEquip(string bodyLocation)
    {
    }

    public virtual void OnPickedUp(PlayerCharacter pc)
    {
		pc.playerInventory.thisHandItems[pc.HandSelected] = (this.MemberwiseClone());
		pc.playerInventory.thisHandItems[pc.HandSelected].currentOwner = pc;
		pc.playerInventory.thisHandItems[pc.HandSelected].touchedByPlayers.Add(pc);
		Destroy(this);
    }
	
	public virtual void OnDropped()
	{
		GameObject item = new GameObject(this.name, this.GetType());
		currentOwner.playerInventory.thisHandItems[currentOwner.HandSelected] = null;
	}
	
	void Update()
	{
		
	}
}