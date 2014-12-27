#region License

// // PangoWorld.cs
// //  
// //  Author:
// //        <colin.p.swensonh@gmail.com>
// // 
// //  Copyright (c) 2013 swensonhcp
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using UnityEngine;

public class PangoWorld : MonoBehaviour
{
    public GameObject cameraObject;
   // public ChatManager chatManager;

    //private bool mouseCaptured = true;
    //private float seaLevel;
    private bool simIsPaused;
    //private bool simRunning = true;
    public GameObject viewerObject;
    public GameObject waterObject;
    public World world;

    private PangoWorld()
    {
    }

    private void Reset()
    {
    }


    private void Start()
    {
        world = new World(this);
//		playerManager = new PlayerManager(this);
//        chatManager = new ChatManager(this);
//		buildManager = new BuildManager(this);

        cameraObject = GameObject.Find("Camera");

        waterObject = GameObject.Find("Daylight Simple Water");

        //seaLevel = waterObject.transform.position.y;


        //Debug.Log ("START.");

        Screen.showCursor = false;
        Screen.lockCursor = true;

        viewerObject = GameObject.Find("Camera"); //First Person Controller");
        //	viewerObject.layer = LayerMask.NameToLayer ("Ignore Raycast");

        world.primeCache();

//		pauseSim (true);
    }

    private void Update()
    {
        if (world == null)
        {
            //Application.Quit();
            Debug.Log("Pangosim has been reinstantiated!!");
            //Application.LoadLevel(0);
            //Start ();
        }

        world.update();

        //chatManager.Update();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (simIsPaused)
            {
                simIsPaused = false;
                //pauseSim (false);
            }
            else
            {
                simIsPaused = true;
                //pauseSim (true);
            }
        }

//		playerManager.updatePlayers();

//		buildManager.Update ();

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


    private void OnGUI()
    {
        //int ry=0;
        if (simIsPaused)
        {
            GUI.TextArea(new Rect(10, 10, 150, 100),
                "Controls: AWSD   E=Grenade  Ctrl-LMouse=Build Ctrl-RMouse=Destroy Esc=Toggle UI");
            if (GUI.Button(new Rect(10, 115, 150, 25), "Resume.."))
            {
                //pauseSim (false);
            }
        }
        //chatManager.Render();
    }

    public void attachCameraToObject(GameObject go)
    {
        cameraObject.camera.transform.transform.position = go.transform.position;
        cameraObject.camera.transform.parent = go.transform;
        cameraObject.GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
    }


//	void	captureMouse (bool sieze)
//	{
//		
////		if(mouseCaptured==true&&
//		if (sieze == true) {
//			
//			if (playerManager.localPlayer!=null) {
//				playerManager.activatePlayerControls (playerManager.localPlayer.body, true);
//			}
//			Screen.showCursor = false;
//			Screen.lockCursor = true;
//			mouseCaptured = false;
//		} else {// if(mouseCaptured==false&&sieze==true){
//			
//			if (playerManager.localPlayer!=null) {
//				playerManager.activatePlayerControls (playerManager.localPlayer.body, false);
//			}
//			Screen.showCursor = true;
//			Screen.lockCursor = false; 
//			mouseCaptured = true;
//		}
//	}

//	void pauseSim (bool	pause)
//	{
//		if (pause == true && simRunning == true) {
//			simRunning = false;
//			simIsPaused = true;
////			Time.timeScale=0.0f;
//			captureMouse (false);
//			Debug.Log ("Sim paused");	
//
//		} else if (pause == false && simRunning == false) {
//			simRunning = true;
//			simIsPaused = false;
////			Time.timeScale=1.0f;
//			captureMouse (true);
//			Debug.Log ("Sim resumed.");
//		}
//	}
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