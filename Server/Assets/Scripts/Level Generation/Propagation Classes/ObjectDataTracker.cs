using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ObjectDataTracker
{
	public static ObjectDataTracker Singleton;
	public const int ObjectDataSize = 512 + (512*512);
	
	public FloorData[] floors = new FloorData[CellDataTracker.CellDataSize];
    public WallData[] walls = new WallData[ObjectDataSize];
    public DoorData[] doors = new DoorData[ObjectDataSize];
    public PowerData[] powerDatas = new PowerData[ObjectDataSize];
	
    public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
	
	public WallData GetWall(Vector3 pos)//Returns Walldata at 3d world vector
    {
        return walls[CellDataTracker.Singleton.ObjectKey(pos)];
    }
	
	public WallData GetWall(int2 pos)//Returns Walldata at 3d world vector
    {
        return walls[CellDataTracker.Singleton.ObjectKey(pos)];
    }

    public FloorData GetFloor(Vector3 pos)//Returns FloorData at 3d world vector
    {
        return floors[CellDataTracker.Singleton.CellKey(pos)];
    }
	
	public FloorData GetFloor(int2 pos)//Returns FloorData at 3d world vector
    {
        return floors[CellDataTracker.Singleton.CellKey(pos)];
    }
	
	public PowerData GetPower(Vector3 pos)
	{
		return powerDatas[CellDataTracker.Singleton.ObjectKey(pos)];
	}
	
	public PowerData GetPower(int2 pos)
	{
		return powerDatas[CellDataTracker.Singleton.ObjectKey(pos)];
	}
	
	public int ObjectKey(Vector3 position)//Returns CellKey for objects in different grid spacing
    {
        int cx = 256 + (int) Mathf.Floor((position.x + 1.25f)/2.5f);
        int cy = 256 + (int) Mathf.Floor((position.z + 1.25f)/2.5f);
        return cx + (cy*512);
	}
    
	public int CellKey(Vector3 position)//Converts world coords to the dictonary key
    {
        int cx = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
        int cy = 128 + (int) Mathf.Floor((position.z + 2.5f)/5f);
        return cx + (cy*256);
    }
	
	public class ObjectCoord
    {
        public int2 coord;

        public ObjectCoord(int x, int y)
        {
            coord.x = x;
            coord.y = y;
        }

        public ObjectCoord(Vector3 position)
        {
            coord.x = 256 + (int) Mathf.Floor((position.x + 1.25f)/5f);
            coord.y = 256 + (int) Mathf.Floor((position.z + 1.25f)/5f);
        }
		
		public Vector3 location
		{
			get
			{
				return new Vector3(coord.x, 0, coord.y);
			}
			set
			{
			}
		}

        public int key
        {
			get
			{
				return coord.x + (coord.y*512);
			}
			set
			{
			}
        }
    }
	
	public void GenerateObjectData()
		//Finds out where walls doors and power cables are
    {
        var sceneObjects = (GameObject[]) GameObject.FindSceneObjectsOfType(typeof (GameObject));
        var coords = new int2(-128, -128);
        Vector3 location;
        for (int i = 0; i < ObjectDataSize; i++)
        {
            location = new Vector3(coords.x, 0, coords.y);
            if (i%256 == 0)
            {
                coords.y++;
                coords.x = 0;
            }
            else
            {
                coords.x++;
            }
            walls[i] = new WallData();
            walls[i].location = location;
            doors[i] = new DoorData();
            doors[i].location = location;
            powerDatas[i] = new PowerData();
        }
        coords = new int2(-128, -128);
        for (int i = 0; i < CellDataTracker.CellDataSize; i++)
        {
            location = new Vector3(coords.x, 0, coords.y);
            if (i%256 == 0)
            {
                coords.y++;
                coords.x = 0;
            }
            else
            {
                coords.x++;
            }
            floors[i] = new FloorData();
            floors[i].location = location;
        }

        for (int index = 0; index < sceneObjects.Length; index++)
        {
			#region Finding Walls
            if (sceneObjects[index].name.Contains("Wall"))
            {
                if (sceneObjects[index].name.Contains("Wall Corner"))
                {
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].exists = true;
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].exists = true;

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].direction =
                            new Vector3(0, 0, -1);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].direction =
                            new Vector3(-1, 0, 0);

                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f);
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].exists = true;
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].exists =
                            true;

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].direction =
                            new Vector3(0, 0, 1);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].direction =
                            new Vector3(-1, 0, 0);

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].location =
                            sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].location =
                            sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f);
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].exists =
                            true;
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].exists =
                            true;

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].direction =
                            new Vector3(0, 0, 1);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].direction =
                            new Vector3(1, 0, 0);

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].location =
                            sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].location =
                            sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f);
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].exists =
                            true;
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].exists = true;

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].direction =
                            new Vector3(0, 0, -1);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].direction =
                            new Vector3(1, 0, 0);

                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].location =
                            sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0);
                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].location =
                            sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f);
                    }
                }

                else if (sceneObjects[index].name.Contains("Wall T"))
                {
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(1, 0, 2);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, -1);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(-1, 0, 2);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, 1);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                }

                else if (sceneObjects[index].name.Contains("Wall") && !sceneObjects[index].name.Contains("Corner") &&
                         !sceneObjects[index].name.Contains("Wall T") &&
                         !sceneObjects[index].name.Contains("Wall Section"))
                {
                    walls[ObjectKey(sceneObjects[index].transform.position)].exists = true;
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0) ||
                        sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, 0);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0) ||
                        sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(0, 0, 2);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                }
            }
			#endregion
			#region Finding Doors
            else if (sceneObjects[index].name.Contains("Door"))
            {
                if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0) ||
                    sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
                {
                    doors[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(0, 0, 2);
                    doors[ObjectKey(sceneObjects[index].transform.position)].location =
                        sceneObjects[index].transform.position;
                }
                if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0) ||
                    sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
                {
                    doors[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, 0);
                    doors[ObjectKey(sceneObjects[index].transform.position)].location =
                        sceneObjects[index].transform.position;
                }
            }
			#endregion
			#region Finding Floors
            else if (sceneObjects[index].name.Contains("Floor Object"))
            {
                try
                {
                    floors[CellKey(sceneObjects[index].transform.position)].exists = true;
                    floors[CellKey(sceneObjects[index].transform.position)].location =
                        sceneObjects[index].transform.position;
                    //cellsContents[CellKey(sceneObjects[index].transform.position)].oxy = 0.2f;
                }
                catch
                {
                }
            }
			#endregion
        }
    }
	
	private static Vector3 zeroRot;
	private static Vector3 halfPIRot;
	private static Vector3 PIRot;
	private static Vector3 threeHalfPIRot;
	private static List<GameObject> wireList;
	//private GameMap currGameMap;
	private GameObject[] goInScene;
	public BitArray directionVectors = new BitArray (4, false);
	public void CreatePowerGrid()
	{
		zeroRot = new Vector3 (0, 0, 0);
		halfPIRot = new Vector3 (0, 90, 0);
		PIRot = new Vector3 (0, 180, 0);
		threeHalfPIRot = new Vector3 (0, 270, 0);
		wireList = new List<GameObject> ();
		//goInScene = new GameObject[10000];
		//currGameMap = new GameMap ();
		
		goInScene = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];
		
		foreach (GameObject go in goInScene) {
			if (go.name.Contains ("Wire Elbow")
                || go.name.Contains ("Wire Quad")
                || (go.name.Contains ("Wire Straight") & !go.name.Contains ("Half"))
                || go.name.Contains ("Wire Straight Half")
                || go.name.Contains ("Wire T")) {
				wireList.Add (go);
			}
		}
        
		for (int i = 0; i < wireList.Count; i++) {
				
			
			var powerData = CellDataTracker.Singleton.GetCell(wireList[i].transform.position).thisPower;
			
                
			Vector3 angle = wireList [i].transform.rotation.eulerAngles;
			
			if (wireList[i].name.Contains ("Wire Straight") && !wireList [i].name.Contains ("Half")) {

				if (angle == zeroRot) {//N-S
					powerData.connections.Set (0, true);
					powerData.connections.Set (1, true);
				}   

				if (angle == halfPIRot) {//W-E
					
					powerData.connections.Set (2, true);
					powerData.connections.Set (3, true);
				}       
                        
				if (angle == PIRot) {//S-N
					powerData.connections.Set (0, true);
					powerData.connections.Set (1, true);
				}       
				
                        
				if (angle == threeHalfPIRot) {//E-W
					powerData.connections.Set (2, true);
					powerData.connections.Set (3, true);
				}       
                    
			}
			if (wireList [i].name.Contains ("Wire Elbow")) {
				//Debug.Log("Elbow");
				if (angle == zeroRot) {//S-W
					powerData.connections.Set(1, true);
					powerData.connections.Set(3, true);
				}
                        
				if (angle == halfPIRot) {//W-N
					powerData.connections.Set(0, true);
					powerData.connections.Set(3, true);
				}   
                        
				if (angle == PIRot) {//N-E
					powerData.connections.Set(0, true);
					powerData.connections.Set(2, true);
				}   
                        
				if (angle == threeHalfPIRot) {//W-S
					powerData.connections.Set(1, true);
					powerData.connections.Set(3, true);
				}   
			}
			if (wireList [i].name.Contains ("Wire Straight Half")) {
				//Debug.Log("Half");
				if (angle == zeroRot) {//
                        
				}       
                        
				if (angle == halfPIRot) {//
                        
				}       
                        
				if (angle == PIRot) {//
                        
				}       
                        
				if (angle == threeHalfPIRot) {//
                        
				}       
                    
			}
			if (wireList [i].name.Contains ("Wire T")) {
				//Debug.Log("T");
				if (angle == zeroRot) {//W-N-E
					powerData.connections.Set(0, true);
					powerData.connections.Set(2, true);
					powerData.connections.Set(3, true);
				}       
                        
				if (angle == halfPIRot) {//N-E-S
					powerData.connections.Set(0, true);
					powerData.connections.Set(1, true);
					powerData.connections.Set(2, true);
				}       
                        
				if (angle == PIRot) {//E-S-W
					powerData.connections.Set(1, true);
					powerData.connections.Set(2, true);
					powerData.connections.Set(3, true);
				}       
                        
				if (angle == threeHalfPIRot) {//S-W-N
					powerData.connections.Set(0, true);
					powerData.connections.Set(1, true);
					powerData.connections.Set(3, true);
				}       
                    
			}
			if (wireList [i].name.Contains ("Wire Quad")) {
				Debug.Log ("Quad");
				if (angle == zeroRot) {//N-S-E-W
					powerData.connections.SetAll(true);
				}   
                        
				if (angle == halfPIRot) {//W-N-S-E
					powerData.connections.SetAll(true);
				}   
                        
				if (angle == PIRot) {//E-W-N-S
					powerData.connections.SetAll(true);
				}   
                        
				if (angle == threeHalfPIRot) {//S-E-W-N
					powerData.connections.SetAll(true);
				}   
                    
			}     
		}
	}
	
	public void PropigatePower(ObjectCoord objectLocation)
	{
		
		int powerDirection = System.Convert.ToInt32(powerDatas[objectLocation.key].connections);
		
		PowerData northPower = GetPower((objectLocation.coord + new int2(0,1)));
		PowerData southPower = GetPower((objectLocation.coord + new int2(0,-1)));
		PowerData eastPower = GetPower((objectLocation.coord + new int2(1,0)));
		PowerData westPower = GetPower((objectLocation.coord + new int2(-1,0)));
		PowerData thisPower = powerDatas[objectLocation.key];
		BitArray connectedDirections = new BitArray(0, false);
		int connections = 1;
		
		float averageAmps = thisPower.amps;
		if(thisPower != null)
		{
			if(thisPower.electronicsAttached.Count > 0)
			{
				if(thisPower.amps > 0)
				{
					float requiredAmps = 0;
					
					foreach(HeavyElectronic device in thisPower.electronicsAttached)
					{
						requiredAmps += device.powerRequirement;
					}
					
					if(requiredAmps > thisPower.amps)
					{
						thisPower.amps = 0;
					}
					else
					{
						thisPower.amps -= requiredAmps;
					}
					
					foreach(HeavyElectronic device in thisPower.electronicsAttached)
					{
						device.incommingPower = averageAmps/thisPower.electronicsAttached.Count;
					}
				}
			}
		}
		
		switch(powerDirection)//Find out if this location is actually connected to cables around it and track those directions
		{
		case 0://None			
			break;
		case 1://North
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}
			
			break;
		case 2://South
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}
			
			break;
		case 3://North South
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}
			
			break;
		case 4://East
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			
			break;
		case 5://East North
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}
			
			break;
		case 6://East South
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}	

			break;
		case 7://East North South
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}
			
			break;
		case 8://West
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}

			break;
		case 9://West North
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}

			break;
		case 10://West South
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}

			break;
		case 11://West North South
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}

			break;
		case 12://West East
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			break;		
		case 13://West East North
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}
			averageAmps = averageAmps/connections;
			thisPower.amps = averageAmps;
			break;
		case 14://West East South
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}
			break;
		case 15://West East South North
			if(westPower.connections.Get(2) == true)
			{
				averageAmps += westPower.amps;
				connections++;
				connectedDirections.Set(3, true);
			}
			if(eastPower.connections.Get(3) == true)
			{
				averageAmps += eastPower.amps;
				connections++;
				connectedDirections.Set(2, true);
			}
			if(southPower.connections.Get(0) == true)
			{
				averageAmps += southPower.amps;
				connections++;
				connectedDirections.Set(1, true);
			}
			if(northPower.connections.Get(1) == true)
			{
				averageAmps += northPower.amps;
				connections++;
				connectedDirections.Set(0, true);
			}

			break;
		default:
			break;
		}
		
		if(powerDirection >= 0 && powerDirection <= 15)//Whatever the number of connections were, average the amps into the observed location
		{
			averageAmps = averageAmps/connections;
			thisPower.amps = averageAmps;
		}
		
		switch(Convert.ToInt32(connectedDirections))//Once all connections are tracked, make sure to average those connections which were valid from observed location
		{
		case 0:
			break;
		case 1:
			northPower.amps = averageAmps;
			break;
		case 2:
			southPower.amps = averageAmps;
			break;
		case 3:
			northPower.amps = averageAmps;
			southPower.amps = averageAmps;
			break;
		case 4:
			eastPower.amps = averageAmps;
			break;
		case 5:
			eastPower.amps = averageAmps;
			northPower.amps = averageAmps;
			break;
		case 6:
			eastPower.amps = averageAmps;
			southPower.amps = averageAmps;
			break;
		case 7:
			eastPower.amps = averageAmps;
			northPower.amps = averageAmps;
			southPower.amps = averageAmps;
			break;
		case 8:
			westPower.amps = averageAmps;
			break;
		case 9:
			westPower.amps = averageAmps;
			northPower.amps = averageAmps;
			break;
		case 10:
			westPower.amps = averageAmps;
			southPower.amps = averageAmps;
			break;
		case 11:
			westPower.amps = averageAmps;
			northPower.amps = averageAmps;
			southPower.amps = averageAmps;
			break;
		case 12:
			westPower.amps = averageAmps;
			eastPower.amps = averageAmps;
			break;
		case 13:
			westPower.amps = averageAmps;
			eastPower.amps = averageAmps;
			northPower.amps = averageAmps;
			break;
		case 14:
			westPower.amps = averageAmps;
			eastPower.amps = averageAmps;
			southPower.amps = averageAmps;
			break;
		case 15:
			westPower.amps = averageAmps;
			eastPower.amps = averageAmps;
			southPower.amps = averageAmps;
			northPower.amps = averageAmps;
			break;
		default:
			break;
		}
	}
}



