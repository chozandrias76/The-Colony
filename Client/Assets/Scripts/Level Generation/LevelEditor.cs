using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.Text;




public class LevelEditor : MonoBehaviour 
	{
   

    public Vector3 target;
	
    private GameObject cursor;
    private GameObject good; 
    private GameObject bad;
	
	public float buttonWidth = 80;						//define the width of all of the buttons we will be using for gameobjects
	public float buttonHeight = 80;						//define the height of all of the buttons we wll be using for gameobjects
	public float closeButtonWidth = 20;					//the width of a close button (small x in a corner)
	public float closeButtonHeight = 20;				//the heght of a close button (small x in the corner)
	public Vector2 scrollPosition = Vector2.zero;					//the location of the scroll bar in the GUI window
	
	private Texture btnTexture;
	public int _editorRows = 16;
	public int _editorCols = 4;
	public float _editorLocX;
	public float _editorLocY;
	public float _editorScaleX;
	public float _editorScaleY;
	public Rect _editorWindowRect;
 	private bool _displayEditorWindow =false;
	private const int EDITOR_WINDOW_ID = 1;
	
	private string[] editorButtonItems = new string[64];
	private string selEditorButtonItem;
	
    private int fileNumber = 0;
	
	const int TILE_SIZE = 5;
	
	public string _data;
	
	Vector3 currentLoadPosition;
	
	void Start ()
	{
		//ProcessDirectory(Application.dataPath + "/Resources/Prefabs/");
      UnityEngine.Object[] prefabs = Resources.LoadAll("Prefabs");
      foreach (UnityEngine.Object prefab in prefabs)
      {
          editorButtonItems[fileNumber] += prefab.name;
          fileNumber++;
      }
	}
	
	 public void ProcessDirectory(string targetDirectory) 
    {
		
        // Process the list of files found in the directory. 
        Debug.Log(targetDirectory);
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        foreach(string fileName in fileEntries)
		{
            ProcessFile(fileName);
		}
    }

    // Insert logic for processing found files here. 
    public void ProcessFile(string path) 
    {
        path = path.Substring((Application.dataPath + "/Resources/Prefabs/").Length, path.Length - (Application.dataPath + "/Resources/Prefabs/").Length);
		path = path.Substring(0, path.Length - 7);
			editorButtonItems[fileNumber] += path;
			fileNumber++;
    }
	
	void OnGUI()
	{
		GUILayout.Label ("Press Q open the menu");
			if(_displayEditorWindow)
			{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(_editorScaleX+25), GUILayout.Height(_editorScaleY));
			_editorLocX = (Screen.width/16);
			_editorLocY = (Screen.height/16);
			_editorScaleX = (_editorCols*buttonWidth+10+200);
			for(int i = 0;i*buttonHeight+21 < Screen.height-Screen.height/8;i++)
			_editorScaleY = (i*buttonHeight+21);
			_editorWindowRect = new Rect(_editorLocX, _editorLocY, _editorScaleX, _editorScaleY);
			_editorWindowRect = GUI.Window (EDITOR_WINDOW_ID, _editorWindowRect, EditorWindow, "Editor");
			GUILayout.EndScrollView();
			}
	}
	
	public void EditorWindow(int id)
	{
		int cnt = 0;
		for(int y = 0; y < _editorRows; y++) 
		{
			for(int x = 0; x < _editorCols; x++) 
			{
				if(GUI.Button(new Rect(5 + (x * buttonWidth), 20 + (y * buttonHeight), buttonWidth, buttonHeight), editorButtonItems[cnt]))
				{
					selEditorButtonItem = editorButtonItems[cnt];
					
					Destroy (cursor);
                    //Debug.Log(Application.dataPath + "/Resources/Prefabs/" + selEditorButtonItem.ToString());
					cursor = (GameObject)Instantiate(Resources.Load("Prefabs/" + selEditorButtonItem),new Vector3(x, 0, y), Quaternion.identity);
					
					if(cursor.name.Contains("Tile")){cursor.transform.Rotate(new Vector3(270,0,0));}
					cursor.gameObject.name = "_editor_Cursor";
					Transform[] cursorChildren = cursor.GetComponentsInChildren<Transform>();
					foreach(Transform T in cursorChildren)
					{
						if(T != null && T.gameObject != null)
						{
							T.gameObject.layer = 2;
						}
					}
				}
				if(y == 0 && x == 0)
				{
					if(GUI.Button(new Rect (5 + (4 * buttonWidth), 20, buttonWidth, buttonHeight), "Save" + '\n' + "Map"))
					{
						//SaveGameMap();
                        GameObject.Find("_gameLogic").GetComponent<JSONFiles>().GetData();
					}
					if(GUI.Button(new Rect (5 + (5 * buttonWidth), 20, buttonWidth, buttonHeight), "Load \n Map"))
					{
						if(!GameObject.Find("_gameLogic").GetComponent("LoadMap"))
						{
						GameObject.Find("_gameLogic").AddComponent("LoadMap");
						}
						else
						{
							
						}
					}
					if(GUI.Button(new Rect (5 + (4 * buttonWidth), 20 + buttonHeight, buttonWidth, buttonHeight), "Initilize" + '\n' + "World"))
					{
					
					}
					if(GUI.Button(new Rect (5 + (5 * buttonWidth), 20 + buttonHeight, buttonWidth, buttonHeight), "Propagate" + '\n' + "Tool"))
					{
						//selEditorButtonItem = "Propagate";
						//Destroy(cursor);
					}
					
					
				}
				cnt++;
			}
		}
	}
	
	public void ToggleEditorWindow() 
	{
		if(_displayEditorWindow == false)
		{
			_displayEditorWindow = true;
		}
		else
		{
			_displayEditorWindow = false;
		}
	}

    void Update () 
    {
		float x;
    	float y;
		if(Input.GetKeyDown("q"))
		{
			ToggleEditorWindow();
		}
		 
        RaycastHit hit;
		float inf = Mathf.Infinity;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, inf, ~0))
        {
			if(!_displayEditorWindow)
			{
				float modX = 0;
				float modZ = 0;
				float hitX = hit.point.x;
				float hitZ = hit.point.z;
				//int hitX = (int)hit.point.x;
				//int hitY = (int)hit.point.z;
				if(cursor)
				{
					if(cursor.tag == "Grid1")
					{
					modX = hit.point.x % 5f;
					modZ = hit.point.z % 5f;
						if (modX < 2.5f)
							{
							hitX -= modX;
							}
							else
							{
							hitX +=5f-modX;
							}
						if (modZ < 2.5f)
							{
							hitZ -= modZ;
							}
							else
							{
							hitZ +=5f-modZ;
							}
							
					//hitX= (int)Mathf.Floor(hit.point.x*5f);
		  			//hitY= (int)Mathf.Floor(hit.point.z*5f);
					}
					else if(cursor.tag == "Grid2")
					{

                        modX = hit.point.x % 2.5f;
                        modZ = hit.point.z % 2.5f;
						if (modX < 1.25f)
							{
							hitX -= modX;
							}
							else
							{
                                hitX += 2.5f - modX;
							}
                        if (modZ < 1.25f)
							{
							hitZ -= modZ;
							}
							else
							{
                                hitZ += 2.5f - modZ;
							}
							
					//hitX= (int)Mathf.Floor(hit.point.x*5f);
		  			//hitY= (int)Mathf.Floor(hit.point.z*5f);	
					}
					else if(cursor.tag != "Grid2" || cursor.tag != "Grid1")
					{
					}
				}
				else if(selEditorButtonItem == null || cursor == null)
				{
					
				}
				/*
				if( selEditorButtonItem == "Propagate")
				{
				/*
					modX = hit.point.x % 5f;
					modZ = hit.point.z % 5f;
						if (modX < 2.5f)
							{
							hitX -= modX;
							}
							else
							{
							hitX +=5f-modX;
							}
						if (modZ < 2.5f)
							{
							hitZ -= modZ;
							}
							else
							{
							hitZ +=5f-modZ;
							}
						*
					hitX=(int)Mathf.Floor(hit.point.x/5f);
		  			hitY=(int)Mathf.Floor(hit.point.z/5f);
				}
				*/
				
				target.x = hitX;
				target.y = 0;
				target.z = hitZ;

	            x = target.x;
	            y = target.z;
	            
					//Left click places current selected asset
		            if (Input.GetMouseButtonDown(0) && selEditorButtonItem != null && selEditorButtonItem != "Propagate")
		            {
						AddGameObject(x, y);
						if (selEditorButtonItem.Contains("Plane"))
						{
						//GameObject.Find("_gameLogic").GetComponent<GameCell>().CreateCellAtLocation(new int2(x,y));
						}
		            }
					else if(Input.GetMouseButton(0) && selEditorButtonItem == "Propagate")
					{
						Debug.Log(hit.collider.gameObject.name);
						//GameObject.Find("_gameLogic").GetComponent<GameCell>().Propagate(hit);
					}
					//Right click rotates current selected asset
		            if (Input.GetMouseButtonDown(1) && selEditorButtonItem != "Plane" && selEditorButtonItem != null)
		
		            {
						RotateGameObject();
		            }
					//Middle click selects any number of assets, sets them green and marks them for deletion
		            if (Input.GetMouseButtonDown(2))
		            {	
						SelectGameObject(hit);
		            }
					//Delete button deletes ALL green highlighted assets
					if (Input.GetKeyDown(KeyCode.Delete))
					{
						DeleteGameObject();
					}
			}

        }
		if(cursor != null)
        cursor.transform.position = target;     
    }
	public void AddGameObject(float addX, float addY)
	{
		GameObject go = null;
		go = (GameObject)Instantiate(Resources.Load("Prefabs/" + selEditorButtonItem),new Vector3(addX, 0, addY), Quaternion.identity);
		go.name = selEditorButtonItem + "(" + (float)(go.transform.position.x/TILE_SIZE) + "," + (float)(go.transform.position.z/TILE_SIZE) + "," + (float)(go.transform.position.y/TILE_SIZE) + ")";
		if(selEditorButtonItem == "Plane")
			{
			}
				else
			{
				go.transform.rotation = Quaternion.Slerp(go.transform.localRotation, cursor.transform.localRotation, 1f);
				//GameObject.Find("_gameLogic").GetComponent<GameCell>().FindCellForGameObject(go);
			}
		foreach(GameObject goObject in FindObjectsOfType(typeof(GameObject)))
		{
			if(!goObject.GetComponent<MeshCollider>())
			goObject.AddComponent("MeshCollider");
		}
		
	}
	
	public void RotateGameObject()
	{
		cursor.transform.Rotate(new Vector3(0,90,0));
	}
	
	public void SelectGameObject(RaycastHit hitSelect)
	{
		int skip = ~((1<<8|1<<2));
		if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitSelect, 1000, skip))
		{
			if(hitSelect.collider.gameObject.transform.name != "_editor_Cursor")
			{
				GameObject goEdit;
				goEdit = hitSelect.transform.parent.gameObject;
				Transform[] goEditChildren = goEdit.GetComponentsInChildren<Transform>();
				if(goEdit.renderer.material.color == Color.green)
				{	
					foreach(Transform child in goEditChildren)
					{
						if(child != null && child.gameObject != null && child.gameObject.renderer)
						{
							
							child.gameObject.renderer.material.color = Color.white;
							child.gameObject.tag  = null;
							goEdit = null;
						}
					}
				}
				else
				{
					foreach(Transform child in goEditChildren)
					{
						if(child != null && child.gameObject != null && child.gameObject.renderer)
						{
							
							child.gameObject.renderer.material.color = Color.green;
							child.gameObject.tag = "Deletion";
							goEdit = null;
						}
					}
					
				}

			}
		}
	}
	
	public void DeleteGameObject()
	{
		GameObject[] deletions = null;
		if(deletions == null)
		deletions = GameObject.FindGameObjectsWithTag("Deletion");
		foreach (GameObject Deletion in deletions)
		{
			Destroy(Deletion);
			GameObject.Find("_gameLogic").GetComponent<GameMap>().DestroyCellAtLocation
				(new int2((int)Deletion.transform.position.x,(int)Deletion.transform.position.z));
		}
	}
	
}

