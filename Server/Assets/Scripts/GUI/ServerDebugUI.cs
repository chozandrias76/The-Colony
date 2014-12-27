#region License

// // ServerDebugUI.cs
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

using System;
using UnityEngine;

public class ServerDebugUI : MonoBehaviour
{
    private int buttonPress = 0;
    private float rayLimit = 250.0f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(buttonPress))
        {
            GetDataOnHit();
        }
    }

    private void GetDataOnHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLimit))
        {
			
            Debug.Log("Click Location " + hit.transform.position);
            Debug.Log(
                String.Format(
                    "Oxygen Value: {0}\n Cell Transmissive: {1}\n Wall Exists: {2}\n Floor Exists: {3}",
                    CellDataTracker.Singleton.GetCell(hit.transform.position).oxy,
                    CellDataTracker.Singleton.GetCell(hit.transform.position).transmissive,
                    ObjectDataTracker.Singleton.GetWall(hit.transform.position).exists,
                    ObjectDataTracker.Singleton.GetFloor(hit.transform.position).exists));
            Debug.Log(String.Format("East Wall:{0}\n" +
                                    " West Wall:{1}\n" +
                                    " North Wall:{2}\n" +
                                    " South Wall:{3}\n",
                CellDataTracker.Singleton.GetCell(hit.transform.position).connections[0].isWall,
                CellDataTracker.Singleton.GetCell(hit.transform.position).connections[1].isWall,
                CellDataTracker.Singleton.GetCell(hit.transform.position).connections[2].isWall,
                CellDataTracker.Singleton.GetCell(hit.transform.position).connections[3].isWall));
            foreach (WallData wall in ObjectDataTracker.Singleton.walls)
            {
                if (wall.location != new Vector3(0, 100000, 0))
                {
                    if (wall.exists)
                    {
                        //Debug.Log(wall.location);
                    }
                }
                ////
            }
        }
    }
}