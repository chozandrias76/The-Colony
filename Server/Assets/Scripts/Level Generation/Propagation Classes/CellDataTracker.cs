using System;
using UnityEngine;
using System.Collections;

public class CellDataTracker : MonoBehaviour
{
	public static CellDataTracker Singleton;

	private const int ObjectDataSize = 512 + (512*512);
    public void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
	public CellData[] cellsContents;
	
	public int CellKey(Vector3 position)//Converts world coords to the dictonary key
    {
        int cx = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
        int cy = 128 + (int) Mathf.Floor((position.z + 2.5f)/5f);
        return cx + (cy*256);
    }
	
	public int CellKey(int2 position)//Converts world coords to the dictonary key
    {
        int cx = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
        int cy = 128 + (int) Mathf.Floor((position.y + 2.5f)/5f);
        return cx + (cy*256);
    }
	
	public void SetCell(int x, int y, CellData val)
    {
        cellsContents[x + (y*256)] = val;
        cellsContents.SetValue(val, (x + (y*256)));
    }
	public CellData GetCell(int2 ipos)//Returns CellData using 2d int vector
    {
        return cellsContents[ipos.x + (ipos.y*256)];
    }

    public CellData GetCell(Vector3 pos)//Returns CellData using 3d world vector
    {
        return cellsContents[CellKey(pos)];
    }
	
	public int ObjectKey(Vector3 position)//Returns CellKey for objects in different grid spacing
    {
        int cx = 256 + (int) Mathf.Floor((position.x + 1.25f)/2.5f);
        int cy = 256 + (int) Mathf.Floor((position.z + 1.25f)/2.5f);
        return cx + (cy*512);
    }
	
