// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using uLink;
using UnityEngine;
using HostData = uLink.HostData;
using MasterServer = uLink.MasterServer;
using MonoBehaviour = uLink.MonoBehaviour;
using Network = uLink.Network;
using NetworkConnectionError = uLink.NetworkConnectionError;
using NetworkDisconnection = uLink.NetworkDisconnection;
using NetworkPlayer = uLink.NetworkPlayer;
using NetworkView = uLink.NetworkView;
using Random = UnityEngine.Random;

[AddComponentMenu("uLink Utilities/Client GUI")]
public class uLinkClientGUI : MonoBehaviour
{
    private const float QUICK_WIDTH = 220;
    private const float ADVANCED_WIDTH = 620;
    private const float BUSY_WIDTH = 220;
    public UnityEngine.MonoBehaviour[] disableWhenGUI;
    public bool dontDestroyOnLoad = false;
    public UnityEngine.MonoBehaviour[] enableWhenGUI;
    public string gameType = "MyUniqueGameType";
    public int guiDepth = 0;
    public GUISkin guiSkin = null;
    public bool hasAdvancedMode = true;
    public bool hideCursor = true;

    public Texture2D iconFavorite;
    public Texture2D iconNonfavorite;
    public bool inputName = true;

    private bool isQuickMode = true;

    private bool isRedirected;

    public bool lockCursor = true;
    private string playerName;
    public string quickHost = "127.0.0.1";
    public int quickPort = 7100;
    public string quickText = "Play on Localhost";
    public bool reloadOnDisconnect = false;
    private Vector2 scrollPosition = Vector2.zero;
    private int selectedGrid;
    public bool showGameLevel = false;
    public int targetFrameRate = 60;

