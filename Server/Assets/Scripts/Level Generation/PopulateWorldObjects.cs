#region License

// // PopulateWorldObjects.cs
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
using System.Collections.Generic;
using UnityEngine;
using NetworkPlayer = uLink.NetworkPlayer;
using NetworkView = uLink.NetworkView;

public class PopulateWorldObjects : MonoBehaviour
{
    private void Start()
    {
    }

    public void GetData()
    {
        var sceneGameObjects = new GameObject[10000000];
        sceneGameObjects = (GameObject[]) FindObjectsOfType(typeof (GameObject));
        Object[] actualPrefabsInDirectory = Resources.LoadAll("Prefabs");
        var fileEntries = new string[actualPrefabsInDirectory.Length];
        int index = 0;
        foreach (Object prefab in actualPrefabsInDirectory)
        {
            fileEntries[index] = prefab.name;
            index++;
        }
        var dataToSave = new List<GameObjectData>();
        bool save = false;
        foreach (GameObject sceneObject in sceneGameObjects)
        {
            if (sceneObject.name.Trim().Contains("("))
            {
                int firstLocation = sceneObject.name.IndexOf("(");
                if (firstLocation >= 0)
                    sceneObject.name = sceneObject.name.Substring(0, firstLocation);
            }
            foreach (string _fileName in fileEntries)
            {
                if (sceneObject.name.StartsWith(_fileName.Trim()))
                {
                    var _currentGameObjectData = new GameObjectData();
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
        if (save)
        {
            GameObjectData[] _dataToSave = dataToSave.ToArray();
            var newSaveData = new SaveData();
            newSaveData.gOD = _dataToSave;
            JSONGameMap.Save(newSaveData);
            Debug.Log("Saved");
        }
    }


    public static void SetData()
    {
        var newSaveData = new SaveData();
        newSaveData = JSONGameMap.Load();
        GameObjectData[] dataToLoad = newSaveData.gOD;
        Object[] actualPrefabsInDirectory = Resources.LoadAll("Prefabs");
        var fileEntries = new string[actualPrefabsInDirectory.Length];
        //var assetBundle = new AssetBundle();
        //var assetBundleGOsList = new ArrayList();
        int index = 0;
        foreach (Object prefab in actualPrefabsInDirectory)
        {
            fileEntries[index] = prefab.name;
            index++;
        }

        foreach (GameObjectData currentGameObjectData in dataToLoad)
        {
            if (currentGameObjectData != null)
            {
                for (int i = 0; i < fileEntries.Length; i++)
                {
                    string _filename = fileEntries[i];
                    if (currentGameObjectData.objectName.StartsWith(_filename) &&
                        currentGameObjectData.objectName.Length == _filename.Length)
                    {
                        GameObject current;
                        var currentLocation = new Vector3(currentGameObjectData.transformX,
                            currentGameObjectData.transformY + 110f, currentGameObjectData.transformZ);
                        var currentQuaternion = new Quaternion(
                            currentGameObjectData.rotationX,
                            currentGameObjectData.rotationY,
                            currentGameObjectData.rotationZ,
                            currentGameObjectData.rotationW);
                        current = (GameObject) Instantiate(
                            Resources.Load("Prefabs/" + _filename),
                            currentLocation,
                            currentQuaternion);
                        current.name = _filename + currentLocation;
                    }
                }
            }
        }
    }


    public static void SetData(NetworkPlayer player)
    {
        var newSaveData = new SaveData();
        newSaveData = JSONGameMap.Load();
        GameObjectData[] dataToLoad = newSaveData.gOD;
        Object[] actualPrefabsInDirectory = Resources.LoadAll("Prefabs");
        var fileEntries = new string[actualPrefabsInDirectory.Length];
        int index = 0;
        foreach (Object prefab in actualPrefabsInDirectory)
        {
            fileEntries[index] = prefab.name;
            index++;
        }

        foreach (GameObjectData currentGameObjectData in dataToLoad)
        {
            if (currentGameObjectData != null)
            {
                for (int i = 0; i < fileEntries.Length; i++)
                {
                    string _filename = fileEntries[i];
                    if (currentGameObjectData.objectName.StartsWith(_filename) &&
                        currentGameObjectData.objectName.Length == _filename.Length)
                    {
                        NetworkView.Get(GameObject.Find("_gameLogic"))
                            .RPC("SetData", player, currentGameObjectData, _filename);
                    }
                }
            }
        }
    }

    public class GameObjectData
    {
        public string objectName;
        public float rotationW;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float transformX;
        public float transformY;
        public float transformZ;
    }

    //Save Data class
    public class SaveData
    {
        public GameObjectData[] gOD;
        //Needed for serialization
    }
}