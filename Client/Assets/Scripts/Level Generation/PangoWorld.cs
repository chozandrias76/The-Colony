using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public class PangoWorld : MonoBehaviour
{
	public World			world;

	public PlayerManager	playerManager;
	public ChatManager		chatManager;
	public BuildManager		buildManager;
	
	public GameObject	viewerObject;
	public GameObject	waterObject;
	public GameObject	cameraObject;
	
	float				seaLevel;
	
	bool				mouseCaptured = true;
	bool 				simRunning = true;
	bool 				simIsPaused = false;
	
	PangoWorld(){
	
	}
	
	void Reset ()
	{
	}
	
	
	void Start ()
	{
		world = new World (this);
		playerManager = new PlayerManager(this);
		chatManager = new ChatManager(this);
		buildManager = new BuildManager(this);

		cameraObject = GameObject.Find ("Camera");
		
		waterObject = GameObject.Find ("Daylight Simple Water");
		
		seaLevel=waterObject.transform.position.y;
		
		
		//Debug.Log ("START.");
		
		Screen.showCursor = false;
		Screen.lockCursor = true;
		
		viewerObject = GameObject.Find ("Camera");//First Person Controller");
	//	viewerObject.layer = LayerMask.NameToLayer ("Ignore Raycast");
		
		world.primeCache ();
		
		pauseSim (true);
	}
	
	void Update ()
	{
		if(world==null){
			//Application.Quit();
			Debug.Log ("Pangosim has been reinstantiated!!");
			//Application.LoadLevel(0);
			//Start ();
		}
		
		world.update ();
		
		chatManager.Update ();
		
		
		if (Input.GetKeyDown (KeyCode.Escape) == true) {
			if (simIsPaused) {
				simIsPaused = false;
				pauseSim (false);
			} else {
				simIsPaused = true;
				pauseSim (true);
			}
		}
		
		playerManager.updatePlayers();

		buildManager.Update ();

		//Load/generate queued regions one per frame...
		//world.updateGenerator ();
		
        //if (waterObject != null) {	//Make water object follow player...
        //    Vector3 vpos = viewerObject.transform.position;
        //    vpos.y = waterObject.transform.position.y;
        //    waterObject.transform.position = vpos;
        //}
        /*
		if (world.isRegionLoaded (world.regionName) == false) {
			if (simIsPaused == false)
				pauseSim (true);
		} else {//Keep updating the players current region hasnt loaded yet..	
			if (simIsPaused == false)
				pauseSim (false);
		}
        */
        if (simIsPaused)
        {
            GameObject.Find("_gameLogic").GetComponent<GameMap>().enabled = false;
        }
        else if (!simIsPaused)
        {
            GameObject.Find("_gameLogic").GetComponent<GameMap>().enabled = true;
        }
	}
	
	
	void OnGUI ()
	{
		//int ry=0;
		if (simIsPaused == true) {
			GUI.TextArea (new Rect (10, 10, 150, 100), "Controls: AWSD   E=Grenade  Ctrl-LMouse=Build Ctrl-RMouse=Destroy Esc=Toggle UI");
			if (GUI.Button (new Rect (10, 115, 150, 25), "Resume..")) {
				pauseSim (false);
			}
			
			
		}
		chatManager.Render ();
		
	}
	
	public void attachCameraToObject (GameObject go)
	{
		cameraObject.camera.transform.transform.position = go.transform.position;
		cameraObject.camera.transform.parent = go.transform;
		cameraObject.GetComponent<MouseLook> ().axes = MouseLook.RotationAxes.MouseY;
	}
	
	
	void	captureMouse (bool sieze)
	{
		
//		if(mouseCaptured==true&&
		if (sieze == true) {
			
			if (playerManager.localPlayer!=null) {
				playerManager.activatePlayerControls (playerManager.localPlayer.body, true);
			}
			Screen.showCursor = false;
			Screen.lockCursor = true;
			mouseCaptured = false;
		} else {// if(mouseCaptured==false&&sieze==true){
			
			if (playerManager.localPlayer!=null) {
				playerManager.activatePlayerControls (playerManager.localPlayer.body, false);
			}
			Screen.showCursor = true;
			Screen.lockCursor = false; 
			mouseCaptured = true;
		}
	}
	
	void pauseSim (bool	pause)
	{
		if (pause == true && simRunning == true) {
			simRunning = false;
			simIsPaused = true;
//			Time.timeScale=0.0f;
			captureMouse (false);
			Debug.Log ("Sim paused");	

		} else if (pause == false && simRunning == false) {
			simRunning = true;
			simIsPaused = false;
//			Time.timeScale=1.0f;
			captureMouse (true);
			Debug.Log ("Sim resumed.");
		}
	}
	/*
	[Serializable ()]
	public class SaveData : ISerializable {
		public SaveData(){
		}
	}
	public void Save () {

		SaveData data = new SaveData ();
	
		Stream stream = File.Open("MySavedGame.game", FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
	        bformatter.Binder = new VersionDeserializationBinder(); 
		Debug.Log ("Writing Information");
		bformatter.Serialize(stream, data);
		stream.Close();
	}

public void Load () {

	SaveData data = new SaveData ();
	Stream stream = File.Open("MySavedGame.gamed", FileMode.Open);
	BinaryFormatter bformatter = new BinaryFormatter();
	bformatter.Binder = new VersionDeserializationBinder(); 
	Debug.Log ("Reading Data");
	data = (SaveData)bformatter.Deserialize(stream);
	stream.Close();
}
	*/
	
	
	
	
	
	
}