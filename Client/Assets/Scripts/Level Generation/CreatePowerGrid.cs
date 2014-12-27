using UnityEngine;
using System.Collections;



public class CreatePowerGrid : MonoBehaviour {
    GameObject[] goInScene;
    GameMap currGameMap;
	
	static Vector3 zeroRot;
	static Vector3 halfPIRot;
	static Vector3 PIRot;
	static Vector3 threeHalfPIRot;
	static System.Collections.Generic.List<GameObject> wireList;
    // Use this for initialization
    void Start () {
		zeroRot = new Vector3(0,0,0);
		halfPIRot = new Vector3(0,90,0);
		PIRot = new Vector3(0,180,0);
		threeHalfPIRot = new Vector3(0,270,0);
		wireList = new System.Collections.Generic.List<GameObject>();
        goInScene = new GameObject[10000];
		currGameMap = new GameMap();
        goInScene = GameObject.FindObjectsOfType(typeof(GameObject) )as GameObject[];
        foreach(GameObject go in goInScene)
        {
            if(go.name.Contains("Wire Elbow") 
                || go.name.Contains("Wire Quad") 
                || (go.name.Contains("Wire Straight") & !go.name.Contains("Half")) 
                || go.name.Contains("Wire Straight Half") 
                || go.name.Contains("Wire T"))
            {
				wireList.Add(go);
               
            }
        }
		/*
		for(int i = 0; i < wireList.Count; i++)
		{
				
				CellData cell = currGameMap.getCell(wireList[i].transform.position);
                Debug.Log(cell.GetType());
                cell.contents.Add(wireList[i].gameObject.name);
                
                Vector3 angle = wireList[i].transform.rotation.eulerAngles;
			
                if(wireList[i].name.Contains("Wire Straight") && !wireList[i].name.Contains("Half"))
                {
					//Debug.Log("Straight");
                    
                    
                    if(angle == zeroRot)//N-S
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                    }   

                    if(angle == halfPIRot)//W-E
                    {
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }       
                        
                    if(angle == PIRot)//S-N
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                    }       
                        
                    if(angle == threeHalfPIRot)//E-W
                    {
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }       
                    
                }
                if(wireList[i].name.Contains("Wire Elbow"))
                {
					//Debug.Log("Elbow");
                    if(angle == zeroRot)//S-W
                    {
                        cell.powerCell.south = true;
                        cell.powerCell.west = true;
                    }
                        
                    if(angle == halfPIRot)//W-N
                    {
                        cell.powerCell.west = true;
                        cell.powerCell.north = true;
                    }   
                        
                    if(angle == PIRot)//N-E
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.east = true;
                    }   
                        
                    if(angle == threeHalfPIRot)//W-S
                    {
                        cell.powerCell.west = true;
                        cell.powerCell.south = true;
                    }   
                }
                if(wireList[i].name.Contains("Wire Straight Half"))
                {
					//Debug.Log("Half");
                    if(angle == zeroRot)//
                    {
                        
                    }       
                        
                    if(angle == halfPIRot)//
                    {
                        
                    }       
                        
                    if(angle == PIRot)//
                    {
                        
                    }       
                        
                    if(angle == threeHalfPIRot)//
                    {
                        
                    }       
                    
                }
                if(wireList[i].name.Contains("Wire T"))
                {
					//Debug.Log("T");
                    if(angle == zeroRot)//W-N-E
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }       
                        
                    if(angle == halfPIRot)//N-E-S
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                        cell.powerCell.east = true;
                    }       
                        
                    if(angle == PIRot)//E-S-W
                    {
                        cell.powerCell.south = true;
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }       
                        
                    if(angle == threeHalfPIRot)//S-W-N
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                        cell.powerCell.west = true;
                    }       
                    
                }
                if(wireList[i].name.Contains("Wire Quad"))
                {
					Debug.Log("Quad");
                    if(angle == zeroRot)//N-S-E-W
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }   
                        
                    if(angle == halfPIRot)//W-N-S-E
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }   
                        
                    if(angle == PIRot)//E-W-N-S
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }   
                        
                    if(angle == threeHalfPIRot)//S-E-W-N
                    {
                        cell.powerCell.north = true;
                        cell.powerCell.south = true;
                        cell.powerCell.east = true;
                        cell.powerCell.west = true;
                    }   
                    
                }
                
		}*/
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}