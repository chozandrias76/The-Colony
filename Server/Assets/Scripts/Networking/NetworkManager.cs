#region License

// // NetworkManager.cs
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

using System;
using SQLite;
using uLink;
using UnityEngine;
using MasterServer = uLink.MasterServer;
using MonoBehaviour = uLink.MonoBehaviour;
using Network = uLink.Network;
using NetworkPlayer = uLink.NetworkPlayer;
using NetworkViewID = uLink.NetworkViewID;
using System.IO;
using JsonFx.Json;

public class NetworkManager : MonoBehaviour
{
    public bool cleanupAfterPlayers = true;
    public bool dontDestroyOnLoad = false;
    public string gameName = "The Colony";
    public string gameType = "Default";
    public InstantiateOnConnected instantiateOnConnected = new InstantiateOnConnected();
    public bool isDedicatedServer;
    public string masterServerIP = "127.0.0.1";
    public int maxConnections = 64;
    public int port = 7100;

    public bool registerHost = false;

    public int targetFrameRate = 60;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        if (dontDestroyOnLoad) DontDestroyOnLoad(this);
        Network.InitializeServer(maxConnections, port);
    }

    private void uLink_OnServerInitialized()
    {
        MasterServer.dedicatedServer = isDedicatedServer;
        MasterServer.gameType = gameType;
        MasterServer.gameName = gameName;
        Debug.Log("Server successfully started on port " + Network.listenPort);
        if (registerHost) MasterServer.RegisterHost(gameType, gameName);
    }
	private string keyDir = Application.dataPath + "key.priv";
	
    private void uLink_OnPlayerDisconnected(NetworkPlayer player)
    {
        if (cleanupAfterPlayers)
        {
            Network.DestroyPlayerObjects(player);
            Network.RemoveRPCs(player);

            // this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
            Network.RemoveInstantiates(player);
        }
    }

    private void uLink_OnPlayerConnected(NetworkPlayer player)
    {
		//<summary>
		//Tell player to create a file to make them unique if they don't already have one
		//After the file is created, send the data back to the server
		var db = new SQLiteConnection(Application.dataPath+ "The Colony Server.db");
		//db.CreateCommand();
		//If the server already has the data, ignore adding to the database
		//This is where you check to make sure the player is authorized and do something about it
		//</summary>
        instantiateOnConnected.Instantiate(player);
        //instantiateOnConnected.AddToDatabase(player.ipAddress,player.loginData);
        PopulateWorldObjects.SetData(player);
    }

    [Serializable]
    public class InstantiateOnConnected
    {
        public NetworkViewID NetworkViewId;
        public bool appendLoginData = true;
        public int groupId = 0;
        public NetworkGroup netGroup;
        public GameObject ownerPrefab;
        public string playerJob;
        public string playerName;
        public GameObject proxyPrefab;
        public int securityStatus;
        public GameObject serverPrefab;
        public Vector3 startPosition = new Vector3(1, 120, 1);
        public Vector3 startRotation = new Vector3(0, 0, 0);
        public StoredPlayerPrefs PlayerPrefs;

        public void Instantiate(NetworkPlayer player)
        {
            //netGroup = new NetworkGroup(groupId);
            groupId++;
            if (ownerPrefab != null && proxyPrefab != null && serverPrefab != null)
            {
                Quaternion rotation = Quaternion.Euler(startRotation);
                AddToDatabase(playerName, playerJob);
                var playerData = new object[0];
                object[] initialData = appendLoginData ? Network.loginData : playerData;
                Network.Instantiate(player, proxyPrefab, ownerPrefab, serverPrefab, startPosition, rotation,
                    netGroup, initialData);
                //var playerPrefs = serverPrefab.GetComponent<StoredPlayerPrefs>();
                //playerPrefs = serverPrefab.uLinkNetworkView().RPC("SendPlayerPrefs",player);
            }
        }

        public void AddToDatabase(string _playerName, string _playerJob)
        {
            var sqlLiteConnection = new SQLiteConnection(Application.dataPath + "The Colony Server.db");
            
            sqlLiteConnection.CreateCommand("INSERT INTO Students('{0}','{1}',0)", _playerName, _playerJob);
        }

        public void GetClientPlayerPrefrences(StoredPlayerPrefs playerPrefs)
        {
            //Network.RPC(playerViewId, "GetStoredPlayerPrefs", player);
            this.PlayerPrefs = playerPrefs;
        }
    }
}