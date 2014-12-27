using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QMRequisitionForms : HandItems
{
	public List<string> itemsToRequest;
	private bool stampApproved = false;
	private bool stampDenied = false;
	private bool computerApproved = false;
	
	public bool ComputerApproved 
	{
		get
		{
			return computerApproved;
		}
		set
		{
			if(!computerApproved)
			{
				computerApproved = true;
				ComputerApproved = value;
			}
			
		}
	}
	
	public bool StampApproved 
	{
		get 
		{
			return stampApproved;
		}
		set
		{
			if(stampDenied)
			{
				stampDenied = false;
				StampApproved = value;
			}
			else if (!stampDenied)
			{
				StampApproved = value;
			}
		}
	}
	
	public bool StampDenied
	{
		get 
		{
			return stampDenied;
		}
		set
		{
			if(stampApproved)
			{
				stampApproved = false;
				stampDenied = value;
			}
			else if (!stampDenied)
			{
				StampDenied = value;
			}
		}
	}
	
	public QMRequisitionForms(IDBadge insertedBadge)
	{
		playerWhoRequested = insertedBadge.playerName;
		requestersSection = insertedBadge.playerJob;
	}
	
	public string playerWhoRequested;
	public string requestersSection;

    protected QMRequisitionForms()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerUsingThis (PlayerCharacter pc)
	{
		//base.OnUsing (pc);
		
	}
	
	private void Start()
	{
		QMData.formsToProcess.Add(this);
	}
	
}

