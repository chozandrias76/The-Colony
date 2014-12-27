using System;
using UnityEngine;
using System.Collections;

public class AtmosphericDataTracker
{
	public static AtmosphericDataTracker Singleton;
	    public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
}


