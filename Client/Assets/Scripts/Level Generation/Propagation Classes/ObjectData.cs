using UnityEngine;
using System.Collections;

public class ObjectData
{
    public ObjectData()
    {
    }

    public Vector3 direction;
    public Vector3 location;
    public bool exists = false;
    public bool[] connections = 
    {
        false,
        false,
        false,
        false,
    };
}