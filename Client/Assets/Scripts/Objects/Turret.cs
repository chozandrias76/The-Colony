using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : Unit {
	static	List<PlayerManager.Player>	scanResult=new List<PlayerManager.Player>();
	Transform 	ourTransform;
	Transform 	base1;
	Transform 	base2;
	Transform 	muzzle;
	int 		checkCounter;
	int			checkInterval=1;
	int			fireCounter=0;
	int 		fireInterval=30;
	float		muzzleFlash=0;
	// Use this for initialization
	public override void  unitStart () {
		ourTransform=transform;
		base1=ourTransform.GetChild(0);//GetComponentsInChildren<Material>()
		base2=base1.GetChild(0);
		muzzle=base2.GetChild(0);
	}
    /*
	void trackTurret(Vector3 target)
	{
		Vector3 delt=target-base1.position;
		Vector3 dxz=new Vector3(Vector3.Dot(ourTransform.right,delt),0,Vector3.Dot(ourTransform.forward,delt));
		Vector3 dup1=new Vector3(0,1,0);
		//float theta=atan2(dxz.x,dxz.z);
		Quaternion q=new Quaternion();
		q.SetLookRotation(dxz,dup1);//dup1,dxz);
		base1.transform.localRotation=q;
		
		delt=target-base2.position;
		dxz=new Vector3(Vector3.Dot(base1.right,delt),Vector3.Dot(base1.up,delt),Vector3.Dot(base1.forward,delt));
		//dxz=new Vector3(Vector3.Dot(base1.right,delt),Vector3.Dot(base1.up,delt),Vector3.Dot(base1.forward,delt));
		//q.SetLookRotation(Vector3.Cross(base1.up,dxz),dup1);
		q.SetLookRotation(dxz,dup1);
		base2.transform.localRotation=q;
	}*/
	
	// Update is called once per frame
	public override void unitUpdate () {
		if(checkCounter++>=checkInterval){
			scanResult.Clear();
			root.playerManager.findPlayersInRadius(gameObject.transform.position,30.0f,ref scanResult);
            /*
			foreach(PlayerManager.Player plyr in scanResult){
				//if(owningPlayer!=plyr)
				{
					//If target is not owner of turret...
					//plyr.applyDamage(0.1f);
					trackTurret(plyr.body.transform.position);
					if(fireCounter++>=fireInterval){
						
						GameObject	projectile = (GameObject)GameObject.Instantiate (Resources.Load ("Objs/laserBolt"), muzzle.position,muzzle.rotation);//, 0);
						
						fireCounter=0;
					}
					break;
				}
			}*/
			checkCounter=0;
		}
		if(muzzleFlash>0.0f){
			muzzle.gameObject.renderer.materials[0].color=new Color(1,1,1,muzzleFlash);
			muzzleFlash-=0.1f;
		}
	}
}
