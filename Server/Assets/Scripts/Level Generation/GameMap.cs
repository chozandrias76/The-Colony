#region License

// // GameMap.cs
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

using System.Collections;
using UnityEngine;
using BitStream = uLink.BitStream;
using MonoBehaviour = uLink.MonoBehaviour;
using NetworkMessageInfo = uLink.NetworkMessageInfo;

public class GameMap : MonoBehaviour
{
    //private const int TILE_SIZE = 5; //Unity Units = 3ds/100, Tile = 500x500 3ds units
    private const int CellDataSize = 256 + (256*256);
    private const int ObjectDataSize = 512 + (512*512);
	
//    public static CellData[] cellsContents = new CellData[CellDataSize];
//    public static FloorData[] floors = new FloorData[CellDataSize];
//    public static WallData[] walls = new WallData[ObjectDataSize];
//    public static DoorData[] doors = new DoorData[ObjectDataSize];
//    public static PowerData[] powerCords = new PowerData[ObjectDataSize];
//
//    private static CellData tempWest;
//    private static CellData tempEast;
//    private static CellData tempSouth;
//    private static CellData tempNorth;
//
//    private static readonly int2 right = new int2(1, 0);
//    private static readonly int2 down = new int2(0, 1);

    private int cellsPerFrame = (256*256)/60;
    public CellData currentCellData;
    public bool dataGenerated = false;
    private int i;

    //private int iterationCount = 0;
    private GameObject iterator;
    //private int iteratorX, iteratorY;
    private bool run = true;
    private int updateIndex;
	
	public static GameMap Singleton;
	
	public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
	
//    public static int CellKey(Vector3 position)//Converts world coords to the dictonary key
//    {
//        int cx = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
//        int cy = 128 + (int) Mathf.Floor((position.z + 2.5f)/5f);
//        return cx + (cy*256);
//    }
//
//    public static CellData GetCell(int2 ipos)//Returns CellData using 2d int vector
//    {
//        return cellsContents[ipos.x + (ipos.y*256)];
//    }
//
//    public static CellData GetCell(Vector3 pos)//Returns CellData using 3d world vector
//    {
//        return cellsContents[CellKey(pos)];
//    }
//
//    public static WallData GetWall(Vector3 pos)//Returns Walldata at 3d world vector
//    {
//        return walls[ObjectKey(pos)];
//    }
//
//    public static FloorData GetFloor(Vector3 pos)//Returns FloorData at 3d world vector
//    {
//        return floors[CellKey(pos)];
//    }
//
//    public static void SetCell(int x, int y, CellData val)
//    {
//        cellsContents[x + (y*256)] = val;
//        cellsContents.SetValue(val, (x + (y*256)));
//    }
//
//    private static int ObjectKey(Vector3 position)//Returns CellKey for objects in different grid spacing
//    {
//        int cx = 256 + (int) Mathf.Floor((position.x + 1.25f)/2.5f);
//        int cy = 256 + (int) Mathf.Floor((position.z + 1.25f)/2.5f);
//        return cx + (cy*512);
//    }

//    public static void SetConnections(int x, int y, CellData val)//Creates neighbor data for valid cells around target cell
//    {
//        if (floors[ObjectKey(val.location)].exists)
//        {
//            for (int directionx = 0; directionx <= 1; directionx++)
//            {
//                for (int directiony = 0; directiony <= 1; directiony++)
//                {
//                    if (directionx == 0 && directiony == 0)
//                    {
//                    }
//                }
//            }
//        }
//
//        x = x*2;
//        y = y*2;
//        for (int directionx = 0; directionx <= 1; directionx++)
//        {
//            for (int directiony = 0; directiony <= 1; directiony++)
//            {
//                if (directionx == 0 && directiony == 0)
//                {
//                    continue;
//                }
//                if ((x + directionx) + ((y + directiony)*512) > ObjectDataSize)
//                {
//                    if (walls[(x + directionx) + ((y + directiony)*512)].exists)
//                    {
//                    }
//                    if (doors[(x + directionx) + ((y + directiony)*512)].exists)
//                    {
//                    }
//                    if (powerCords[(x + directionx) + ((y + directiony)*512)].exists)
//                    {
//                    }
//                }
//            }
//        }
//    }

