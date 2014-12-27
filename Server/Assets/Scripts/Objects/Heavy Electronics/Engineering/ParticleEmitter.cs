using System;
using UnityEngine;

public class ParticleEmitter : HeavyElectronic
{
	public ParticleEmitter ()
	{
		powerRequirement = 1000.0f;
	}
	
	int tick = 0;
	//GameObject bullet;
	//float emittedCharge;
	void Start()
	{
		//emittedCharge = powerRequirement * 0.75f;
	}
	void Update()
	{
		tick++;
		if(tick%(60*10) == 0 && online)
		{
			//bullet = FireEmitter();
		}
//		if(bullet != null)
//		{
//			//bullet.transform.position += bullet.transform.forward * 10;
//			//var raycastHit = new RaycastHit();
////			if(Physics.Raycast(transform.position, transform.forward, out rayHit, ~0))
////			{
////				if(raycastHit.collider.gameObject.name.Contains("Reactor Core"))
////				{
////					raycastHit.collider.GetComponent<ReactorCore>().ReactorCharge += emittedCharge;
////					incommingPower -= emittedCharge;
////				}
////			}
//		}
	}
	
//	GameObject FireEmitter()
//	{
//		//Instanciate bullet
//		//GameObject bullet = new GameObject("bullet", MeshCollider);
//		//return bullet;
//	}

	
}


