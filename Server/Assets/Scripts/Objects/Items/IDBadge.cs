#region License

// // IDBadge.cs
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
using UnityEngine;

public class IDBadge : HandItems
{
	public IDBadge (string _name, string _job, string _gender)
	{
		playerName = _name;
		playerJob = _job;
		gender = _gender;
		//badgeClearance = SecurityStatus.GetJobClearance(_job);		
	}

	public string playerName { get; set; }

	public string playerJob { get; set; }

	public int[] badgeClearance;

	public string gender { get; set; }
	
	public int[] GetBadgeClearance ()
	{
		return badgeClearance;
	}
	
	public override string ToString ()
	{
		return string.Format ("IDBadge");
	}
	
	public IDBadge()
	{
		equippableLocationValues [0] = true; //Valid in ID slot
		usableOn [0] = "Terminal";
		itemSize = (int)ObjectSizes.tiny;
		itemUseDistance = (int)UsableDistances.near;
	}

	private void Awake ()
	{
	}

	private void Start ()
	{
		
	}

	private void Main ()
	{
	}

	private void Update ()
	{
	}
}