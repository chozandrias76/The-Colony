using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public 	uLink.NetworkViewID 				ownerView;
	public 		PlayerManager.Player	owningPlayer;
	public static int					unit_inactive=0;
	public static int					unit_active=1;
	public static int					unit_cursor_can_build=2;
	public static int					unit_cursor_cant_build=3;
	
	protected PangoWorld				root;
	int							unitState;
	List<Material>				solidMaterials=new List<Material>();
	List<GameObject>				parts=new List<GameObject>();
	
	public virtual void unitStart(){
	}
	
	public virtual void unitUpdate(){
	}
	
	void Start(){
		if(root==null){
			getRoot ();
		}
		findChildren (gameObject,0);
		foreach(GameObject obj in parts){
			obj.layer = LayerMask.NameToLayer ("Ignore Raycast");
			if(obj.renderer.materials[0].shader==BuildManager.diffuseShader){
				solidMaterials.Add(obj.renderer.materials[0]);
			}
		}
	}
	
	void Update(){
		/*
		if(root==null){
			getRoot ();
			unitStart();
		}*/
		
		unitUpdate();
	}
	
	public void findChildren(GameObject obase,int cidx){
		if(cidx==0)
			parts.Add (obase);
		if(cidx>=obase.transform.GetChildCount())
			return;
		Transform child = obase.transform.GetChild(cidx);
		findChildren (child.gameObject,0);
		findChildren (obase.gameObject,cidx+1);
	}
	public void getRoot(){
		root=GameObject.Find("Root").GetComponent<PangoWorld>();
		unitStart();
	}
	
	
	public void initializeUnit(uLink.NetworkViewID viewID){
		if(root==null)
			getRoot ();
		ownerView=viewID;
		owningPlayer=root.playerManager.findPlayerFromViewID(viewID);
	}
	
	public void setUnitState(int newState){
		if(root==null)
			getRoot ();
		if(newState==unit_cursor_can_build || newState==unit_cursor_cant_build){
			enabled=false;		
			foreach(GameObject gobj in parts){
				gobj.active=true;
			}
			Color bcolor=newState==unit_cursor_can_build?Color.green:Color.red;
			foreach(Material mr in solidMaterials){
				mr.shader=BuildManager.transparentShader;
				mr.color=bcolor;
			}
		}else if(newState==unit_active)
		{
			enabled=true;
			foreach(Material mr in solidMaterials){
				mr.shader=BuildManager.diffuseShader;
				mr.color=new Color(1,1,1,1);
			}
			foreach(GameObject gobj in parts)
				gobj.active=true;
			
		}else if(newState==unit_inactive)
		{
			enabled=false;
			foreach(GameObject gobj in parts)gobj.active=false;
		}
	}
	public Unit ()
	{
	}
}


