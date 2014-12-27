using System.Collections;
using UnityEngine;
using MonoBehaviour = uLink.MonoBehaviour;

public class GameMap : MonoBehaviour
{
    private const int TILE_SIZE = 5; //Unity Units = 3ds/100, Tile = 500x500 3ds units
    //public Dictionary<int2, CellData> cellsContents = new Dictionary<int2, CellData>();
    private const int CellDataSize = 256 + (256*256);
    private const int ObjectDataSize = 512 + (512*512);
    public static CellData[] cellsContents = new CellData[CellDataSize];
    public static FloorData[] floors = new FloorData[CellDataSize];
    private static readonly WallData[] walls = new WallData[ObjectDataSize];
    private static readonly DoorData[] doors = new DoorData[ObjectDataSize];
    private static readonly PowerData[] powerCords = new PowerData[ObjectDataSize];

    private static CellData tempWest;
    private static CellData tempEast;
    private static CellData tempSouth;
    private static CellData tempNorth;

    private static readonly int2 right = new int2(1, 0);
    private static readonly int2 down = new int2(0, 1);
    private static int2 tempCellLoc = new int2(0, 0);
    private int2 cellLocation;

    private int cellsPerFrame = (256*256)/60;
    public CellData currentCellData;
    public bool dataGenerated = false;
    
    private int iterationCount = 0;
    private GameObject iterator;
    private int iteratorX, iteratorY;
    public IEnumerator myCoroutine;
    private bool run = true;
    private int updateIndex;
    private CellData validCellData;

    public class CellCoord
    {
        public int2 coord;

        public CellCoord(int x, int y)
        {
            coord.x = x;
            coord.y = y;
        }

        public CellCoord(Vector3 position)
        {
            coord.x = 128 + (int)Mathf.Floor((position.x + 2.5f) / 5f);
            coord.y = 128 + (int)Mathf.Floor((position.z + 2.5f) / 5f);
        }

        public int key()
        {
            return coord.x + (coord.y * 256);
        }
    }

    public static int CellKey(Vector3 position)
    {
        int cx = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
        int cy = 128 + (int) Mathf.Floor((position.z + 2.5f)/5f);
        return cx + (cy*256);
    }

    public CellData GetCell(int2 ipos)
    {
        return cellsContents[ipos.x + (ipos.y*256)];
    }

    public static CellData GetCell(Vector3 pos)
    {
        return cellsContents[CellKey(pos)];
    }

    public static void SetCell(int x, int y, CellData val)
    {
        cellsContents[x + (y*256)] = val;
        cellsContents.SetValue(val, (x+(y*256)));
    }

    private static int ObjectKey(Vector3 position)
    {
        int cx = 256 + (int) Mathf.Floor((position.x + 1.25f)/2.5f);
        int cy = 256 + (int) Mathf.Floor((position.z + 1.25f)/2.5f);
        return cx + (cy*512);
    }

    public static void SetConnections(int x, int y, CellData val)
    {
        x = x*2;
        y = y*2;
        for (int directionx = 0; directionx <= 1; directionx++)
        {
            for (int directiony = 0; directiony <= 1; directiony++)
            {
                if (directionx == 0 && directiony == 0)
                {
                    continue;
                }
                if ((x + directionx) + ((y + directiony)*512) > ObjectDataSize)
                {
                    if (walls[(x + directionx) + ((y + directiony)*512)].exists)
                    {
                    }
                    if (doors[(x + directionx) + ((y + directiony)*512)].exists)
                    {
                    }
                    if (powerCords[(x + directionx) + ((y + directiony)*512)].exists)
                    {
                    }
                }
            }
        }
    }

