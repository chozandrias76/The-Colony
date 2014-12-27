using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chemical 
{
	public string chemicalName;
	public float singularQuantity;

	public float quantity
	{
		get{
		float mixtureQuantity = 0;
			if(chemicalMixture.Count > 1)
			{
				foreach(Chemical chem in chemicalMixture)
				{
					mixtureQuantity += chem.singularQuantity;
				}
				return mixtureQuantity;
			}
			else
			{
				return singularQuantity;
			}

		}
		set
		{
			float removalQuantityRatio =  value/quantity;
			if(chemicalMixture.Count > 1)
			{
				foreach(Chemical chem in chemicalMixture)
				{
					chem.singularQuantity = chem.singularQuantity - (chem.singularQuantity * removalQuantityRatio);
				}
			}
			else
			{
				//Chemical thisChemical = chemicalMixture.Find(this as Chemical);
				singularQuantity = value;
				//thisChemical.singularQuantity = value;
			}
		}
	}
	public FluidContainer container;

	public List<Chemical> chemicalMixture = new List<Chemical>();
	public List<Chemical> baseChemicals = new List<Chemical>();

	public List<int> chemicalRatioRequisites = new List<int>();

	public Chemical (FluidContainer containedin)
	{
		container = containedin;
	}
	public Chemical ()
	{
	}

	// Use this for initialization
	void Start ()
	{
		chemicalMixture.Add(this);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

