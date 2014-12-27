// (c)2011 MuchDifferent. All Rights Reserved.

using uLink;
using UnityEngine;
using BitStream = uLink.BitStream;
using MonoBehaviour = uLink.MonoBehaviour;
using NetworkMessageInfo = uLink.NetworkMessageInfo;

/// <summary>
///     Implementation for the script component uLinkNetworkObservedList.
/// </summary>
/// <remarks>
///     This script component is used to set up a list of components to serialize for
///     a game object or prefab with a uLinkNetworkView.
///     One example of a game object with two components to serialize is a tank with a turret. The tank is
///     the game object, the turret is one component, the body of the tank is another component. When the state
///     of the tank is serialized the body includes position + velocity + rotation. The turret has another
///     rotation. The send the comple tank state over the network using uLink state synchronization it is
///     manadatory to add a uLinkNetworkView (script component) to the game object. Then add a
///     uLinkNetworkObservedList (script component) to the game object.
///     Add the tank body and the tank turret to the list in the uLinkNetworkObservedList component.
///     Make sure the observed property of the uLinkNetworkView component is the the uLinkNetworkObservedList component.
///     Another example is a game object with an animation script component. If the game designer wants to send animation
///     state and position state, the uLinkNetworkObservedList can be used. THis is how to do that:
///     Add a uLinkNetworkObservedList to the game object. Add the postions (your script) and the animation (your script)
///     to the list. Your two scripts must implement the callback uLink_OnSerializeNetworkView. Make sure the observed
///     property of the uLinkNetworkView component is the the uLinkNetworkObservedList component.
/// </remarks>
[AddComponentMenu("uLink Utilities/Observed List")]
public class uLinkObservedList : MonoBehaviour
{
    private NetworkObserved[] _list;
    public Component[] observedList;

    protected void uLink_OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (observedList == null) return;

        _CheckList();

        foreach (NetworkObserved observed in _list)
        {
            if (observed.serializeProxy != null)
            {
                observed.serializeProxy(stream, info);
            }
        }
    }

    protected void uLink_OnSerializeNetworkViewOwner(BitStream stream, NetworkMessageInfo info)
    {
        if (observedList == null) return;

        _CheckList();

        foreach (NetworkObserved observed in _list)
        {
            if (observed.serializeOwner != null)
            {
                observed.serializeOwner(stream, info);
            }
        }
    }

    protected void uLink_OnHandoverNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (observedList == null) return;

        _CheckList();

        foreach (NetworkObserved observed in _list)
        {
            if (observed.handover != null)
            {
                observed.handover(stream, info);
            }
        }
    }

    private void _CheckList()
    {
        if (_list == null || _list.Length != observedList.Length)
        {
            _list = new NetworkObserved[observedList.Length];

            for (int i = 0; i < observedList.Length; i++)
            {
                _list[i] = new NetworkObserved(observedList[i]);
            }
        }
        else
        {
            for (int i = 0; i < observedList.Length; i++)
            {
                _list[i].UpdateBinding(observedList[i]);
            }
        }
    }
}