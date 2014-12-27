#region License

// // CreatePowerGrid.cs
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
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class CreatePowerGrid : MonoBehaviour
{
	private static Vector3 zeroRot;
	private static Vector3 halfPIRot;
	private static Vector3 PIRot;
	private static Vector3 threeHalfPIRot;
	private static List<GameObject> wireList;
	//private GameMap currGameMap;
	private GameObject[] goInScene;
	public BitArray directionVectors = new BitArray (4, false);
	// Use this for initialization
	private void Start ()
	{
		zeroRot = new Vector3 (0, 0, 0);
		halfPIRot = new Vector3 (0, 90, 0);
		PIRot = new Vector3 (0, 180, 0);
		threeHalfPIRot = new Vector3 (0, 270, 0);
		wireList = new List<GameObject> ();
		//goInScene = new GameObject[10000];
		//currGameMap = new GameMap ();
		
		goInScene = FindObjectsOfType (typeof(GameObject)) as GameObject[];
		
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

	// Update is called once per frame
	private void Update ()
	{
	}
}