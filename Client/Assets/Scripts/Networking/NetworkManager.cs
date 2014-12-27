// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using UnityEngine;
using uLink;

public class NetworkManager : uLink.MonoBehaviour
{
    [Serializable]
    public class InstantiateOnConnected
    {
        public Vector3 startPosition = new Vector3(1, 120, 1);
        public Vector3 startRotation = new Vector3(0, 0, 0);

        public GameObject ownerPrefab;
        public GameObject proxyPrefab;
        public GameObject serverPrefab;

        uLink.NetworkGroup netGroup;
        

        public bool appendLoginData = false;
        public void Instantiate(uLink.NetworkPlayer player)
        {
            if (ownerPrefab != null && proxyPrefab != null && serverPrefab != null)
            {
                netGroup = new uLink.NetworkGroup();

                Quaternion rotation = Quaternion.Euler(startRotation);
                object[] initialData = appendLoginData ? uLink.Network.loginData : new object[0];

                uLink.Network.Instantiate(player, proxyPrefab, ownerPrefab, serverPrefab, startPosition, rotation, netGroup, initialData);
            }
        }
    }
    public string gameName = "The Colony";
    public string gameType = "Default";
    public string masterServerIP = "127.0.0.1";
    public bool isDedicatedServer;
    public int port = 7100;
    public int maxConnections = 64;

    public bool cleanupAfterPlayers = true;

    public bool registerHost = false;

    public int targetFrameRate = 60;

    public bool dontDestroyOnLoad = false;

    public InstantiateOnConnected instantiateOnConnected = new InstantiateOnConnected();

    void Start()
    {
        
        Application.targetFrameRate = targetFrameRate;

        if (dontDestroyOnLoad) DontDestroyOnLoad(this);
        uLink.Network.InitializeServer(maxConnections, port);
    }

    void uLink_OnServerInitialized()
    {
        uLink.MasterServer.dedicatedServer = isDedicatedServer;
        uLink.MasterServer.gameType = gameType;
        uLink.MasterServer.gameName = gameName;
        Debug.Log("Server successfully started on port " + uLink.Network.listenPort);
        
        if (registerHost) uLink.MasterServer.RegisterHost(gameType,gameName);
    }

    void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
    {
        if (cleanupAfterPlayers)
        {
            uLink.Network.DestroyPlayerObjects(player);
            uLink.Network.RemoveRPCs(player);

            // this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
            uLink.Network.RemoveInstantiates(player);
        }
    }
        
    void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
    {
        
        instantiateOnConnected.Instantiate(player);
    }
}
