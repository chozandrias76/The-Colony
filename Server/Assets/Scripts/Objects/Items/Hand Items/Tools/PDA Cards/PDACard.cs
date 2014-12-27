using UnityEngine;
using System.Collections;
using System;

public class PDACard : HandItems
{
	public string cardString = "1)Messenger /n 2)Note Keeper /n3)Atmospheric Scanner /n4)Flashlight/n";
	public int menuCount = 4;
	public PDA attachedPDA;
	public string notes;
	public bool readyToUse = false;
	public bool waitingToUse = false;
	public bool wasUsed = false;
	public bool returnToMenu;
	public PDACard ()
	{

	}
	
    public virtual string MenuTextOnInput(string inputSentFromPDA)
    {
		return new string(' ',0);
    }
	
    //public string MessengerMenu()
    //{
    //}
	
    //public string NoteMenu()
    //{
    //    attachedPDA.recievedString = notes;
    //}
	
    //public string ScanAtmosphere()
    //{
    //    CellData cellDataAtLocation = new CellData();
    //    //RPC call
    //}
	
    //public void ToggleFlashLight()
    //{
		
    //}
	
    //public string ScanGas()
    //{
    //    waitingToUse = true;
    //    while(waitingToUse)
    //    {
    //        if(wasUsed)
    //        {
    //            wasUsed = false;
    //            waitingToUse = false;
				
    //        }
    //    }
    //}
	
    //public string GetManifest()
    //{
    //}
	
    //public string SetDisplay()
    //{
    //}
	
    //public string GetPowerStatus(bool allRecords)
    //{
    //}
	
    //public string GetMedicalRecords()
    //{
    //}
	
    //public string ScanPlayerVitals()
    //{
    //}
	
    //public string GetSecurityRecords()
    //{
    //}
	
    //public string ScanChemicals()
    //{
    //}
	
    //public string ScanPlayerRad()
    //{
    //}
	
    //public string SendVirus(string cardType)
    //{
    //}
	
    //public void HonkSynth()
    //{
    //}
	
    //public string GetSupplyRecords()
    //{
    //}
	
    //public string AccessMULE()
    //{
    //}
	
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

