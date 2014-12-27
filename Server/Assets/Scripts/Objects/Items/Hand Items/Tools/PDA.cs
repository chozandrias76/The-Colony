using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PDA : HandItems
{
	
	public Item PDAcard;
	public PDACard insertedCard;
	public ConsoleScreen pdaScreen = new ConsoleScreen();
	public string pdaString;
	public string recievedString;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public override void OnPlayerUsingThis (PlayerCharacter pc)
	{
		if(insertedCard.waitingToUse)
		{
			insertedCard.wasUsed = true;
		}
		
	}
	public void OnInteract()
	{
		bool inOperation = true;
		pdaString = insertedCard.cardString;
		recievedString = pdaString;
		pdaScreen.WriteExternalString(pdaString);
		while(inOperation)
		{
			if(pdaString != recievedString)
			{
				pdaString = recievedString;
				pdaScreen.WriteExternalString(pdaString);
			}
			//if(Input.anyKeyDown)
				//pdaScreen.WriteExternalString(insertedCard.MenuTextOnInput(Input.inputString));
			//pdaString = Input.anyKey ? insertedCard.MenuTextOnInput(Input.inputString) : insertedCard.cardString;
		}
		//base.OnUsing (pc);
	}
	
}

