using UnityEngine;
using System.Collections;

public class DebugPlayerGUI : uLink.MonoBehaviour {
    
    public float updateInterval =0.5f;
    
    private float accum = 0;//FPS over interval
    private int frames = 0;//Frames over Interval
    private float timeleft;//Left time for current interval
    public int DebugGUIWidth = 100;
    public int DebugGUIHeight = 200;
    public GUISkin debugSkin;
    public string format;
    //private CellData debugData = new CellData();
    GameObject player;
    GameMap _GameMap = new GameMap();
    //private Vector3 cellLocation;
    public float fps;
    

    // Use this for initialization
    void Start () {
        timeleft = updateInterval;
        _GameMap = GameObject.Find ("_gameLogic").GetComponent<GameMap>();
    }
    static string inputFrame;
    // Update is called once per frame
    void Update () {
            
        timeleft -= Time.deltaTime;
        accum += Time.timeScale/Time.deltaTime;
        ++frames;
     
        // Interval ended - update GUI text and start new interval
        if( timeleft <= 0.0 )
        {
            // display two fractional digits (f2 format)
            fps = accum/frames;
            format = System.String.Format("{0:F2} FPS",fps);
            //  DebugConsole.Log(format,level);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
		}
            
    }
    
    void OnGUI () {
        GUI.skin = debugSkin;
        GUI.Window(1,new Rect(Screen.width - DebugGUIWidth, 0, DebugGUIWidth, DebugGUIHeight),DebugWindow, "Debug");
    }
    public void DebugWindow (int id){
        
        player = gameObject;
        
        int fpsWidth = 100;
        int fpsHeight = 50;
        GUI.Label(new Rect(0,20, fpsWidth, fpsHeight),format);

            if(GUI.Button(new Rect(0,60,fpsWidth, fpsHeight),"Drop Oxygen\nValue to 0"))
        {
            //CellData testData = GameMap.GetCell(player.transform.position);
            //int2 cellLocation = new GameMap.CellCoord(player.transform.position).coord;
            //cellLocation.x = cellLocation.x - 128;
            //cellLocation.y = cellLocation.y - 128;
            //Debug.Log(player.transform.position.ToString());
            //Debug.Log(GameMap.floors[GameMap.CellKey(player.transform.position)].location + " " + GameMap.floors[GameMap.CellKey(player.transform.position)].exists.ToString());
			//string powerGridAtLocation = System.String.Format("East Power: {0}; West Power: {1}; South Power: {2}; North Power: {3}" ,testData.powerCell.east.ToString() ,testData.powerCell.west.ToString() ,testData.powerCell.south.ToString() ,testData.powerCell.north.ToString() ) ;
			//Debug.Log (powerGridAtLocation);
            //GetComponent<CreatePlayerCellData>().playerData.oxy = 0;
            var view = uLink.NetworkView.Get(player);
            float oxygen = GetComponent<CreatePlayerCellData>().playerData.oxy;
            oxygen = 0.0f;
            uLink.NetworkView.Get(this).RPC("SendData", uLink.RPCMode.Server, oxygen);
            
        }
        
        if(GUI.Button(new Rect(0,150,fpsWidth, fpsHeight),"Up Oxygen\nValue 2x"))
        {
            CellData testData = GameMap.GetCell(player.transform.position);
            int2 cellLocation = new GameMap.CellCoord(player.transform.position).coord;
            testData.oxy = testData.oxy*2;
            testData = null;
            
        }
    

    }
}