    // Use this for initialization
    public void Start()
    {
        iterator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        iterator.name = "iterator";
        iterator.renderer.material.color = Color.red;
        iterator.collider.enabled = false;
    }

//    public static void GenerateObjectData()//Finds out where walls doors and power cables are
//    {
//        var sceneObjects = (GameObject[]) FindSceneObjectsOfType(typeof (GameObject));
//        var coords = new int2(-128, -128);
//        Vector3 location;
//        for (int i = 0; i < ObjectDataSize; i++)
//        {
//            var _wall = new WallData();
//            var _door = new DoorData();
//            var _power = new PowerData();
//            location = new Vector3(coords.x, 0, coords.y);
//            if (i%256 == 0)
//            {
//                coords.y++;
//                coords.x = 0;
//            }
//            else
//            {
//                coords.x++;
//            }
//            walls[i] = _wall;
//            walls[i].location = location;
//            doors[i] = _door;
//            doors[i].location = location;
//            powerCords[i] = _power;
//        }
//        coords = new int2(-128, -128);
//        for (int i = 0; i < CellDataSize; i++)
//        {
//            var _floor = new FloorData();
//            location = new Vector3(coords.x, 0, coords.y);
//            if (i%256 == 0)
//            {
//                coords.y++;
//                coords.x = 0;
//            }
//            else
//            {
//                coords.x++;
//            }
//            floors[i] = _floor;
//            floors[i].location = location;
//        }
//
//        for (int index = 0; index < sceneObjects.Length; index++)
//        {
//            if (sceneObjects[index].name.Contains("Wall"))
//            {
//                if (sceneObjects[index].name.Contains("Wall Corner"))
//                {
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].exists = true;
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].exists = true;
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].direction =
//                            new Vector3(0, 0, -1);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].direction =
//                            new Vector3(-1, 0, 0);
//
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f);
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].exists = true;
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].exists =
//                            true;
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].direction =
//                            new Vector3(0, 0, 1);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].direction =
//                            new Vector3(-1, 0, 0);
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0))].location =
//                            sceneObjects[index].transform.position + new Vector3(2.5f, 0, 0);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].location =
//                            sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f);
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].exists =
//                            true;
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].exists =
//                            true;
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].direction =
//                            new Vector3(0, 0, 1);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].direction =
//                            new Vector3(1, 0, 0);
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].location =
//                            sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f))].location =
//                            sceneObjects[index].transform.position + new Vector3(0, 0, -2.5f);
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].exists =
//                            true;
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].exists = true;
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].direction =
//                            new Vector3(0, 0, -1);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].direction =
//                            new Vector3(1, 0, 0);
//
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0))].location =
//                            sceneObjects[index].transform.position + new Vector3(-2.5f, 0, 0);
//                        walls[ObjectKey(sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f))].location =
//                            sceneObjects[index].transform.position + new Vector3(0, 0, 2.5f);
//                    }
//                }
//
//                else if (sceneObjects[index].name.Contains("Wall T"))
//                {
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(1, 0, 2);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position;
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, -1);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position;
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(-1, 0, 2);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position;
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, 1);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position;
//                    }
//                }
//
//                else if (sceneObjects[index].name.Contains("Wall") && !sceneObjects[index].name.Contains("Corner") &&
//                         !sceneObjects[index].name.Contains("Wall T") &&
//                         !sceneObjects[index].name.Contains("Wall Section"))
//                {
//                    walls[ObjectKey(sceneObjects[index].transform.position)].exists = true;
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0) ||
//                        sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, 0);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position;
//                    }
//                    if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0) ||
//                        sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
//                    {
//                        walls[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(0, 0, 2);
//                        walls[ObjectKey(sceneObjects[index].transform.position)].location =
//                            sceneObjects[index].transform.position;
//                    }
//                }
//            }
//
//            else if (sceneObjects[index].name.Contains("Door"))
//            {
//                if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 90, 0) ||
//                    sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 270, 0))
//                {
//                    doors[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(0, 0, 2);
//                    doors[ObjectKey(sceneObjects[index].transform.position)].location =
//                        sceneObjects[index].transform.position;
//                }
//                if (sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 0, 0) ||
//                    sceneObjects[index].transform.rotation.eulerAngles == new Vector3(0, 180, 0))
//                {
//                    doors[ObjectKey(sceneObjects[index].transform.position)].direction = new Vector3(2, 0, 0);
//                    doors[ObjectKey(sceneObjects[index].transform.position)].location =
//                        sceneObjects[index].transform.position;
//                }
//            }
//
//            else if (sceneObjects[index].name.Contains("Wire"))
//            {
//                powerCords[ObjectKey(sceneObjects[index].transform.position)].exists = true;
//                if (sceneObjects[index].name.Contains("Wire Elbow"))
//                {
//                }
//                if (sceneObjects[index].name.Contains("Wire T"))
//                {
//                }
//                if (sceneObjects[index].name.Contains("Wire Quad"))
//                {
//                }
//                if (sceneObjects[index].name.Contains("Wire Straight") && !sceneObjects[index].name.Contains("Half"))
//                {
//                }
//                if (sceneObjects[index].name.Contains("Wire Straight Half"))
//                {
//                }
//            }
//
//            else if (sceneObjects[index].name.Contains("Floor Object"))
//            {
//                try
//                {
//                    floors[CellKey(sceneObjects[index].transform.position)].exists = true;
//                    floors[CellKey(sceneObjects[index].transform.position)].location =
//                        sceneObjects[index].transform.position;
//                    cellsContents[CellKey(sceneObjects[index].transform.position)].oxy = 0.2f;
//                }
//                catch
//                {
//                }
//            }
//        }
//    }

