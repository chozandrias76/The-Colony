using UnityEngine;
using System.Collections;

public class C02 : MonoBehaviour {

	public float transferRate = 1.0f;
 
        public DamageInfo Damage = new DamageInfo()
        {
                bruteDamage  = 0.0f,
                oxyDamage    = 0.3f,
                toxiDamage   = 0.0f,
                heatDamage   = 0.0f,
                cloneDamage  = 0.0f,
                hallucDamage = 0.0f
        };
 
        public InteractionInfo Interaction = new InteractionInfo()
        {
                lungs     = 1.0f,
                hands     = 0.0f,
                feet      = 0.0f,
                face      = 0.0f,
                ingestion = 0.0f,
                injection = 1.0f
        };
}
