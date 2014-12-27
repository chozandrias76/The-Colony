using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerManager
{
	
	public	class Player
	{
		public string		name;
		public GameObject	body;
		public float		hp	=	0.75f;
		public float		hpMax = 1.0f;
		public float		mineral = 50.0f;
		public float		mineralMax = 500.0f;
		public float		power = 0.0f;
		public float		powerMax = 1.0f;
		public float		armor = 0.5f;
		public float		armorMax = 1.0f;
		public bool			isLocal = false;
		PangoWorld			root;
		public void applyDamage(float amt){
			armor-=amt;
			if(armor<0.0f)
			{	amt=armor;
				armor=0;
				hp+=amt;
				if(hp<0.0f){
					hp=0.0f;
					body.transform.position=new Vector3(0,0,0);
					
					root.chatManager.localChat("PLAYER DESTROYED.");
					root.world.update();
					root.world.primeCache();
					//GameObject.Destroy(body,3.0f);
				}
			}
			updateHUD();
		}
		
		public void applyMinerals(float amt){
			mineral+=amt;
			if(mineral<0.0f)mineral=0.0f;
			if(mineral>mineralMax)mineral=mineralMax;
		}
		
		public void updateStats(){
			bool dirty=false;
			
			if(hp<hpMax){
				hp+=0.001f;
				if(hp>hpMax)
					hp=hpMax;
				dirty=true;
			}
			if(armor<armorMax){
				float repair=World.fmin(0.002f,power);
				power-=repair;
				if(power<0.0f){repair+=power;power=0;}
				armor+=repair;
				if(armor>armorMax){power+=(armor-armorMax);armor=armorMax;}
				dirty=true;
			}
			if(power<powerMax){
				float	generate=World.fmin(0.001f,mineral);
				mineral-=generate;
				if(mineral<0.0f){generate+=mineral;mineral=0.0f;}
				power+=generate;
				if(power>powerMax){mineral+=(power-powerMax);power=powerMax;}
				dirty=true;
			}
			if(dirty && isLocal){
				updateHUD();
			}
		}
		
		public void updateHUD (){
            /*
			Transform root=body.transform.FindChild("Main Camera").FindChild("HUDModel");
			Transform t=root.FindChild("HUDHitpoints");
			t.localScale=new Vector3(hp/hpMax,t.localScale.y,t.localScale.z);
			t=root.FindChild("HUDArmor");
			t.localScale=new Vector3(armor/armorMax,t.localScale.y,t.localScale.z);
			t=root.FindChild("HUDEnergy");
			t.localScale=new Vector3(power/powerMax,t.localScale.y,t.localScale.z);
			t=root.FindChild("HUDMinerals");
			t.localScale=new Vector3(mineral/mineralMax,t.localScale.y,t.localScale.z);
             */
		}
		public Player(PangoWorld _world){
			root=_world;
		}
	}

	PangoWorld							root;
	List<Player>						players=new List<Player>();
	Dictionary<uLink.NetworkViewID,Player>	playerObjects=new Dictionary<uLink.NetworkViewID, Player>();

	public Player						localPlayer;
	
	public void dumpPlayers(){
		foreach(Player p in players){
			root.chatManager.localChat("Player:"+p.body.GetComponent<uLink.NetworkView>().viewID.ToString());
		}
	}
	
	
	public void activatePlayerControls (GameObject playerObject, bool active)
	{
		Transform ocam=playerObject.transform.FindChild ("Main Camera");
//		ocam.camera.enabled = active;
//		ocam.GetComponent<AudioListener>().enabled=active;
		//ocam.gameObject.GetComponent<MouseLook> ().enabled = active;
		playerObject.GetComponent<CharacterController> ().enabled = active;
		playerObject.GetComponent<MouseLook> ().enabled = active;
		playerObject.GetComponent<CharacterMotor> ().enabled = active;
		playerObject.GetComponent<FPSInputController> ().enabled = active;
	}
	
	public Player createLocalPlayer(GameObject	body){
		Player player=findPlayerFromObject(body);//registerPlayerObject(body);
		localPlayer=player;
		player.isLocal=true;
		player.updateHUD();
		activatePlayerControls(player.body,true);
		
		Transform ocam=player.body.transform.FindChild ("Main Camera");
		ocam.camera.enabled = true;
		ocam.GetComponent<AudioListener>().enabled = true;
		
		return player;
	}
	
	public void deRegisterPlayerObject(GameObject body){
		uLink.NetworkViewID nvid=body.GetComponent<uLink.NetworkView>().viewID;
		Player p=findPlayerFromObject(body);
		playerObjects.Remove(nvid);
		players.Remove(p);
		if(localPlayer==p){
			localPlayer=null;
			
		}
	}
	
	public Player registerPlayerObject(GameObject body){
		Player player=new Player(root);
		player.body=body;
		uLink.NetworkViewID nvid=body.GetComponent<uLink.NetworkView>().viewID;
		playerObjects[nvid]=player;
		Debug.Log ("Registering player:"+nvid.ToString());
		players.Add(player);
		if(root.viewerObject==body){
			//Hack, if this is our local player...
			createLocalPlayer(body);
		}
		return player;
	}
	
	
	public void findPlayersInRadius(Vector3	pos,float	rad,ref List<Player>  result){
		float sqrad=rad*rad;
		foreach(Player p in players)
			if((p.body.transform.position-pos).sqrMagnitude < sqrad)
				result.Add(p);
	}
	
	public Player	findPlayerFromViewID(uLink.NetworkViewID vi){
		if(playerObjects.ContainsKey(vi))
			return playerObjects[vi];
		return null;
	}
	public Player	findPlayerFromObject(GameObject gameObject){
		uLink.NetworkView view=gameObject.GetComponent<uLink.NetworkView>();
		if(view==null)
			return null;	
		return findPlayerFromViewID(view.viewID);
	}
	
	public void	updatePlayers(){
		foreach(Player p in players)
			p.updateStats(); 
	}
	public PlayerManager (PangoWorld _root)
	{
		root=_root;
	}
}