//    public static void FindFullAtmosphereSpaces()
//    {
//        for (int x = 0; x < 256; x++)
//        {
//            for (int y = 0; y < 256; y++)
//            {
//                if (floors[(x + 0) + ((y + 0)*256)].exists)
//                {
//                    cellsContents[(x + 0) + ((y + 0)*256)].oxy = 0.2f;
//                }
//                else if (floors[(x + 0) + ((y + 0)*256)].exists == false)
//                {
//                    cellsContents[(x + 0) + ((y + 0)*256)].oxy = 0.0f;
//                }
//            }
//        }
//    }

    public void Update()
    {
        if (dataGenerated)
        {
            for (int t = 0; t < cellsPerFrame; t++)
            {
                int x = (updateIndex%254) + 1;
                int y = ((updateIndex/254)%254) + 1;

                iterator.transform.position = new Vector3(x, 0, y);
                //Propagate(new CellCoord(x, y));
				CellDataTracker.Singleton.PropagateAtmosphere(new CellDataTracker.CellCoord(x,y));
				ObjectDataTracker.Singleton.PropigatePower(new ObjectDataTracker.ObjectCoord(x,y));
				
                i++;
                updateIndex++;
                if (i == (256*256) && run)
                {
                    //print(i);
                    //print(Time.time);
                    run = false;
                }

                if (updateIndex >= (254*254)) updateIndex = 0;
            }
        }
    }

