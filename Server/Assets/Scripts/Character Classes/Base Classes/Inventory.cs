#region License

// // Inventory.cs
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
using The_Colony_Proj;

public class Inventory
{
	public IDBadge thisBadge;
	public HeadItems thisHeadItem;
	public TorsoItems thisTorsoItem;
	public LegItems thisLegItem;
	public FeetItems thisFeetItem;
	public HandItems[] thisHandItems = new HandItems[2];
	public WristItems thisWristItem;
	public BeltItems thisBeltItem;
	public HandItems[] thisPocketItems = new HandItems[2];
	public ExTorsoItems thisExTorsoItem;
	
	public IEnumerable<Item> InventoryItems ()
	{
		yield return (Item)this.thisBadge;
		yield return (Item)this.thisHeadItem;
		yield return (Item)this.thisTorsoItem;
		yield return (Item)this.thisLegItem;
		yield return (Item)this.thisFeetItem;
		yield return (Item)this.thisHandItems[0];
		yield return (Item)this.thisHandItems[1];
		yield return (Item)this.thisWristItem;
		yield return (Item)this.thisBeltItem;
		yield return (Item)this.thisPocketItems[0];
		yield return (Item)this.thisPocketItems[1];
		yield return (Item)this.thisExTorsoItem;
	}

	//public Dictionary<string, Item> inventoryItems = new Dictionary<string, Item>();

	private void Start ()
	{
	}

	private void Update ()
	{
	}
}