    private void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
        }
        else
        {
        }
    }

    // Use this for initialization
    public void Start()
    {
        iterator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        iterator.name = "iterator";
        iterator.renderer.material.color = Color.red;
        iterator.collider.enabled = false;
    }

    public static void GenerateObjectData()
    {
        var sceneObjects = (GameObject[]) FindSceneObjectsOfType(typeof (GameObject));
        int2 coords = new int2(-128,-128);
        Vector3 location;
        for (int i = 0; i < ObjectDataSize; i++)
        {
            WallData _wall = new WallData();
            DoorData _door = new DoorData();
            PowerData _power = new PowerData();
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
            walls[i] = _wall;
            walls[i].location = location;
            doors[i] = _door;
            doors[i].location = location;
            powerCords[i] = _power;
        }
        coords = new int2(-128,-128);
        for (var i = 0; i < CellDataSize; i++)
        {
            FloorData _floor = new FloorData();
            location = new Vector3(coords.x, 0, coords.y);
            if (i % 256 == 0)
            {
                coords.y++;
                coords.x = 0;
            }
            else
            {
                coords.x++;
            }
            floors[i] = _floor;
            floors[i].location = location;
        }

        for (int index = 0; index < sceneObjects.Length; index++)
        {
            if (sceneObjects[index].name.Contains("Wall"))
            {
                if (sceneObjects[index].name.Contains("Wall Corner"))
                {
                    walls[ObjectKey(sceneObjects[index].transform.position)].exists = true;
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(-1, 0, -1);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(-1, 0, 1);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(1, 0, 1);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
                    }
                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0))
                    {
                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(1, 0, -1);
                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
                            sceneObjects[index].transform.position;
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
            else if (sceneObjects[index].name.Contains("Wire"))
            {
                powerCords[ObjectKey(sceneObjects[index].transform.position)].exists = true;
                if (sceneObjects[index].name.Contains("Wire Elbow"))
                {
                }
                if (sceneObjects[index].name.Contains("Wire T"))
                {
                }
                if (sceneObjects[index].name.Contains("Wire Quad"))
                {
                }
                if (sceneObjects[index].name.Contains("Wire Straight") && !sceneObjects[index].name.Contains("Half"))
                {
                }
                if (sceneObjects[index].name.Contains("Wire Straight Half"))
                {
                }
            }
            else if (sceneObjects[index].name.Contains("Floor Object"))
            {
                try
                {
                    floors[CellKey(sceneObjects[index].transform.position)].exists = true;
                    floors[CellKey(sceneObjects[index].transform.position)].location = sceneObjects[index].transform.position;
                    cellsContents[CellKey(sceneObjects[index].transform.position)].oxy = 0.2f;
                }
                catch
                {
                }
            }
        }
    }

    public static void FindFullAtmosphereSpaces()
    {
        int connections = 0;
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                if (connections == 4)
                {
                    connections = 0;
                }

                if (floors[(x + 0) + ((y + 0)*256)].exists)
                {
                    try
                    {
                        if (walls[(((x*2) + 1) + (((y*2) + 0)*256))].exists)
                        {
                            if (walls[(((x*2) + 1) + (((y*2) + 0)*256))].direction.y != 0)
                            {
                                connections++;
                            }
                        }
                        else if (floors[(x + 1) + ((y + 0)*256)].exists)
                        {
                            connections++;
                            floors[(x + 0) + ((y + 0)*256)].connections[0] = true;
                        }
                        if (connections == 4)
                        {
                            continue;
                        }

                        if (walls[(((x*2) + 0) + (((y*2) + 1)*256))].exists)
                        {
                            if (walls[(((x*2) + 0) + (((y*2) + 1)*256))].direction.x != 0)
                            {
                                connections++;
                            }
                        }
                        else if (floors[(x + 0) + ((y + 1)*256)].exists)
                        {
                            connections++;
                            floors[(x + 0) + ((y + 0)*256)].connections[1] = true;
                        }
                        if (connections == 4)
                        {
                            continue;
                        }

                        if (walls[(((x*2) - 1) + (((y*2) + 0)*256))].exists)
                        {
                            if (walls[(((x*2) - 1) + (((y*2) + 0)*256))].direction.y != 0)
                            {
                                connections++;
                            }
                        }
                        else if (floors[(x - 1) + ((y + 0)*256)].exists)
                        {
                            connections++;
                            floors[(x + 0) + ((y + 0)*256)].connections[2] = true;
                        }
                        if (connections == 4)
                        {
                            continue;
                        }

                        if (walls[(((x*2) + 0) + (((y*2) - 1)*256))].exists)
                        {
                            if (walls[(((x*2) + 0) + (((y*2) - 1)*256))].direction.x != 0)
                            {
                                connections++;
                            }
                        }
                        else if (floors[(x + 0) + ((y - 1)*256)].exists)
                        {
                            connections++;
                            floors[(x + 0) + ((y + 0)*256)].connections[3] = true;
                        }
                        if (connections == 4)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                    }
                }
                else if (floors[(x + 0) + ((y + 0)*256)].exists == false)
                {
                    cellsContents[(x + 0) + ((y + 0)*256)].oxy = 0.0f;
                }
            }
        }
    }
    private int i;
    public void Update()
    {
        if (dataGenerated)
        {
            for (int t = 0; t < cellsPerFrame; t++)
            {
                int x = (updateIndex%254) + 1;
                int y = ((updateIndex/254)%254) + 1;

                iterator.transform.position = new Vector3(x, 0, y);
                Propagate(new CellCoord(x, y));
                i++;
                updateIndex++;
                if (i == (256*256) && run)
                {
                    // print(i);
                    //print(Time.time);
                    run = false;
                }

                if (updateIndex >= (254*254)) updateIndex = 0;
            }
        }
    }


    public void Init()
    {
    }

    public void CreateCellAtLocation(int2 cellLocation)
    {
    }

    public void DestroyCellAtLocation(int2 cellLocation)
    {
        SetCell(cellLocation.x, cellLocation.y, null);
    }

    public void GetCellContents(int2 cellLocation)
    {
    }

    public void Propagate(CellCoord cellLocation)
        //Gets Cell Neighbors and compares all the data to the Data at this location
    {
        currentCellData = GetCell(cellLocation.coord);
        tempNorth = GetCell(cellLocation.coord - down);
        tempSouth = GetCell(cellLocation.coord + down);
        tempEast = GetCell(cellLocation.coord + right);
        tempWest = GetCell(cellLocation.coord - right);
        /*
        if(currentCellData.kPh)
        {
            
        }
        
        if(currentCellData.n02)
        {
            
        }
        */
        try
        {
            if (floors[ObjectKey(currentCellData.location)] != null &&
                floors[ObjectKey(currentCellData.location)].exists == false)
            {
                currentCellData.oxy = 0.0f;
            }
        }
        catch
        {
        }
        if (currentCellData.oxy != tempEast.oxy ||
            currentCellData.oxy != tempNorth.oxy ||
            currentCellData.oxy != tempWest.oxy ||
            currentCellData.oxy != tempSouth.oxy)
        {
            int validCells = 0;
            if ((walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction != new Vector3(0, 0, 0)))
                //east wall invalid
            {
                validCells++;
            }
            else if ((walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction ==
                      new Vector3(0, 0, 0))) //east wall valid
            {
                tempEast.transmissive = false;
            }

            if ((walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction != new Vector3(0, 0, 0)))
                //west wall invalid
            {
                validCells++;
            }
            else if ((walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction ==
                      new Vector3(0, 0, 0))) //west wall valid
            {
                tempWest.transmissive = false;
            }

            if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction != new Vector3(0, 0, 0)))
                //north wall invalid
            {
                validCells++;
            }
            else if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction ==
                      new Vector3(0, 0, 0))) //north wall valid
            {
                tempNorth.transmissive = false;
            }

            if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, -2.5f))].direction != new Vector3(0, 0, 0)))
                //south wall invalid
            {
                validCells++;
            }
            else if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction ==
                      new Vector3(0, 0, 0))) //south wall valid
            {
                tempSouth.transmissive = false;
            }


            if (tempEast.transmissive)
            {
                if ((floors[CellKey(tempEast.location)].exists))
                {
                }
                else if (!(floors[CellKey(tempEast.location)].exists))
                {
                    tempEast.oxy = 0.0f;
                }
            }
            if (tempNorth.transmissive)
            {
                if ((floors[CellKey(tempNorth.location)].exists))
                {
                }
                else if (!(floors[CellKey(tempNorth.location)].exists))
                {
                    tempNorth.oxy = 0.0f;
                }
            }
            if (tempWest.transmissive)
            {
                if ((floors[CellKey(tempWest.location)].exists))
                {
                }
                else if (!(floors[CellKey(tempWest.location)].exists))
                {
                    tempWest.oxy = 0.0f;
                }
            }
            if (tempSouth.transmissive)
            {
                if ((floors[CellKey(tempSouth.location)].exists))
                {
                }
                else if (!(floors[CellKey(tempSouth.location)].exists))
                {
                    tempEast.oxy = 0.0f;
                }
            }
            //currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy) / validCells;
            //tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy) / validCells;
            //tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy) / validCells;
            //tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy) / validCells;
            //tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy) / validCells;
        }

        /*
        if(currentCellData.plasma)
        {
            
        }
        if(currentCellData.rad)
        {
            
        }
        if(currentCellData.temp)
        {
            
        }
        */
    }

    
}