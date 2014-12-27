using UnityEngine;
using System.Collections;

public class PopulateWorldObjects : uLink.MonoBehaviour {
    public class GameObjectData
    {
        public GameObjectData() { }
        public string objectName;
        public float transformX;
        public float transformY;
        public float transformZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float rotationW;

    }

    [RPC]
	public void SetData(GameObjectData currentGameObjectData, string cFilename)
	{
        GameObject current = Resources.Load("Prefabs/" + cFilename) as GameObject;
        Vector3 currentLocation = new Vector3(currentGameObjectData.transformX, currentGameObjectData.transformY + 110f, currentGameObjectData.transformZ);
        Quaternion currentQuaternion = new Quaternion(
            currentGameObjectData.rotationX,
            currentGameObjectData.rotationY,
            currentGameObjectData.rotationZ,
            currentGameObjectData.rotationW);
        current = Instantiate(current, currentLocation, currentQuaternion) as GameObject;
        current.name = cFilename  + currentLocation.ToString();
	}

    [RPC]
    public StoredPlayerPrefs SendPlayerPrefs()
    {
        return GameObject.Find("Login Data").GetComponent<StoredPlayerPrefs>();
    }
}
