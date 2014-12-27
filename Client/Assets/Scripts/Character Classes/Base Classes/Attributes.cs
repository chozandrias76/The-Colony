using UnityEngine;
using System.Collections;

public class Attributes : MonoBehaviour {

	// Use this for initialization
    public struct AttributeTypes 
    {
       public enum skinColor
       {
           Green, 
           Blue, 
           Red, 
           Rainbow
       }

       public enum gender
       {
           Male, 
           Female,
           Transgendered
       }

       public enum race
       {
           Human, 
           Surrogate, 
           Xenomorph, 
           Dog, 
           Cat, 
           Monkey
       }

       public enum mutation
       {
           None,
           Xray, 
           Telekenesis, 
           Hulk,
       }

       public enum diseases
       {
           None,
           Alopecia, 
           ABMetabolism,
           Choking, 
           Coughing, 
           Confusion, 
           Deafness, 
           Dizziness, 
           EternalYouth, 
           FacialHypertrichosis, 
           Fever, 
           Hallucinogen, 
           Headache, 
           Itching, 
           Longevity, 
           NecrotizingFasciitis, 
           SelfRespiration, 
           Shivering, 
           Sneezing, 
           Stimulant, 
           ToxicCompensation, 
           ToxinFilter, 
           VoiceChange, 
           Vomiting, 
           VomitingBlood, 
           WeightGain, 
           WeightLoss, 
           WeightEven
       }

    }
    
	void Start () 
    {
	}

    // Update is called once per frame
    void Update() 
    {
	
	}
}