    private void Awake()
    {
#if !UNITY_2_6 && !UNITY_2_6_1
        if (Application.webSecurityEnabled)
        {
            Security.PrefetchSocketPolicy(NetworkUtility.ResolveAddress(quickHost).ToString(), 843);
            Security.PrefetchSocketPolicy(MasterServer.ipAddress, 843);
        }
#endif

        Application.targetFrameRate = targetFrameRate;

        if (dontDestroyOnLoad) DontDestroyOnLoad(this);

        playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 100));
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("playerName", playerName);
    }

    private void OnGUI()
    {
        if (Network.lastError == NetworkConnectionError.NoError && Network.status == NetworkStatus.Connected &&
            NetworkView.FindByOwner(Network.player).Length != 0 && (!lockCursor || Screen.lockCursor))
        {
            EnableGUI(false);
            return;
        }

        EnableGUI(true);

        GUISkin oldSkin = GUI.skin;
        int oldDepth = GUI.depth;

        GUI.skin = guiSkin;
        GUI.depth = guiDepth;

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        if (Network.lastError != NetworkConnectionError.NoError || Network.status != NetworkStatus.Disconnected)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(BUSY_WIDTH));
            BusyGUI();
            GUILayout.EndVertical();
        }
        else if (isQuickMode)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(QUICK_WIDTH));
            QuickGUI();
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(ADVANCED_WIDTH));
            AdvancedGUI();
            GUILayout.EndVertical();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUI.skin = oldSkin;
        GUI.depth = oldDepth;
    }

    private void BusyGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(5);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        string busyDoingWhat = "Busy...";

        if (Network.lastError != NetworkConnectionError.NoError)
        {
            busyDoingWhat = "Error: " + NetworkUtility.GetErrorString(Network.lastError);
        }
        else if (Network.status == NetworkStatus.Connected)
        {
            if (NetworkView.FindByOwner(Network.player).Length != 0)
            {
                if (lockCursor)
                {
                    busyDoingWhat = "Click to start playing";

                    if (Input.GetMouseButton(0)) Screen.lockCursor = true;
                }
            }
            else
            {
                busyDoingWhat = "Instantiating...";
            }
        }
        else if (Network.status == NetworkStatus.Connecting)
        {
            string prefix = isRedirected ? "Redirecting to " : "Connecting to ";
            busyDoingWhat = prefix + NetworkPlayer.server.endpoint;
        }
        else if (Network.status == NetworkStatus.Disconnecting)
        {
            busyDoingWhat = "Disconnecting";
        }

        GUILayout.Label(busyDoingWhat);

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (Network.status == NetworkStatus.Connecting && !isRedirected)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Cancel", GUILayout.Width(80), GUILayout.Height(25)))
            {
                Network.DisconnectImmediate();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginVertical();
        GUILayout.Space(5);
        GUILayout.EndVertical();
    }

    private void QuickGUI()
    {
        if (inputName)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Please enter your name:");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            playerName = GUILayout.TextField(playerName, GUILayout.MinWidth(80));
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(quickText, GUILayout.Width(120), GUILayout.Height(25)))
        {
            Connect(quickHost, quickPort);
        }

        if (hasAdvancedMode)
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Advanced", GUILayout.Width(80), GUILayout.Height(25)))
            {
                isQuickMode = false;
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(2);
        GUILayout.EndVertical();
    }

    private void AdvancedGUI()
    {
        GUILayout.BeginHorizontal();
        selectedGrid = GUILayout.SelectionGrid(selectedGrid, new[] {"Internet", "LAN", "Favorites"}, 3,
            GUILayout.Height(22));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(5);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal("Box");
        GUILayout.Space(5);

        GUILayout.Label("Name", GUILayout.Width(80));
        if (showGameLevel) GUILayout.Label("Level", GUILayout.Width(80));
        GUILayout.Label("Players", GUILayout.Width(80));
        GUILayout.Label("Host", GUILayout.Width(140));
        GUILayout.Label("Ping", GUILayout.Width(80));

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Height(260));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, "Box");

        HostData[] hosts = null;
        switch (selectedGrid)
        {
            case 0:
                hosts = MasterServer.PollAndRequestHostList(gameType, 2);
                break;
            case 1:
                hosts = MasterServer.PollAndDiscoverLocalHosts(gameType, quickPort, 2);
                break;
            case 2:
                hosts = MasterServer.PollAndRequestKnownHosts(2);
                break;
        }

        if (hosts != null && hosts.Length > 0)
        {
            Array.Sort(hosts, delegate(HostData x, HostData y) { return x.gameName.CompareTo(y.gameName); });

            foreach (HostData data in hosts)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);

                GUILayout.Label(data.gameName, GUILayout.Width(80));
                if (showGameLevel) GUILayout.Label(data.gameLevel, GUILayout.Width(80));
                GUILayout.Label(data.connectedPlayers + "/" + data.playerLimit, GUILayout.Width(80));
                GUILayout.Label(data.ipAddress + ":" + data.port, GUILayout.Width(140));
                GUILayout.Label(data.ping.ToString(), GUILayout.Width(80));

                GUILayout.FlexibleSpace();

                if (MasterServer.PollKnownHostData(data.externalEndpoint) != null)
                {
                    if (iconFavorite != null ? GUILayout.Button(iconFavorite, "Label") : GUILayout.Button("Unlove"))
                    {
                        MasterServer.RemoveKnownHostData(data.externalEndpoint);
                    }
                }
                else
                {
                    if (iconNonfavorite != null ? GUILayout.Button(iconNonfavorite, "Label") : GUILayout.Button("Love"))
                    {
                        MasterServer.AddKnownHostData(data);
                    }
                }

                GUILayout.Space(5);

                if (Network.status == NetworkStatus.Disconnected)
                {
                    if (GUILayout.Button("Join"))
                    {
                        Connect(data);
                    }
                }

                GUILayout.Space(10);
                GUILayout.EndHorizontal();
            }
        }
        else if (selectedGrid == 2)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("No hosts have been marked as favorite");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (selectedGrid == 1 &&
                 (Application.platform == RuntimePlatform.WindowsWebPlayer ||
                  Application.platform == RuntimePlatform.OSXWebPlayer))
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("No LAN hosts have been discovered yet");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("this may be due to security restrictions on the webplayer");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Please wait for the list to begin populating...");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(5);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Space(2);

        if (GUILayout.Button("Back", GUILayout.Width(80), GUILayout.Height(25)))
        {
            isQuickMode = true;
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(2);
        GUILayout.EndVertical();
    }

    private void EnableGUI(bool enabled)
    {
        if (lockCursor) Screen.lockCursor = !enabled;
        if (hideCursor) Screen.showCursor = enabled;

        foreach (UnityEngine.MonoBehaviour component in enableWhenGUI)
        {
            component.enabled = enabled;
        }

        foreach (UnityEngine.MonoBehaviour component in disableWhenGUI)
        {
            component.enabled = !enabled;
        }
    }

    private void Connect(string host, int port)
    {
        isRedirected = false;

        if (inputName)
        {
            Network.Connect(host, port, "", playerName);
        }
        else
        {
            Network.Connect(host, port);
        }
    }

    private void Connect(HostData host)
    {
        isRedirected = false;

        if (inputName)
        {
            Network.Connect(host, "", playerName);
        }
        else
        {
            Network.Connect(host);
        }
    }

    private void uLink_OnRedirectingToServer()
    {
        isRedirected = true;
        EnableGUI(true);
    }

    private void uLink_OnDisconnectedFromServer(NetworkDisconnection mode)
    {
        isQuickMode = true;

        if (reloadOnDisconnect && mode != NetworkDisconnection.Redirecting && Application.loadedLevel != -1)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}