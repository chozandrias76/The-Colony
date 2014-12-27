using UnityEngine;
using System.Collections;
using System;

public class LoadMap : MonoBehaviour {
    const int TILE_SIZE = 5;
    private CellData initGameMap;
    JSONFiles _JSONFiles = new JSONFiles();
    GameMap _GameMap = new GameMap();
    bool loop = true;
    

    // Use this for initialization
    void Start () {
        _GameMap = GameObject.Find ("_gameLogic").GetComponent<GameMap>();
        _JSONFiles = GameObject.Find ("_gameLogic").GetComponent<JSONFiles>();
        _JSONFiles.SetData();
        GenerateCellData();
        GameMap.GenerateObjectData();
        //_GameMap.GenerateObjectData();
        //_GameMap.FindFullAtmosphereSpaces();
        GameMap.FindFullAtmosphereSpaces();
        _GameMap.dataGenerated = true;
        this.gameObject.AddComponent("ConsoleScreen");
        //GameObject.Instantiate(Resources.Load("Character/Owner@playerPrefab"), new Vector3(0, 115, 0),
        //                       Quaternion.identity);
    }
    

    public void GenerateCellData()
    {
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                initGameMap = new CellData();
                initGameMap.location = new Vector3(x, 0, y);
                GameMap.SetCell(x, y, initGameMap);
                //_GameMap.SetCell(x, y, initGameMap);
                //_GameMap.SetConnections(x, y, initGameMap);
                GameMap.SetConnections(x,y,initGameMap);
            }
        }
    }

    
    void Update() {
    }
}