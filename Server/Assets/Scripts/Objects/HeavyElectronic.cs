#region License

// // HeavyElectronic.cs
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
using uLink;

public class HeavyElectronic : Item
{
	public float powerRequirement;
	public float incommingPower;
	public bool powerRequirementMet = false;
	public string computerText;
	public ConsoleScreen deviceScreen = new ConsoleScreen();
	
	public BaseTerminal attachedTerminal;
	protected List<HeavyElectronic> attachedDevices = new List<HeavyElectronic>();
	protected List<HeavyElectronic> possibleDevices = new List<HeavyElectronic>();

	//public bool inUse;
	public bool online 
	{
		get
		{
			return online;
		}
		set
		{
			if(!powerRequirementMet)
			{
				online = false;
			}
			else
			{
				online = value;
			}
		}
	}
	
    public override void OnPlayerUsingThis(PlayerCharacter pc)
    {
        
    }
	
	public override void OnOtherUsingThis(Item usedWith)
	{
	}
		

	public virtual void Main()
	{
	}

    public bool CanPickUp
    {
        get { return false; }
    }

    public int ItemSize
    {
        get { return (int) ObjectSizes.huge; }
    }

    // Use this for initialization
    private void Start()
    {
    }
	
	public void AttachDevice(HeavyElectronic deviceToAttach)
	{
		bool validDevice = false;
		foreach(HeavyElectronic device in possibleDevices)
		{
			if(device.GetType() == deviceToAttach.GetType())
			{
				validDevice = true;
				break;
			}
		}
		if(validDevice)
		{
			bool deviceAlreadyAttached = false;
			foreach(HeavyElectronic device in attachedDevices)
			{
				if(device == deviceToAttach)
				{
					deviceAlreadyAttached = true;
				}
			}
			if(!deviceAlreadyAttached)
			{
				attachedDevices.Add(deviceToAttach);
			}
		}
		
	}

    // Update is called once per frame
    private void Update()
    {
		if(incommingPower >= powerRequirement)
		{
			powerRequirementMet = true;
		}
		else
		{
			powerRequirementMet = false;
			if(online)
			{
				online = false;
			}
		}
    }
}