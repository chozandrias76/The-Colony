// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using uLink;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using Network = uLink.Network;

// TODO: add more settings...

[AddComponentMenu("uLink Utilities/Override Settings")]
public class uLinkOverrideSettings : MonoBehaviour
{
    public CellServer cellServer;
    public Client client;

    public bool dontDestroyOnLoad = false;
    public General general;
    public MasterServer masterServer;
    public Server server;

    private void Awake()
    {
        if (dontDestroyOnLoad) DontDestroyOnLoad(this);

        general.Override();
        client.Override();
        server.Override();
        cellServer.Override();
        masterServer.Override();
    }

    private void uLink_OnPreStartNetwork(NetworkStartEvent nsEvent)
    {
        general.Override();

        switch (nsEvent)
        {
            case NetworkStartEvent.Client:
                client.Override();
                break;
            case NetworkStartEvent.Server:
                server.Override();
                break;
            case NetworkStartEvent.CellServer:
                cellServer.Override();
                break;
            case NetworkStartEvent.MasterServer:
                masterServer.Override();
                break;
        }
    }

    [Serializable]
    public class CellServer : Settings
    {
        public float trackMaxDelta = Network.trackMaxDelta;
        public float trackRate = Network.trackRate;

        protected override void Apply()
        {
            Network.trackRate = trackRate;
            Network.trackMaxDelta = trackMaxDelta;
        }
    }

    [Serializable]
    public class Client : Settings
    {
        public bool requireSecurityForConnecting = Network.requireSecurityForConnecting;
        public int symmetricKeySize = Network.symmetricKeySize;

        protected override void Apply()
        {
            Network.requireSecurityForConnecting = requireSecurityForConnecting;
            Network.symmetricKeySize = symmetricKeySize;
        }
    }

    [Serializable]
    public class General : Settings
    {
        public bool isAuthoritativeServer = Network.isAuthoritativeServer;
        public int maxManualViewIDs = Network.maxManualViewIDs;
        public int minimumAllocatableViewIDs = Network.minimumAllocatableViewIDs;
        public int minimumUsedViewIDs = Network.minimumUsedViewIDs;
        public float sendRate = Network.sendRate;
        public bool useDifferentStateForOwner = Network.useDifferentStateForOwner;

        protected override void Apply()
        {
            Network.sendRate = sendRate;
            Network.maxManualViewIDs = maxManualViewIDs;
            Network.isAuthoritativeServer = isAuthoritativeServer;
            Network.minimumAllocatableViewIDs = minimumAllocatableViewIDs;
            Network.minimumUsedViewIDs = minimumUsedViewIDs;
            Network.useDifferentStateForOwner = useDifferentStateForOwner;
        }
    }

    [Serializable]
    public class MasterServer : Settings
    {
        public string comment = uLink.MasterServer.comment;
        public bool dedicatedServer = uLink.MasterServer.dedicatedServer;
        public string gameLevel = uLink.MasterServer.gameLevel;
        public string gameMode = uLink.MasterServer.gameMode;
        public string gameName = uLink.MasterServer.gameName;
        public string gameType = uLink.MasterServer.gameType;
        public string ipAddress = uLink.MasterServer.ipAddress;
        public string password = uLink.MasterServer.password;
        public int port = uLink.MasterServer.port;
        public float updateRate = uLink.MasterServer.updateRate;

        protected override void Apply()
        {
#if !UNITY_2_6 && !UNITY_2_6_1
            if (Application.webSecurityEnabled)
            {
                Security.PrefetchSocketPolicy(ipAddress, 843);
            }
#endif

            uLink.MasterServer.comment = comment;
            uLink.MasterServer.dedicatedServer = dedicatedServer;
            uLink.MasterServer.gameLevel = gameLevel;
            uLink.MasterServer.gameMode = gameMode;
            uLink.MasterServer.gameName = gameName;
            uLink.MasterServer.gameType = gameType;
            uLink.MasterServer.ipAddress = ipAddress;
            uLink.MasterServer.password = password;
            uLink.MasterServer.port = port;
            uLink.MasterServer.updateRate = updateRate;
        }
    }

    [Serializable]
    public class Server : Settings
    {
        public string incomingPassword = Network.incomingPassword;
        public string redirectIP = Network.redirectIP;
        public int redirectPort = Network.redirectPort;
        public bool useProxy = Network.useProxy;
        public bool useRedirect = Network.useRedirect;

        protected override void Apply()
        {
            Network.incomingPassword = incomingPassword;
            Network.useProxy = useProxy;
            Network.useRedirect = useRedirect;
            Network.redirectIP = redirectIP;
            Network.redirectPort = redirectPort;
        }
    }

    [Serializable]
    public abstract class Settings
    {
        [SerializeField] private bool _override = false;

        public void Override()
        {
            if (_override) Apply();
        }

        protected abstract void Apply();
    }
}