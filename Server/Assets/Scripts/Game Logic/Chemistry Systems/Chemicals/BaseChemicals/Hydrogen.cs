using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hydrogen : Chemical
{

	// Use this for initialization
	void Start ()
	{

	}

	public Hydrogen()
	{
		chemicalName = "Hydrogen";
	}

	public Hydrogen(float count)
	{
		chemicalName = "Hydrogen";
		quantity = count;
	}

//	public override void OnMix (List<Chemical> otherChemicalMix)
//	{
//		otherChemicalMix.Add(this);
//		bool[] chemicalReaction = new bool[otherChemicalMix.Count];
//		for(int i = 0; i < chemicalReaction.Length; i++)
//		{
//			chemicalReaction[i] = false;
//		}
//		ChemistryRecipes.Water waterMix = new ChemistryRecipes.Water();
//		int chemicalsRequired;
//		int chemicalsFound;
//
//		foreach(Chemical chem in waterMix)
//		{
//			chemicalsRequired++;
//			for(int i = 0; i < otherChemicalMix.Count; i++)
//			{
//				if(otherChemicalMix[i].GetType() == chem.GetType())
//				{
//					chemicalReaction[i] = true;
//					chemicalsFound++;
//					break;
//				}
//			}
//		}
//
//		if(chemicalsRequired == chemicalsFound)
//		{
//			for(int i = 0; i < otherChemicalMix.Count; i++)
//			{
//				if(chemicalReaction[i])
//				{
//					otherChemicalMix.RemoveAt(i);
//				}
//			}
//			otherChemicalMix.Add(Water);
//		}
//	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

