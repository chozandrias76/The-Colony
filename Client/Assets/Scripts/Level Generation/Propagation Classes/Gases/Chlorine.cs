using UnityEngine;
using System.Collections;

public class Chlorine : MonoBehaviour {

	public float transferRate = 1.0f;
 
        public DamageInfo Damage = new DamageInfo()
        {
                bruteDamage  = 1.0f,
                oxyDamage    = 0.0f,
                toxiDamage   = 0.0f,
                heatDamage   = 0.0f,
                cloneDamage  = 0.0f,
                hallucDamage = 0.0f
        };
 
        public InteractionInfo Interaction = new InteractionInfo()
        {
                lungs     = 0.0f,
                hands     = 1.0f,
                feet      = 1.0f,
                face      = 1.0f,
                ingestion = 1.0f,
                injection = 1.0f
        };
}