	public int ObjectKey(int2 position)//Returns CellKey for objects in different grid spacing
    {
        int cx = 256 + (int) Mathf.Floor((position.x + 1.25f)/2.5f);
        int cy = 256 + (int) Mathf.Floor((position.y + 1.25f)/2.5f);
        return cx + (cy*512);
    }
	
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
            coord.x = 128 + (int) Mathf.Floor((position.x + 2.5f)/5f);
            coord.y = 128 + (int) Mathf.Floor((position.z + 2.5f)/5f);
        }

        public int key()
        {
            return coord.x + (coord.y*256);
        }
    }
	
	public const int CellDataSize = 256 + (256 * 256);
	public CellData initGameMap;
	
	public void GenerateCellData ()
	{
		cellsContents = new CellData[CellDataSize];
		for (int x = 0; x < 256; x++) {
			for (int y = 0; y < 256; y++) {
				initGameMap = new CellData ();
				initGameMap.location = new Vector3 (x, 0, y);
				SetCell (x, y, initGameMap);
			}
		}
	}
	
	public void FindFullAtmosphereSpaces()
    {
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                if (ObjectDataTracker.Singleton.floors[(x + 0) + ((y + 0)*256)].exists)
                {
                    cellsContents[(x + 0) + ((y + 0)*256)].oxy = 0.2f;
                }
                else if (ObjectDataTracker.Singleton.floors[(x + 0) + ((y + 0)*256)].exists == false)
                {
                    cellsContents[(x + 0) + ((y + 0)*256)].oxy = 0.0f;
                }
            }
        }
    }
	
	private readonly int2 right = new int2(1, 0);
    private readonly int2 down = new int2(0, 1);	
	
	public void PropagateAtmosphere(CellCoord cellLocation)
        //Gets Cell Neighbors and compares all the data to the Data at this location
    {
        var currentCellData = GetCell(cellLocation.coord);
        var tempNorth = GetCell(cellLocation.coord - down);
        var tempSouth = GetCell(cellLocation.coord + down);
        var tempEast = GetCell(cellLocation.coord + right);
        var tempWest = GetCell(cellLocation.coord - right);
		BitArray validConnections = new BitArray(4,false);
        try
        {
            if (ObjectDataTracker.Singleton.floors[ObjectKey(currentCellData.location)] != null &&
                ObjectDataTracker.Singleton.floors[ObjectKey(currentCellData.location)].exists == false)
            {
                currentCellData.oxy = 0.0f;
                currentCellData.transmissive = false;
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
            if ((ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction !=
                 new Vector3(0, 0, 0)))
                //east wall invalid
            {
                //validCells++;
				validConnections.Set(0, true);
            }

            if ((ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction !=
                 new Vector3(0, 0, 0)))
                //west wall invalid
            {
                //validCells++;
				validConnections.Set(2, true);
            }
            
            if ((ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction !=
                 new Vector3(0, 0, 0)))
                //north wall invalid
            {
                //validCells++;
				validConnections.Set(1, true);
            }
            
            if ((ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(0, 0, -2.5f))].direction !=
                 new Vector3(0, 0, 0)))
                //south wall invalid
            {
				validConnections.Set(3, true);
                //validCells++;
            }
            
            if (tempEast.transmissive)
            {
                if ((ObjectDataTracker.Singleton.floors[CellKey(tempEast.location)].exists))
                {
                }
                else if (!(ObjectDataTracker.Singleton.floors[CellKey(tempEast.location)].exists))
                {
                    tempEast.oxy = 0.0f;
                    //tempEast.transmissive = false;
                }
            }
            if (tempWest.transmissive)
            {
                if ((ObjectDataTracker.Singleton.floors[CellKey(tempWest.location)].exists))
                {
                }
                else if (!(ObjectDataTracker.Singleton.floors[CellKey(tempWest.location)].exists))
                {
                    tempWest.oxy = 0.0f;
                    //tempWest.transmissive = false;
                }
            }
            if (tempNorth.transmissive)
            {
                if ((ObjectDataTracker.Singleton.floors[CellKey(tempNorth.location)].exists))
                {
                }
                else if (!(ObjectDataTracker.Singleton.floors[CellKey(tempNorth.location)].exists))
                {
                    tempNorth.oxy = 0.0f;
                    //tempNorth.transmissive = false;
                }
            }
            if (tempSouth.transmissive)
            {
                if ((ObjectDataTracker.Singleton.floors[CellKey(tempSouth.location)].exists))
                {
                }
                else if (!(ObjectDataTracker.Singleton.floors[CellKey(tempSouth.location)].exists))
                {
                    tempSouth.oxy = 0.0f;
                    //tempSouth.transmissive = false;
                }
            }

//            byte direction = 0;
//            if (cellsContents[CellKey(currentCellData.location)].connections[0].isWall == false)
//            {
//                direction += 1;
//            }
//            else if (cellsContents[CellKey(currentCellData.location)].connections[0].isWall)
//            {
//                if (ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(2.5f, 0, 0))].direction.z != 0)
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
//                if (ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(0, 0, 2.5f))].direction.x != 0)
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
//                if (ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(-2.5f, 0, 0))].direction.z != 0)
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
//                if (ObjectDataTracker.Singleton.walls[ObjectKey(currentCellData.location + new Vector3(0, 0, -2.5f))].direction.x != 0)
//                {
//                    direction += 8;
//                }
//            }
			
			int direction = Convert.ToInt32(validConnections);
			int directionCount = 1;
			for(int i = 0; i < validConnections.Count; i++)
			{
				if(validConnections[i] == true)
					directionCount++;
			}
            switch (direction)
            {
                case 0: //None
                {
                    currentCellData.oxy = 0.0f;
                    //tempEast.oxy = 0.0f;
                    //tempNorth.oxy = 0.0f;
                    //tempWest.oxy = 0.0f;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 1: //E
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy)/directionCount;
                    //tempNorth.oxy = 0.0f;
                    //tempWest.oxy = 0.0f;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 2: //N
                {
                    currentCellData.oxy = (currentCellData.oxy + tempNorth.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    tempNorth.oxy = (currentCellData.oxy + tempNorth.oxy)/directionCount;
                    //tempWest.oxy = 0.0f;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 3: //NE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/directionCount;
                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/directionCount;
                    //tempWest.oxy = 0.0f;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 4: //W
                {
                    currentCellData.oxy = (currentCellData.oxy + tempWest.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    //tempNorth.oxy = 0.0f;
                    tempWest.oxy = (currentCellData.oxy + tempWest.oxy)/directionCount;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 5: //WE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy)/directionCount;
                    //tempNorth.oxy = 0.0f;
                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy)/directionCount;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 6: //WN
                {
                    currentCellData.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    tempNorth.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    tempWest.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 7: //WNE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy)/directionCount;
                    //tempSouth.oxy = 0.0f;
                }
                    break;
                case 8: //S
                {
                    currentCellData.oxy = (currentCellData.oxy + tempSouth.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    //tempNorth.oxy = 0.0f;
                    //tempWest.oxy = 0.0f;
                    tempSouth.oxy = (currentCellData.oxy + tempSouth.oxy)/directionCount;
                }
                    break;
                case 9: //SE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempSouth.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempSouth.oxy)/directionCount;
                    //tempNorth.oxy = 0.0f;
                    //tempWest.oxy = 0.0f;
                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempSouth.oxy)/directionCount;
                }
                    break;
                case 10: //SN
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/directionCount;
                    //tempWest.oxy = 0.0f;
                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy)/directionCount;
                }
                    break;
                case 11: //SNE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/directionCount;
                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/directionCount;
                    //tempWest.oxy = 0.0f;
                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempSouth.oxy)/directionCount;
                }
                    break;
                case 12: //SW
                {
                    currentCellData.oxy = (currentCellData.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    //tempNorth.oxy = 0.0f;
                    tempWest.oxy = (currentCellData.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    tempSouth.oxy = (currentCellData.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                }
                    break;
                case 13: //SWE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    //tempNorth.oxy = 0.0f;
                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                }
                    break;
                case 14: //SWN
                {
                    currentCellData.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    //tempEast.oxy = 0.0f;
                    tempNorth.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    tempWest.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                    tempSouth.oxy = (currentCellData.oxy + tempNorth.oxy + tempWest.oxy + tempSouth.oxy)/directionCount;
                }
                    break;
                case 15: //SWNE
                {
                    currentCellData.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
                                           tempSouth.oxy)/directionCount;
                    tempEast.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
                                    tempSouth.oxy)/directionCount;
                    tempNorth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
                                     tempSouth.oxy)/directionCount;
                    tempWest.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
                                    tempSouth.oxy)/directionCount;
                    tempSouth.oxy = (currentCellData.oxy + tempEast.oxy + tempNorth.oxy + tempWest.oxy +
                                     tempSouth.oxy)/directionCount;
                }
                    break;
            }
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


