using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager
{
	PangoWorld						root;
	float 							curEditRadius = 3.3f;
	RaycastHit 						hit = new RaycastHit ();
	GameObject 						cursorObject;
	int								editCooldown = 0;
	int								lastSelectedSlot = -1;
	int								selectedSlot = 0;
	public static Shader			diffuseShader=Shader.Find("Diffuse");
	public static Shader			transparentShader=Shader.Find ("Transparent/Diffuse");
	public static int 				action_fire = 0;
	public static int 				action_mine = 1;
	public static int 				action_build = 2;
	
	class Tool
	{
		public static Dictionary<int,Tool>	toolMap=new Dictionary<int,Tool>();
		public GameObject	cursor;
		public int			id;
		public float 		energyCost;
		public float 		mineralCost;
		public bool			canApply(PlayerManager.Player	player){
			if((player.power >= energyCost)
				&&(player.mineral>=mineralCost))
				return true;
			return false;
		}
		
		
		public void setUnitState(GameObject obj,int state){
			Unit t=obj.GetComponent<Unit>();//Activate its script if any...
			if(t!=null)
				t.setUnitState(state);
			else{
				obj.layer = LayerMask.NameToLayer ("Ignore Raycast");
			}
		}
		
		
		
		public Tool(int _id,GameObject	c,float _energyCost,float	_mineralCost){
			cursor=c;
			
			id=_id;
			toolMap[id]=this;
			energyCost=_energyCost;
			mineralCost=_mineralCost;
			
			setUnitState(cursor,Unit.unit_cursor_cant_build);
		}
	}
	
	Tool[]	toolSlots=new Tool[]{
		new Tool(0,GameObject.CreatePrimitive (PrimitiveType.Sphere),1,1),
		//new Tool(1,(GameObject)GameObject.Instantiate(Resources.Load("Objs/TurretBase")),0,0),
		//new Tool(2,(GameObject)GameObject.Instantiate(Resources.Load("Objs/Silo")),0,0),
		//new Tool(3,(GameObject)GameObject.Instantiate(Resources.Load("Objs/PowerNode")),0,0),
	};
	
	

	public void	targetAction (uLink.NetworkViewID	netviewID, Vector3	pos, Vector3	look, float power, int action)
	{
		
		if (action == BuildManager.action_fire){
			Grenade.fireGrenade (root, pos, look);
		}else if (action == action_mine) {
			
			GameObject psys = (GameObject)GameObject.Instantiate (Resources.Load ("Sparks"));
			psys.transform.position = pos;
			GameObject.Destroy (psys, 0.5f);
			ParticleEmitter pe = (ParticleEmitter)psys.GetComponent ("ParticleEmitter");
			pe.localVelocity = look * 6.0f;
			
			float sumDensity=root.world.editWorld (root.gameObject, pos, curEditRadius, power, World.cubeTool);
			PlayerManager.Player player = root.playerManager.findPlayerFromViewID(netviewID);
			if(player!=null){
				if(sumDensity<-0.01f)
				{
					root.chatManager.localChat(String.Format("Acquired {0:0.00} minerals.",-sumDensity));
				}
				else if(sumDensity>0.01f)
				{
					root.chatManager.localChat(String.Format("Depleted {0:0.00} minerals.",sumDensity));
				}
			}
		}else if(action == action_build){
			
		}
	}
	
	void enableCursor(GameObject cursor,bool enable){
		if(cursor.active==enable)
			return;
		cursor.active=enable;
		Unit u=cursor.GetComponent<Unit>();
		if(u!=null)
			u.setUnitState(enable?Unit.unit_cursor_can_build:Unit.unit_cursor_cant_build);
/*		Renderer[] renderers = cursor.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers)
			r.enabled = enable;
		if(enable){
			setShaderRecursive(cursor,transparentShader);
		}
*/
	}
	
	void setCursor(GameObject	ncurs){
		if(cursorObject!=null)
			enableCursor(cursorObject,false);
		cursorObject = ncurs;
		enableCursor(cursorObject,true);
	}
	
	void updateToolSlots(){
		if(lastSelectedSlot!=selectedSlot){
			lastSelectedSlot=selectedSlot;
			setCursor(toolSlots[selectedSlot].cursor);
		}
	}
	
	public void Update ()
	{
		if (Camera.main == null)
			return;
		if(Input.GetKeyDown(KeyCode.F1))selectedSlot=0;
		else if(Input.GetKeyDown(KeyCode.F2))selectedSlot=1;
		else if(Input.GetKeyDown(KeyCode.F3))selectedSlot=2;
		else if(Input.GetKeyDown(KeyCode.F4))selectedSlot=3;
		updateToolSlots();
		
		bool editEnabled = Input.GetKey (KeyCode.LeftControl);
		Transform cam = Camera.main.transform;
		editCooldown--;
		//float crsScl=0.25f;
		//float dur=0.1f;
		if (cursorObject != null) {
			if (editEnabled == false){
				enableCursor(cursorObject,false);
				return;
			}else
				enableCursor(cursorObject,true);
		}
		
		/*	Debug.DrawRay(cam.position, cam.forward*hit.distance);//,Color.green,dur);
			Debug.DrawRay(hit.point+new Vector3(0,-crsScl,0), new Vector3(0,2*crsScl,0));//,Color.red,dur);
			Debug.DrawRay(hit.point+new Vector3(-crsScl,0,0), new Vector3(2*crsScl,0,0));//,Color.red,dur);
			Debug.DrawRay(hit.point+new Vector3(0,0,-crsScl), new Vector3(0,0,2*crsScl));//,Color.red,dur);
		*/
		bool gotHit = Physics.Raycast (cam.position + (cam.forward * World.fmax (2.1f,(curEditRadius*2))), cam.forward, out hit, 200);
		if (gotHit) {
			enableCursor(cursorObject,true);
				
			//renderer.enabled=true;
			PlayerManager.Player player = root.playerManager.localPlayer;
			Tool ptool=toolSlots[selectedSlot];
			if(ptool.canApply(player))
				cursorObject.GetComponent<MeshRenderer>().materials[0].color=Color.green;
			else
				cursorObject.GetComponent<MeshRenderer>().materials[0].color=Color.red;
			
			cursorObject.transform.position = hit.point;
			Vector3 crs=Vector3.Cross(hit.normal,cam.forward);
			crs.Normalize();
	        Quaternion rotation = new Quaternion();
	        rotation.SetLookRotation(crs,hit.normal);
	        cursorObject.transform.rotation = rotation;
			
		} else {
			enableCursor(cursorObject,false);
		}
	
		if (editEnabled && gotHit && (Input.GetMouseButton (0) || Input.GetMouseButton (1)) && (editCooldown <= 0)) {
			editCooldown = 25;
			
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
			// Update is called once per frame
			//if (Physics.Raycast(ray, out hit, 500.0f))
			{
				//if (hit.transform.tag != "Untagged") 
				{	
					//Debug.Log ("Hit:" + hit.transform.tag);
					//if (hit.transform.tag == "Finish")
					
					float editValue = Input.GetMouseButton (0) ? -1.0f : 1.0f;
					
					if(selectedSlot==0){//"Sphere"){
						//Debug.Log ("Fire the missiles");
						//float crsScl=0.25f;
						//Debug.DrawRay(cam.position, cam.forward*hit.distance,Color.green,5.0f);
						//Debug.DrawRay(hit.point+new Vector3(0,-crsScl,0), new Vector3(0,2*crsScl,0),Color.red,5.0f);
						//Debug.DrawRay(hit.point+new Vector3(-crsScl,0,0), new Vector3(2*crsScl,0,0),Color.red,5.0f);
						//Debug.DrawRay(hit.point+new Vector3(0,0,-crsScl), new Vector3(0,0,2*crsScl),Color.red,5.0f);

					}else{
						Transform ptrans=cursorObject.transform;
					}
				}
			}
		}
	}
	
	public BuildManager (PangoWorld _root)
	{
		root=_root;
		selectedSlot = 0;
	}
}


