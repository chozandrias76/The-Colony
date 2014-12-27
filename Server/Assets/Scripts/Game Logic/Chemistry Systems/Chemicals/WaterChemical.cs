using UnityEngine;
using System.Collections;

public class WaterChemical : Chemical
{
	public WaterChemical()
	{
		chemicalName = "Water";
	}

	public WaterChemical(float count)
	{
		quantity = count;
		baseChemicals.Add(new Oxygen(1.0f));
		baseChemicals.Add(new Hydrogen(2.0f));
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

