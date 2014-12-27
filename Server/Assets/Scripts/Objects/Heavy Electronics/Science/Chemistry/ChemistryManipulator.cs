using UnityEngine;
using System.Collections;

public class ChemistryManipulator : HeavyElectronic
{
	public FluidContainer attachedContainer;
	public Chemical[] deployableChemicals =
	{
		new Copper(),
		new Hydrogen(),
		new Mercury(),
		new Nitrogen(),
		new Oxygen(),
		new Potassium(),
		new Sulfur()
	};
	int menuIndex = 0;
	public override void Main()
	{
		computerText = "Welcome to your menu for your Chemistry Manipulator./n" +
			"Type the name of the chemical exactly and press enter to add./n";
		for(menuIndex = 0; menuIndex < deployableChemicals.Length; menuIndex++)
		{
			computerText += System.String.Format("{0})Deploy {1}/n",menuIndex,deployableChemicals[menuIndex].ToString());
		}
		attachedTerminal.deviceScreen.WriteExternalString(computerText);
		bool usingThisDevice = true;
		string inputString = "";
		float addingAmmount = 10.0f;
		while(usingThisDevice)
		{
			foreach(char c in Input.inputString)
			{
				if(c != ' ')
				{
					inputString += c;
				}
				else
				{
					if(inputString == "Quit" || inputString == "quit")
					{
						usingThisDevice = false;
						attachedTerminal.deviceScreen.WriteExternalString("Quitting out of machine interface...");
						attachedTerminal.deviceScreen.ClearScreen();
						break;
					}
					foreach(Chemical chemical in deployableChemicals)
					{
						if(chemical.ToString() == inputString)
						{

							computerText = System.String.Format("Adding {1} {0} to container",addingAmmount, chemical.ToString());
							attachedTerminal.deviceScreen.ClearScreen();
							attachedTerminal.deviceScreen.WriteExternalString(computerText);
							AddToContainer(chemical,addingAmmount);
							Main();
						}
					}
				}
			}
		}
		attachedTerminal.Main();
	}

	public void AddToContainer(Chemical chemicalToAdd, float quantityToAdd)
	{
		//float avaliableQty = attachedContainer.maxVolume - attachedContainer.currentVolume;
		if(quantityToAdd + attachedContainer.currentVolume > attachedContainer.maxVolume)
		{
			quantityToAdd = attachedContainer.maxVolume - attachedContainer.currentVolume;
			chemicalToAdd.quantity = quantityToAdd;
			if(quantityToAdd > 0.0f)
			{
				attachedContainer.containedChemical.chemicalMixture.Add(chemicalToAdd);
				attachedContainer.containedChemical = ChemistryRecipes.MixNoReaction(attachedContainer.containedChemical);
				attachedContainer.containedChemical = ChemistryRecipes.CheckForReaction(attachedContainer.containedChemical);
			}
		}
		else
		{
			attachedContainer.containedChemical.chemicalMixture.Add(chemicalToAdd);
			attachedContainer.containedChemical = ChemistryRecipes.MixNoReaction(attachedContainer.containedChemical);
			attachedContainer.containedChemical = ChemistryRecipes.CheckForReaction(attachedContainer.containedChemical);
		}
	}

	public void AddContainer(FluidContainer playersContainer)
	{
		attachedContainer = playersContainer;
	}
	
	public override void OnOtherUsingThis(Item itemUsing)
	{
		if(itemUsing.GetType() == typeof(FluidContainer))
		{
			AddContainer(itemUsing as FluidContainer);
			itemUsing = null;
		}
	}

	public void RemoveContainer()
	{
		//attachedContainer = null;
	}

	public override string ToString ()
	{
		return string.Format ("Chemistry Manipulator");
	}

	public void RemoveContainer(FluidContainer computersContainer, PlayerCharacter pc)
	{

	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

