#region License

// // LoadMap.cs
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
using NetworkPlayer = uLink.NetworkPlayer;

public class LoadMap : MonoBehaviour
{
	private const int TILE_SIZE = 5;
	//private CellData initGameMap = new CellData ();
	//private bool loop = true;

	// Use this for initialization
	private void Start ()
	{
		PopulateWorldObjects.SetData ();//Add assets to world saved in map file
		CellDataTracker.Singleton.GenerateCellData ();//Create cells for atmospheric calculations
		//GameMap.GenerateObjectData();//Fill cells with appropreate data
		ObjectDataTracker.Singleton.GenerateObjectData ();
		CellDataTracker.Singleton.FindFullAtmosphereSpaces ();//Start filling and removing atmosphere appropreately
		ObjectDataTracker.Singleton.CreatePowerGrid();//Find where wires are and set power connections for those grid spaces
		GameMap.Singleton.dataGenerated = true;//Trigger update function
		//GameMap.FindFullAtmosphereSpaces();//Start filling and removing atmosphere appropreately
		//gameObject.AddComponent ("ConsoleScreen");//Create debug console in game
	}

//	private const int CellDataSize = 256 + (256 * 256);
	
//	public void GenerateCellData ()
//	{
//		CellDataTracker.Singleton.cellsContents = new CellData[CellDataSize];
//		for (int x = 0; x < 256; x++) {
//			for (int y = 0; y < 256; y++) {
//				initGameMap = new CellData ();
//				initGameMap.location = new Vector3 (x, 0, y);
//				CellDataTracker.Singleton.SetCell (x, y, initGameMap);
//				//GameMap.SetCell(x, y, new initGameMap());
//				CellDataTracker.Singleton.SetConnections (x, y, initGameMap);
//				//GameMap.SetConnections(x, y, new initGameMap());
//				
//			}
//		}
//	}

}