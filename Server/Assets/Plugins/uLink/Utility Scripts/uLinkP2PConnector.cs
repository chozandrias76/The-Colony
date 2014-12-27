// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using uLink;
using UnityEngine;
using MonoBehaviour = uLink.MonoBehaviour;

/// <summary>
///     Attach this script component to a game object that needs to be able to make P2P connection attempts.
/// </summary>
/// <remarks>
///     When this script component is attached to a game object a UDP socket will be opened at runtime and this socket will
///     be used bu uLink to send p2p connection attempts to one other peer. THe IP and port for the remote peer can be set
///     using the public properties of this script.
///     If your game object needs to connect to several peers, just add more uLinkP2PConnector components and configure
///     them.
///     It is not possible so send statesync over a p2p connection. Use the connection to send RPCs instead.
/// </remarks>
/// <seealso cref="uLinkNetworkP2P" />
[AddComponentMenu("uLink Utilities/P2P Connector")]
[RequireComponent(typeof (uLinkNetworkP2P))]
public class uLinkP2PConnector : MonoBehaviour
{
    private string cachedHost = String.Empty;
    public float connectingTimeout = 1;
    public string host = "127.0.0.1";
    public string incomingPassword = String.Empty;
    public float interval = 0.2f;
    private float lastTimeConnecting = Single.NaN;

    private NetworkP2P p2p;
    public int port = 0;
    private NetworkPeer target = NetworkPeer.unassigned;

    private void Awake()
    {
        p2p = networkP2P;

        if (enabled) OnEnable();
    }

    private void OnEnable()
    {
        if (!IsInvoking("KeepConnected"))
        {
            InvokeRepeating("KeepConnected", interval, interval);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("KeepConnected");
    }

    private void KeepConnected()
    {
        if (String.IsNullOrEmpty(host) || port == 0)
        {
            return;
        }

        if (cachedHost != host || target.port != port)
        {
            cachedHost = host;
            target = new NetworkPeer(host, port);
        }

        switch (p2p.GetStatus(target))
        {
            case NetworkStatus.Disconnected:
                lastTimeConnecting = Time.time;
                p2p.Connect(target, incomingPassword);
                break;

            case NetworkStatus.Connecting:
                if (Single.IsNaN(lastTimeConnecting)) lastTimeConnecting = Time.time;
                else if (Time.time >= lastTimeConnecting + connectingTimeout) p2p.CloseConnection(target, true);
                break;

            default:
                lastTimeConnecting = Single.NaN;
                break;
        }
    }
}