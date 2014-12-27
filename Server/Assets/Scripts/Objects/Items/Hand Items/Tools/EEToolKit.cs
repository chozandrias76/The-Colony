using System;
using UnityEngine;

public class EEToolKit : ElectronicDevice
{
	public HeavyElectronic[] toolKitBuffer = new HeavyElectronic[2];
	private int _indexer = 0;

	public int indexer
	{
		get
		{
			indexer++;
			return _indexer;
			
		}
		set
		{
			if(value > 1)
			{
				_indexer = 0;
			}
			else
			{
				_indexer = 1;
			}
		}
	}
	
	public enum UseMode
	{
		Scan,
		Link,
		Detatch,
		Diagnose,
	}
	
	private int modeSelected = 0;
	
	public EEToolKit ()
	{
	}
	
	private void Main ()
	{
		deviceScreen.WriteExternalString("Please choose from the following menu");
		deviceScreen.WriteExternalString("0)Add target to buffer" +
			"1)Remove target from buffer" +
			"2)Attach devices in buffer/n" +
			"3)Detach devices in buffer/n" +
			"4)Get the status of device(s) in buffer/n");
		bool inMainMenu = true;
		while(inMainMenu)
		{
			if(Input.GetKeyDown(KeyCode.Alpha0))
			{
				RaycastHit usedOn = new RaycastHit();
				if(Physics.Raycast(pc.transform.position, pc.transform.forward, out usedOn, ~0))
				{
			
					bool inBufferAlready = false;
					if(toolKitBuffer[0] == usedOn.collider.GetComponent<HeavyElectronic>())
					{
						inBufferAlready = true;
					}
					else if(toolKitBuffer[1] == usedOn.collider.GetComponent<HeavyElectronic>())
					{
						inBufferAlready = true;
					}
					if(!inBufferAlready)
					{
						toolKitBuffer[indexer] = usedOn.collider.GetComponent<HeavyElectronic>();
					}
				}
				
			}
			else if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				RaycastHit usedOn = new RaycastHit();
				if(Physics.Raycast(pc.transform.position, pc.transform.forward, out usedOn, ~0))
				{
					if(toolKitBuffer[0] == usedOn.collider.GetComponent<HeavyElectronic>())
					{
						toolKitBuffer[0] = null;
						indexer++;
					}
					else if(toolKitBuffer[1] == usedOn.collider.GetComponent<HeavyElectronic>())
					{
						toolKitBuffer[1] = null;
						indexer++;
					}
				}
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				toolKitBuffer[0].AttachDevice(toolKitBuffer[1]);
				toolKitBuffer[1].AttachDevice(toolKitBuffer[0]);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha3))
			{
			}
			else if(Input.GetKeyDown(KeyCode.Alpha4))
			{
			}
		}
	}
	
	public override void OnPlayerUsingThis (PlayerCharacter pc)
	{
		
	}
	
	public override void OnPlayerUsingThis ()
	{
		//TODO: Add Screen/GUI to hand objects
		//Maybe have Heavy Electronics inherit from new class "Electronic Devices"?
	}
	
	public void InsertIntoBuffer (Item insertItem)
	{
		toolKitBuffer[indexer] = insertItem;
	}
}


