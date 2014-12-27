using System;
using UnityEngine;
using System.Collections;

public class PowerDataTracker
{
	static PowerDataTracker Singleton;
	public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
	//public PowerData[] powers = new PowerData[CellDataTracker.CellDataSize];
	
}


