using System;
using System.IO;
using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class JSONFiles : MonoBehaviour{
	public class GameObjectData
	{
		public GameObjectData(){}
		public string objectName;
		public float transformX;
		public float transformY;
		public float transformZ;
		public float rotationX;
		public float rotationY;
		public float rotationZ;
		public float rotationW;
		
	}
	//public string _filename;
	
	//Save Data class
	public class SaveData {
		public GameObjectData[] gOD;
		public SaveData(){}//Needed for serialization
	}
	
	void Start()
	{
		
	}
	public void GetData()
	{
		GameObject[] sceneGameObjects = new GameObject[10000000];
		sceneGameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
        UnityEngine.Object[] actualPrefabsInDirectory = Resources.LoadAll("Prefabs");
        string[] fileEntries = new string[actualPrefabsInDirectory.Length];
        int index = 0;
        foreach (UnityEngine.Object prefab in actualPrefabsInDirectory)
        {
            fileEntries[index] = prefab.name;
            index++;
        }
		//string[] fileEntries = Directory.GetFiles(Application.dataPath + "/Resources/Prefabs/");
		List<GameObjectData> dataToSave = new List<GameObjectData>();
		bool save = false;
		foreach(GameObject sceneObject in sceneGameObjects)
		{
			
			if(sceneObject.name.Trim().Contains("("))
			{
				int firstLocation = sceneObject.name.IndexOf("(");
				if(firstLocation >= 0)
					sceneObject.name = sceneObject.name.Substring(0,firstLocation);
			}
			foreach(string _fileName in fileEntries)
			{
				if(sceneObject.name.StartsWith(_fileName.Trim()))
				{
					GameObjectData _currentGameObjectData = new GameObjectData();
					_currentGameObjectData.transformX = sceneObject.transform.position.x;
					_currentGameObjectData.transformY = sceneObject.transform.position.y;
					_currentGameObjectData.transformZ = sceneObject.transform.position.z;
					_currentGameObjectData.rotationX = sceneObject.transform.rotation.x;
					_currentGameObjectData.rotationY = sceneObject.transform.rotation.y;
					_currentGameObjectData.rotationZ = sceneObject.transform.rotation.z;
					_currentGameObjectData.rotationW = sceneObject.transform.rotation.w;
                    _currentGameObjectData.objectName = _fileName;
					
					dataToSave.Add(_currentGameObjectData);
					save = true;
					Destroy(sceneObject);
				}
			}
		}
		if(save)
		{
			GameObjectData[] _dataToSave = dataToSave.ToArray();
			SaveData newSaveData = new SaveData();
			newSaveData.gOD = _dataToSave;
			JSONFiles.SavingInterface.Save(newSaveData);
			Debug.Log("Saved");
		}
	}
	public void SetData()
	{	
		
		SaveData newSaveData = new SaveData();
		newSaveData = JSONFiles.SavingInterface.Load();
		GameObjectData[] dataToLoad = newSaveData.gOD;
        UnityEngine.Object[] actualPrefabsInDirectory = Resources.LoadAll("Prefabs");
        string[] fileEntries = new string[actualPrefabsInDirectory.Length];
        int index = 0;
        foreach(UnityEngine.Object prefab in actualPrefabsInDirectory)
        {
            fileEntries[index] = prefab.name;
            index++;
        }
		
		foreach(GameObjectData currentGameObjectData in dataToLoad)
		{
			if(currentGameObjectData != null)
			{
				for(int i = 0; i < fileEntries.Length; i++)
				{
					string _filename = fileEntries[i];
					if(currentGameObjectData.objectName.StartsWith(_filename) && currentGameObjectData.objectName.Length == _filename.Length)
						{
						GameObject current;
						Vector3 currentLocation = new Vector3(currentGameObjectData.transformX, currentGameObjectData.transformY+110f, currentGameObjectData.transformZ);
						Quaternion currentQuaternion = new Quaternion(
                            currentGameObjectData.rotationX,
                            currentGameObjectData.rotationY,
                            currentGameObjectData.rotationZ,
                            currentGameObjectData.rotationW);
						current = (GameObject)Instantiate(
                            Resources.Load("Prefabs/" + _filename),
							currentLocation,
							currentQuaternion);
						current.name = _filename +
                            "(" +
                            (float)(currentGameObjectData.transformX/5) +
                            "," +
                            (float)(currentGameObjectData.transformZ/5) +
                            "," +
                            (float)(currentGameObjectData.transformY/5) +
                            ")";
						}
				}
			}
		}
	}

	
	public class SavingInterface {
		public static string saveDirectory = Application.dataPath;
		private static string filePath = Application.dataPath + "/WorldMap.lvl";
		public static void Save (SaveData data) {
			//If the directory does not yet exist, create it
			
			if(!Directory.Exists(saveDirectory)){
				Directory.CreateDirectory(saveDirectory);
			}
			//Write the serialized JSON string of the save data to the specified filePath
			File.WriteAllText(filePath, JsonWriter.Serialize(data));
		}
		public static SaveData Load() {
			//If the directory doesn't exist, we can't load the file...
			if(!Directory.Exists(saveDirectory)) return null;
			
			if(!File.Exists(filePath)) return null;
			//Return the contents of the save file, translated into SaveData
			return JsonReader.Deserialize<SaveData>(File.ReadAllText(filePath));
		
		}
	}
}

