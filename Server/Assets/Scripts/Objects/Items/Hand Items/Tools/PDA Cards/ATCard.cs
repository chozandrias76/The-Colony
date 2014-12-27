using UnityEngine;
using System.Collections;

public class ATCard : PDACard
	{
		public ATCard()
		{
		menuCount = 5;
		cardString += "5)Gas Scanner/n";
		}
		//string inputSentFromPDA;
		
	public override string MenuTextOnInput (string inputSentFromPDA)
	{
        //switch(inputSentFromPDA)
        //{
        //case '1':
        //    if(menuCount > 0)
        //        return base.MessengerMenu();
        //    break;
        //case '2':
        //    if(menuCount > 1)
        //        return base.NoteMenu();
        //    break;
        //case '3':
        //    if(menuCount > 2)
        //        return base.ScanAtmosphere();
        //    break;
        //case '4':
        //    if(menuCount > 3)
        //        base.ToggleFlashLight();
        //        return cardString;
        //    break;
        //case '5':
        //    if(menuCount > 4)
        //        return base.ScanGas();
        //    break;
        //default:
        //    break;
			
        //}
        return new string(' ',0);
	}
		
		//BreatheDeep Cartridge
		//Gas Scanner: Allows you to smack your PDA into gas-containers (Such as canisters) and display the contained gas's temperature, pressure, and composition.
	}

