// (c)2012 MuchDifferent. All Rights Reserved.

using uLink;
using UnityEngine;
using MonoBehaviour = uLink.MonoBehaviour;
using NetworkMessageInfo = uLink.NetworkMessageInfo;
using NetworkView = uLink.NetworkView;

[AddComponentMenu("uLink Utilities/Override Network Destroy")]
public class uLinkOverrideNetworkDestroy : MonoBehaviour
{
    public bool autoDestroyAfterMessage = true;
    public string broadcastMessage = "uLink_OnNetworkDestroy";

    private NetworkView mainNetworkView;
    private NetworkInstantiator.Destroyer oldDestroyer;

    protected void uLink_OnNetworkInstantiate(NetworkMessageInfo info)
    {
        mainNetworkView = info.networkView;

        // override the instance's NetworkInsantiator Destroyer delegate.
        oldDestroyer = mainNetworkView.instantiator.destroyer;
        mainNetworkView.instantiator.destroyer = OverrideDestroyer;
    }

    private void OverrideDestroyer(NetworkView instance)
    {
        if (autoDestroyAfterMessage)
        {
            instance.BroadcastMessage(broadcastMessage, SendMessageOptions.DontRequireReceiver);
            Destroy();
        }
        else
        {
            // if we're relying on the message receiver for cleanup, then make sure there is one.
            instance.BroadcastMessage(broadcastMessage, SendMessageOptions.RequireReceiver);
        }
    }

    public void Destroy()
    {
        if (oldDestroyer != null) oldDestroyer(mainNetworkView);
    }
}