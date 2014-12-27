using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyElectronicTracker : MonoBehaviour
{
	public HeavyElectronicTracker Singleton;
	
	public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
	
	public List<GameObject> trackedElectronics = new List<GameObject>();
	
	public void Start()
	{
		GameObject[] allGOs = (GameObject[]) GameObject.FindObjectsOfType(typeof (GameObject));
		foreach(GameObject go in allGOs)
		{
			try
			{
				
				trackedElectronics.Add(go);
				
			}
			catch
			{
				//new NullReferenceException ("Heavy Electronic not Attached");
			}
		}
		foreach(GameObject go in trackedElectronics)
		{
			ObjectDataTracker.Singleton.GetPower(go.transform.position).electronicsAttached.Add(go.GetComponent<HeavyElectronic>());
		}
	}
	
	public void AddHeavyElectronic()
	{
		
	}
	
	public void RemoveHeavyElectronic()
	{
		
	}
	
	public void RemoveEnergyFromGrid()
	{
	}
	
}