//    public void DestroyCellAtLocation(int2 cellLocation)
//    {
//        SetCell(cellLocation.x, cellLocation.y, null);
//    }
//
//    public void Propagate(CellCoord cellLocation)
//        //Gets Cell Neighbors and compares all the data to the Data at this location
//    {
//        currentCellData = GetCell(cellLocation.coord);
//        tempNorth = GetCell(cellLocation.coord - down);
//        tempSouth = GetCell(cellLocation.coord + down);
//        tempEast = GetCell(cellLocation.coord + right);
//        tempWest = GetCell(cellLocation.coord - right);
//        try
//        {
//            if (floors[ObjectKey(currentCellData.location)] != null &&
//                floors[ObjectKey(currentCellData.location)].exists == false)
//            {
//                currentCellData.oxy = 0.0f;
//                currentCellData.transmissive = false;
//            }
//        }
//        catch
//        {
//        }
//
//        if (currentCellData.oxy != tempEast.oxy ||
//            currentCellData.oxy != tempNorth.oxy ||
//            currentCellData.oxy != tempWest.oxy ||
//            currentCellData.oxy != tempSouth.oxy)
//        {
//            int validCells = 0;
//            if ((walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction !=
//                 new Vector3(0, 0, 0)))
//                //east wall invalid
//            {
//                validCells++;
//            }
//
//            else if ((walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction ==
//                      new Vector3(0, 0, 0))) //east wall valid
//            {
//                //tempEast.transmissive = false;
//                cellsContents[CellKey(currentCellData.location)].connections[0].isWall = true;
//            }
//
//            if ((walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction !=
//                 new Vector3(0, 0, 0)))
//                //west wall invalid
//            {
//                validCells++;
//            }
//            else if ((walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction ==
//                      new Vector3(0, 0, 0))) //west wall valid
//            {
//                //tempWest.transmissive = false;
//                cellsContents[CellKey(currentCellData.location)].connections[1].isWall = true;
//            }
//
//            if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction !=
//                 new Vector3(0, 0, 0)))
//                //north wall invalid
//            {
//                validCells++;
//            }
//            else if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction ==
//                      new Vector3(0, 0, 0))) //north wall valid
//            {
//                //tempNorth.transmissive = false;
//                cellsContents[CellKey(currentCellData.location)].connections[2].isWall = true;
//            }
//
//            if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, -2.5f))].direction !=
//                 new Vector3(0, 0, 0)))
//                //south wall invalid
//            {
//                validCells++;
//            }
//            else if ((walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction ==
//                      new Vector3(0, 0, 0))) //south wall valid
//            {
//                //tempSouth.transmissive = false;
//                cellsContents[CellKey(currentCellData.location)].connections[3].isWall = true;
//            }
//
//
//            if (tempEast.transmissive)
//            {
//                if ((floors[CellKey(tempEast.location)].exists))
//                {
//                }
//                else if (!(floors[CellKey(tempEast.location)].exists))
//                {
//                    tempEast.oxy = 0.0f;
//                    //tempEast.transmissive = false;
//                }
//            }
//            if (tempWest.transmissive)
//            {
//                if ((floors[CellKey(tempWest.location)].exists))
//                {
//                }
//                else if (!(floors[CellKey(tempWest.location)].exists))
//                {
//                    tempWest.oxy = 0.0f;
//                    //tempWest.transmissive = false;
//                }
//            }
//            if (tempNorth.transmissive)
//            {
//                if ((floors[CellKey(tempNorth.location)].exists))
//                {
//                }
//                else if (!(floors[CellKey(tempNorth.location)].exists))
//                {
//                    tempNorth.oxy = 0.0f;
//                    //tempNorth.transmissive = false;
//                }
//            }
//            if (tempSouth.transmissive)
//            {
//                if ((floors[CellKey(tempSouth.location)].exists))
//                {
//                }
//                else if (!(floors[CellKey(tempSouth.location)].exists))
//                {
//                    tempSouth.oxy = 0.0f;
//                    //tempSouth.transmissive = false;
//                }
//            }
//
//            byte direction = 0;
//            if (cellsContents[CellKey(currentCellData.location)].connections[0].isWall == false)
//            {
//                direction += 1;
//            }
//            else if (cellsContents[CellKey(currentCellData.location)].connections[0].isWall)
//            {
//                if (walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction !=
//                    new Vector3(0, 0, 1) ||
//                    walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction !=
//                    new Vector3(0, 0, -1))
//                {
//                    direction += 1;
//                }
//            }
//            if (cellsContents[CellKey(currentCellData.location)].connections[2].isWall == false)
//            {
//                direction += 2;
//            }
//            else if (cellsContents[CellKey(currentCellData.location)].connections[2].isWall)
//            {
//                if (walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction !=
//                    new Vector3(1, 0, 0) ||
//                    walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction !=
//                    new Vector3(-1, 0, 0))
//                {
//                    direction += 2;
//                }
//            }
//            if (cellsContents[CellKey(currentCellData.location)].connections[1].isWall == false)
//            {
//                direction += 4;
//            }
//            else if (cellsContents[CellKey(currentCellData.location)].connections[1].isWall)
//            {
//                if (walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction !=
//                    new Vector3(0, 0, 1) ||
//                    walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction !=
//                    new Vector3(0, 0, 1))
//                {
//                    direction += 4;
//                }
//            }
//            if (cellsContents[CellKey(currentCellData.location)].connections[3].isWall == false)
//            {
//                direction += 8;
//            }
//            else if (cellsContents[CellKey(currentCellData.location)].connections[3].isWall)
//            {
//                if (walls[ObjectKey(currentCellData.location + new Vector3(0, 0, -2.5f))].direction !=
//                    new Vector3(1, 0, 0) ||
//                    walls[ObjectKey(currentCellData.location + new Vector3(0, 0, -2.5f))].direction !=
//                    new Vector3(-1, 0, 0))
//                {
//                    direction += 8;
//                }
//            }
//            switch (direction)
//            {
//                case 0: //None
//                {
//                    currentCellData.oxy = 0.0f;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 1: //E
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy)/2;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy)/2;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 2: //N
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempNorth.oxy)/2;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = (currentCellData.oxy + tempNorth.oxy)/2;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 3: //NE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/3;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/3;
//                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/3;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 4: //W
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempWest.oxy)/2;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = (currentCellData.oxy + tempWest.oxy)/2;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 5: //WE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy)/3;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy)/3;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy)/3;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 6: //WN
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy)/3;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy)/3;
//                    tempWest.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy)/3;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 7: //WNE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/4;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/4;
//                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/4;
//                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/4;
//                    tempSouth.oxy = 0.0f;
//                }
//                    break;
//                case 8: //S
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempSouth.oxy)/2;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = (currentCellData.oxy + tempSouth.oxy)/2;
//                }
//                    break;
//                case 9: //SE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempSouth.oxy)/3;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempSouth.oxy)/3;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempSouth.oxy)/3;
//                }
//                    break;
//                case 10: //SN
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/3;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/3;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/3;
//                }
//                    break;
//                case 11: //SNE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/4;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/4;
//                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/4;
//                    tempWest.oxy = 0.0f;
//                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/4;
//                }
//                    break;
//                case 12: //SW
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempWest.oxy + tempSouth.oxy)/3;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = (currentCellData.oxy + tempWest.oxy + tempSouth.oxy)/3;
//                    tempSouth.oxy = (currentCellData.oxy + tempWest.oxy + tempSouth.oxy)/3;
//                }
//                    break;
//                case 13: //SWE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                    tempNorth.oxy = 0.0f;
//                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                }
//                    break;
//                case 14: //SWN
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                    tempEast.oxy = 0.0f;
//                    tempNorth.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                    tempWest.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                    tempSouth.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/4;
//                }
//                    break;
//                case 15: //SWNE
//                {
//                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
//                                           tempSouth.oxy)/5;
//                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
//                                    tempSouth.oxy)/5;
//                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
//                                     tempSouth.oxy)/5;
//                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
//                                    tempSouth.oxy)/5;
//                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
//                                     tempSouth.oxy)/5;
//                }
//                    break;
//            }
//        }
//
//
//        /*
//        if(currentCellData.plasma)
//        {
//            
//        }
//        if(currentCellData.rad)
//        {
//            
//        }
//        if(currentCellData.temp)
//        {
//            
//        }
//        */
//    }

//    public class CellCoord
//    {
//        public int2 coord;
//
//        public CellCoord(int x, int y)
//        {
//            coord.x = x;
//            coord.y = y;
//        }
//
//        public CellCoord(Vector3 position)
//        {
//            coord.x = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
//            coord.y = 128 + (int) Mathf.Floor((position.z + 2.5f)/5f);
//        }
//
//        public int key()
//        {
//            return coord.x + (coord.y*256);
//        }
//    }
}