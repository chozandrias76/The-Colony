using System;
using UnityEngine;
using System.Collections;
using uLink;

[Serializable]
public class CreatePlayerCellData : uLink.MonoBehaviour
{
    public  CellData playerData = new CellData();
    
    //public float kPh = playerData.kPh;
    //public Vector3 location = playerData.location;
    //public float n02 = playerData.n02;
    //public float oxy = playerData.oxy;
    //public float plasma = playerData.plasma;
    //public float rad = playerData.rad;
    //public float scale = playerData.scale;
    //public float temp = playerData.temp;
    //public bool transmissive = playerData.transmissive;
    public float oxygen;
    void Start()
    {
        //oxygen = playerData.oxy;
    }
    void Update()
    {
        oxygen = playerData.oxy;
    }

    [RPC]
    void SendData(float data)
    {
        gameObject.GetComponent<CreatePlayerCellData>().playerData.oxy = data;
    }

    void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
    {
        if (info.networkView == networkView)
        {
            if (stream.isWriting)
            {
                
            }
            else if (stream.isReading)
            {
                
            }
        }
    }

}
