using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerConsole
{
	public bool[] subSystemsOnline =
	{
		false,
		false,
		false
	};
	public string areaLocation;
	public float powerUsage
	{
		get
		{
			float deviceUsages = 0.0f;
			foreach(HeavyElectronic device in electronicsOnGrid)
			{
				deviceUsages += device.powerRequirement;
			}
			return deviceUsages;
		}
		set{}
	}
	public List<HeavyElectronic> electronicsOnGrid = new List<HeavyElectronic>();
	
	public enum SubSystems 
	{
		Lighting,
		Atmospherics,
		Electronics
	}
	public PowerConsole ()
	{
		
	}
}


