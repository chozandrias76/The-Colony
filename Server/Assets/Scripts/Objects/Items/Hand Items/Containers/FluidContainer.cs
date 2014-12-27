using UnityEngine;
using System.Collections;

public class FluidContainer : HandItems
{
	public float maxVolume;

	public float currentVolume {
		get {
//			float chemicalQty;
//			foreach(Chemical chemical in containedChemical.chemicalMixture)
//			{
//				chemicalQty += chemical.quantity;
//			}
//			return chemicalQty;
			return containedChemical.quantity;
		}
		set {
//			float chemicalRemovalRatio = value/currentVolume;
//			foreach(Chemical chemical in containedChemical.chemicalMixture)
//			{
//				chemical.quantity = chemical.quantity - (chemical.quantity * chemicalRemovalRatio);
//			}
			if (value <= maxVolume) {
				//containedChemical.quantity = value;
			} else {
				//containedChemical.quantity = maxVolume;
			}
		}
	}

	public Chemical containedChemical;
	
	public FluidContainer ()
	{
		containedChemical = new Chemical (this);
	}
	
	public override void OnPlayerUsingThis (PlayerCharacter pc)
	{
		RaycastHit usedOn = new RaycastHit ();
		if (Physics.Raycast (pc.transform.position, pc.transform.forward, out usedOn, ~0)) {
			if(usedOn.collider.GetComponent<FluidContainer>() != null)
			{
				usedOn.collider.GetComponent<FluidContainer>().OnOtherUsingThis(this);
			}
			else if(usedOn.collider.GetComponent<ChemistryManipulator>() != null)
			{
				usedOn.collider.GetComponent<ChemistryManipulator>().AddContainer(this);
				GameObject.Destroy(this);
			}
		}
	}

	public override void OnOtherUsingThis (Item otherItem)//Use other item on this
	{
		FluidContainer otherContainer;
		if (otherItem.GetType () == typeof(FluidContainer)) {//Is another container using this
			otherContainer = otherItem as FluidContainer;
			containedChemical = ChemistryRecipes.OnMix (containedChemical, otherContainer.containedChemical, this, otherContainer);
		}
	}

	public bool TryGetOtherContainer (PlayerCharacter pc, out FluidContainer otherContainer)
	{
		var rayHit = new RaycastHit ();
		if (Physics.Raycast (pc.transform.position, pc.transform.forward, out rayHit, ~0)) {
			try {
				otherContainer = rayHit.collider.GetComponent<FluidContainer> ();
				return true;
			} catch {
				otherContainer = new FluidContainer ();
				return false;
			}
		} else {
			otherContainer = new FluidContainer ();
			return false;
		}
	}

//	public bool TryGetChemistryManipulator(PlayerCharacter pc, out ChemistryManipulator theManipulator)
//	{
//		var rayHit = new RaycastHit();
//        if (Physics.Raycast(pc.transform.position, pc.transform.forward, out rayHit, ~0))
//        {
//			theManipulator = rayHit.collider.GetComponent<ChemistryManipulator>();
//			if(rayHit.collider.GetComponent<ChemistryManipulator>() != null)
//			{
//				return true;
//			}
//			else
//			{
//				return false;
//			}
//		}
//		else
//		{
//			return false;
//		}
//	}

	public void RemoveChemical (Chemical chemicalToRemove)
	{
		foreach (Chemical chemical in this.containedChemical.chemicalMixture) {
			if (chemicalToRemove.GetType () == chemical.GetType ()) {
				this.containedChemical.chemicalMixture.Remove (chemicalToRemove);
				break;
			}
		}
	}

	public void AddChemical (Chemical chemicalToAdd)
	{
		if (chemicalToAdd.quantity + currentVolume <= maxVolume) {
			containedChemical = ChemistryRecipes.OnMix (chemicalToAdd, containedChemical, this, new FluidContainer ());
		} else {
			chemicalToAdd.quantity = maxVolume - currentVolume;
			containedChemical = ChemistryRecipes.OnMix (chemicalToAdd, containedChemical, this, new FluidContainer ());
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

