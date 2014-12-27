using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChemistryRecipes
{

	public static Chemical[] chemicals =
	{
		new WaterChemical(),
	};

	public static Chemical OnMix(Chemical chemicalTo, Chemical chemicalFrom, FluidContainer containerTo, FluidContainer containerFrom)
	{
		float transferableQty;
		float transferableScale;

		transferableQty = containerTo.maxVolume - containerTo.currentVolume;
		transferableScale = transferableQty/containerTo.maxVolume;

		Chemical intermediateChem = chemicalFrom;
		intermediateChem.quantity = intermediateChem.quantity * transferableScale;
		chemicalTo.chemicalMixture.Add(intermediateChem);
		chemicalFrom.quantity = chemicalFrom.quantity - intermediateChem.quantity;
		
		if(transferableQty > 0.0f)
		{
			chemicalTo = MixNoReaction(chemicalTo);
			chemicalTo = CheckForReaction(chemicalTo);
		}
		return chemicalTo;
	}

	public static Chemical MixNoReaction(Chemical chemicalMix)
	{
		for(int i = 0; i < chemicalMix.chemicalMixture.Count; i++)
		{
			for(int j = 0; j < chemicalMix.chemicalMixture.Count; j++)
			{
				if(chemicalMix.chemicalMixture[i].GetType() == chemicalMix.chemicalMixture[j].GetType())
				{
					if(i != j)
					{
						chemicalMix.chemicalMixture[i].quantity += chemicalMix.chemicalMixture[j].quantity;
						chemicalMix.chemicalMixture.RemoveAt(j);
						chemicalMix = MixNoReaction(chemicalMix);
					}
				}
			}
		}
		return chemicalMix;
	}

	public static Chemical CheckForReaction(Chemical chemicalMix)
	{
		Chemical newChemical = new Chemical();

		for(int index1 = 0; index1 < chemicals.Length; index1++)//Go through each known compound
		{
			int baseChemsFound = 0;
			int baseChemsRequired;
			for(int index2 = 0; index2 < chemicals[index1].baseChemicals.Count; index2++)//Go through compounds contents
			{
				baseChemsRequired = chemicals[index1].baseChemicals.Count;
				for(int index3 = 0; index3 < chemicalMix.chemicalMixture.Count; index3++)//Go through mixtures contents
				{
					if(chemicalMix.chemicalMixture[index3].GetType() == chemicals[index1].baseChemicals[index2].GetType())
					{
						baseChemsFound++;
						if(baseChemsFound == baseChemsRequired)
						{
							if(CreateNewChemical(chemicals[index1], chemicalMix, out newChemical))//If a chemical reaction happens then check for next reactions
							{
								CheckForReaction(newChemical);
							}
							else if(!CreateNewChemical(chemicals[index1], chemicalMix, out newChemical))//If reaction doesn't happen, try with next compound
							{
								goto nextchemical;
							}
						}
					}
				}
			}
		nextchemical:;
		}
		return newChemical;
	}

	static bool CreateNewChemical(Chemical template, Chemical mixture, out Chemical resultant)
	{
		var toProcess = new Chemical();
		var completedChem = new Chemical();
		foreach(Chemical chem in mixture.chemicalMixture)//Find all the chemicals in mixture
		{
			foreach(Chemical chemTemplate in template.baseChemicals)//Find all chemicals required for reaction
			{
				if(chem.GetType() == chemTemplate.GetType())//If the chemical matches the one for reaction
				{
					toProcess.chemicalMixture.Add(chem);//Add that chemical to a processing mixture
					mixture.chemicalMixture.Remove(chem);//Remove it from input mixture
				}
			}
		}

		int[] quantitys = new int[toProcess.chemicalMixture.Count];
		for(int i = 0; i < toProcess.chemicalMixture.Count; i++)//Make an account of all the quantities of the chemicals
		{
			quantitys[i] = Mathf.FloorToInt(toProcess.chemicalMixture[i].quantity);
		}

		int maximumQuantity = 0;
		for(int i = 0; i < quantitys.Length; i++)//Process all the quatities of process chemical
		{
			if(quantitys[i] / template.chemicalRatioRequisites[i] < 1)//If there is not enough for the reaction, quit out
			{
				foreach(Chemical toProcessChemical in toProcess.chemicalMixture)
				{
					completedChem.chemicalMixture.Add(toProcessChemical);
				}
				foreach(Chemical leftoverMixtureChemical in mixture.chemicalMixture)
				{
					completedChem.chemicalMixture.Add(leftoverMixtureChemical);
				}
				resultant = completedChem;
				return false;
			}
			else//Otherwise find smallest denominator
			{
				maximumQuantity = (maximumQuantity < quantitys[i] / template.chemicalRatioRequisites[i]) ? maximumQuantity : quantitys[i] / template.chemicalRatioRequisites[i];
			}
		}

		if(maximumQuantity > 0)//Create reaction only if enough quantity
		{
			Chemical reactionChemical = template;//Reaction created chemical
			reactionChemical.quantity = maximumQuantity;//Multiplied Reaction created chemical based on reagent ratios
			for(int i = 0; i < toProcess.chemicalMixture.Count; i++)
			{
				toProcess.chemicalMixture[i].quantity = toProcess.chemicalMixture[i].quantity - template.chemicalRatioRequisites[i]*maximumQuantity;//Reduce left over reactant chemicals
			}
			foreach(Chemical remainingChemicals in toProcess.chemicalMixture)
			{
				completedChem.chemicalMixture.Add(remainingChemicals);//Add remaining chemicals after reaction to output
			}
			foreach(Chemical unReactedChemical in mixture.chemicalMixture)
			{
				completedChem.chemicalMixture.Add(unReactedChemical);//Add unused chemicals back to completed chemical
			}
			completedChem.chemicalMixture.Add(reactionChemical);//Add total amount of reation created chemical
			resultant = completedChem;
			return true;
		}
		else
		{
			resultant = completedChem;
			return false;
		}

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

