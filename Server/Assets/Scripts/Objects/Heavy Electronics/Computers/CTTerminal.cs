using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CTTerminal : BaseTerminal
{
	//public IDBadge playerBadgeInserted;
	public CTTerminal()
	{
		requiredClearance = (int)SecurityStatus.JobSecurityClearance.CargoSpace;
		tertiaryClearance = (int)SecurityStatus.GeneralSecurityClearance.Tir2;
		terminalName = "Cargo";
		baseMenu = "0)Order items /n" +
			"1)View requests /n" +
			"2)View orders /n" +
			"3)Supply shuttle menu/n";
		baseMenuChoices = 4;
	}
//	public override void OnUsing (PlayerCharacter pc)
//	{
//		base.OnPlayerUsing (pc);
//	}
	
	void Start ()
	{
		
	}
	
	protected override void BaseMenuChoice0 ()
	{
		OrderItems ();
	}
	
	protected override void BaseMenuChoice1 ()
	{
		ViewRequests ();
	}
	
	protected override void BaseMenuChoice2 ()
	{
		ViewOrders ();
	}
	
	protected override void BaseMenuChoice3 ()
	{
		ShuttleMenu ();
	}
	
	protected override void BaseMenuChoice4 ()
	{
		base.BaseMenuChoice4 ();
	}
	
	public override void Main ()
	{
		base.Main();
	}
	
	void Update ()
	{
		
	}
	
	void ShuttleMenu ()
	{
		bool inShuttleMenu = true;
		computerText = "Pick up orders/n" +
				"Send Shuttle to Station/n" +
				"Get status of the Shuttle";
		computerText += "Type 'Quit' to return to the main menu/n";
		deviceScreen.WriteExternalString(computerText);
		string inputString = "";
		while (inShuttleMenu)
		{

			if (Input.inputString == " ")
			{//Enter
				if (inputString == "quit" || inputString == "Quit")
				{
					inShuttleMenu = false;
					break;
				}
				else
				{
					if (inputString == "Pick up orders")
					{
						if (QMData.shuttleCalledTime == 0.0f && !QMData.shuttleCalled)
						{
							QMData.shuttleCalledTime = QMData.currentTime + 3000.0f;
							computerText = "Shuttle is headed to the Hub/n";
							deviceScreen.WriteExternalString(computerText);
						}
						deviceScreen.WriteExternalString("/nPress any key to go back");
						while (!Input.anyKeyDown)
						{
								
						}
						inShuttleMenu = false;
						break;
					}
					else if (inputString == "Send Shuttle to Station")
					{
						if (QMData.shuttleCalledTime != 0.0f && !QMData.shuttleCalled)
						{
							foreach (QMData.Quote quote in QMData.quotes)
							{
								quote.statusIndex = 2;
								
							}
							QMData.shuttleCalled = true;
							computerText = "Shuttle is on its way back to the Station/n";
							deviceScreen.WriteExternalString(computerText);
							deviceScreen.WriteExternalString("/nPress any key to go back");
						while (!Input.anyKeyDown)
						{
								
						}
						}
						inShuttleMenu = false;
						break;
					}
					else if (inputString == "Get status of the Shuttle")
					{
						GetShuttleStatus();
						inShuttleMenu = false;
						break;
					}
					else
					{
						deviceScreen.ClearScreen();
						ShuttleMenu();
					}
				}
			}
			else
			{
				if(Input.inputString != "")
				{
					inputString += Input.inputString;
					deviceScreen.WriteExternalString(Input.inputString);
				}
			}

		}
		Main();
	}
	
	void GetShuttleStatus ()
	{
		computerText = "";
		if (QMData.currentStatusIndex == 0)
		{
			deviceScreen.ClearScreen ();
			computerText = String.Format ("The shuttle has already departed. There is {0}ms until arrival",
                    QMData.TimeLeft);
			deviceScreen.WriteExternalString (computerText);
			deviceScreen.WriteExternalString("/nPress any key to go back");

			while(!Input.anyKeyDown)
			{

			}
			Main ();

		}
		else if (QMData.currentStatusIndex == 1)
		{
			deviceScreen.ClearScreen ();
			computerText = String.Format ("The shuttle is on it's way. It will arrive in {0}ms.",
                    QMData.TimeLeft);
			deviceScreen.WriteExternalString (computerText);
			deviceScreen.WriteExternalString("/nPress any key to go back");

			while(!Input.anyKeyDown)
			{

			}
			Main ();
		}
	}
	
	void ViewRequests ()
	{
	    var terminalBadge = playersInsertedBadge as IDBadge;
		computerText = "Type the playerName of any request you would like to authorize/n";
		foreach (QMRequisitionForms form in QMData.formsToProcess)
		{
			computerText += "Request playerName '" + form.ToString () + "' Requsted by " + form.playerWhoRequested + "From Section " + form.requestersSection + "/n";
		}
		computerText += "Type 'Quit' to return to the main menu/n";
		deviceScreen.WriteExternalString(computerText);
		bool viewRequestsMenuOpen = true;
		string inputString = "";
		while (viewRequestsMenuOpen)
		{
			if (Input.inputString == " ")
			{//Enter
				if (inputString == "quit" || inputString == "Quit")
				{
					viewRequestsMenuOpen = false;
					break;
				}
				else
				{
					foreach (QMRequisitionForms form in QMData.formsToProcess)
					{
						if (inputString == form.ToString ())
						{
						    form.ComputerApproved = true;
                            
							QMData.formsApproved.Add (form);
							QMData.formsToProcess.Remove (form);
                            QMData.Quote newQuote = new QMData.Quote(terminalBadge.playerName, terminalBadge.playerJob, form);
							if (QMData.currentStatusIndex == 2)//Shuttle is at hub
							{
								newQuote.statusIndex = 1;
							}
							else//Shuttle is anywhere else
							{
								newQuote.statusIndex = 0;
							}
                            newQuote.authorizedBy = terminalBadge.playerName;
							QMData.quotes.Add (newQuote);
							viewRequestsMenuOpen = false;
							break;
						}
						else
						{
							deviceScreen.ClearScreen();
							ViewRequests ();
						}
					}
				}
			}
			else
			{
				if(Input.inputString != "")
				{
					inputString += Input.inputString;
					deviceScreen.WriteExternalString(Input.inputString);
				}
			}

		}
		Main ();
	}
	
	void ViewOrders ()
	{
		foreach (QMData.Quote quote in QMData.quotes)
		{
			computerText += quote.ToString () + "/n";
		}
		computerText += "Press any key to return/n";
		deviceScreen.WriteExternalString(computerText);
		while(!Input.anyKeyDown)
		{
		}
		Main();
	}
	
	void OrderItems ()
	{
		//computerText = SampleForm.ToString() + "/n";
		foreach (QMRequisitionForms form in QMData.possibleForms)
		{
			computerText += form.ToString () + "/n";
		}
		computerText += "Type 'Quit' to return to the main menu/n";
		deviceScreen.WriteExternalString(computerText);
		bool orderItemsMenuOpen = true;
		string inputString = "";
	    var terminalBadge = playersInsertedBadge as IDBadge;
		while (orderItemsMenuOpen)
		{
			if (Input.inputString == " ")
			{//Enter
				if (inputString == "quit" || inputString == "Quit")
				{
					orderItemsMenuOpen = false;
					break;
				}
				else
				{
					foreach (QMRequisitionForms form in QMData.possibleForms)
					{
				
						if (inputString == form.ToString ())
						{
							if (QMData.currentStatusIndex != 2)
							{
								computerText = "You can not place orders until the shuttle is at the hub";
							}
							else
							{
                                SampleForm thisForm = new SampleForm(terminalBadge);
                                QMData.Quote newQuote = new QMData.Quote(terminalBadge.playerName, terminalBadge.playerJob, thisForm);
								newQuote.statusIndex = 1;
								QMData.quotes.Add (newQuote);
								orderItemsMenuOpen = false;
							}
						}
						else
						{
							deviceScreen.ClearScreen();
							OrderItems ();
						}
					}
				}
				
			}
			else
			{
				if(Input.inputString != "")
				{
					inputString += Input.inputString;
					deviceScreen.WriteExternalString(Input.inputString);
				}
			}
		}
		Main ();
	}
	
}

