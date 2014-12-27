using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CellData
	{
		public CellData(){}
		//public IDictionary<Vector3, string> cellContents;
		public List<string> contents;
		public float kPh = 100.0f;
		public float oxy = 0.2f;
		public float n02 = 0.8f;
		public float plasma = 0.0f;
		public float temp = 293.15f;
		public float rad = 0.0f;
		public float scale = 0.0f;
		public Vector3 location;
        public bool transmissive = true;
        Connection[] connections = new Connection[4];

        public override string ToString()
        {
            string s = location.ToString();
            return "The location of this cell is: " + s;
        }
	}

struct Connection
{
    bool isWall;
    float breachedScale;
    bool isWire;
    bool isDuct;
    bool isDoor;
}