// (c)2011 MuchDifferent. All Rights Reserved.

using uLink;
using UnityEngine;
using MonoBehaviour = uLink.MonoBehaviour;
using Network = uLink.Network;
using NetworkPlayer = uLink.NetworkPlayer;
using NetworkViewID = uLink.NetworkViewID;

/// <summary>
///     By attaching this to a game object in the Hierarchy view, it will
///     automatically be instantiated for all others over the network when
///     you start. This works for both clients and servers. You can specify
///     different prefabs for Proxy and Owner. The Owner is the original
///     player. This script requires a non-authoritative server.
/// </summary>
[AddComponentMenu("uLink Utilities/Instantiate For Others")]
[RequireComponent(typeof (uLinkNetworkView))]
public class uLinkInstantiateForOthers : MonoBehaviour
{
    public bool appendLoginData = false;
    public GameObject othersPrefab;

    private void Start()
    {
        if (Network.status == NetworkStatus.Connected)
        {
            uLink_OnConnectedToServer();
        }
    }

    private void uLink_OnConnectedToServer()
    {
        if (networkView.viewID != NetworkViewID.unassigned)
        {
            return;
        }

        if (Network.isAuthoritativeServer && Network.isClient)
        {
            // TODO: warn if server is authoritative && this is not the server
            return;
        }

        Transform trans = transform;
        NetworkPlayer owner = Network.player;
        NetworkViewID viewID = Network.AllocateViewID();
        object[] initialData = appendLoginData ? Network.loginData : new object[0];

        if (owner != NetworkPlayer.server)
            Network.Instantiate(viewID, owner, othersPrefab, null, othersPrefab, trans.position, trans.rotation, 0,
                initialData);
        else
            Network.Instantiate(viewID, owner, othersPrefab, othersPrefab, null, trans.position, trans.rotation, 0,
                initialData);

        networkView.SetViewID(viewID, owner);
        networkView.SetInitialData(initialData);
    }
}