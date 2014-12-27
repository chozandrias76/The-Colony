// (c)2011 MuchDifferent. All Rights Reserved.

using System.Collections.Generic;
using uLink;
using UnityEngine;
using MonoBehaviour = uLink.MonoBehaviour;
using NetworkMessageInfo = uLink.NetworkMessageInfo;
using NetworkView = uLink.NetworkView;

/// <summary>
///     Use this script to make a pool of pre-instantiated prefabs that will be used
///     when prefabs are network instantiated via uLink.Network.Instantiate().
/// </summary>
/// <remarks>
///     This wonderful script makes it possible to pool game objects (instantiated prefabs)
///     and uLink will use this pool of objects as soon as the gameplay code makes a call to
///     Network.Instantiate.
///     The wonderful part is that you do not have to change your code at all to use the pool
///     mechanism. This makes it very easy to check if your game is faster and more smooth
///     with a pool.
///     In some games, with lots of spawning NPCs, guided missiles etc, the pool
///     is very convenient to avoid the overhead of calling Object.Instantiate() and Object.Destroy().
///     The need for a pool is mainly to increase performance, Instantiating objects is a
///     heavy operation in Unity. By load-testing the game with the uTsung tool it is easier to
///     make decisions when to use a pool for prefabs.
///     A normal scenario is to pool creator prefabs in the server scene and pool proxy prefabs
///     in the client scene.
///     The value minSize is the number of prefabs that will be instantiated at startup.
///     If the number of Instantiate calls does go above this value, the pool will be increased
///     at run-time.
/// </remarks>
[AddComponentMenu("uLink Utilities/Instantiate Pool")]
public class uLinkInstantiatePool : MonoBehaviour
{
    private readonly Stack<NetworkView> pool = new Stack<NetworkView>();
    public int minSize = 50; // This number of prefabs will be instantiated at startup

    private Transform parent;
    public NetworkView prefab;

    private void Awake()
    {
        if (enabled) CreatePool();
    }

    private void Start()
    {
        // this is here just so the componet can be enabled/disabled.
    }

    private void OnDisable()
    {
        DestroyPool();
    }

    public void CreatePool()
    {
        if (prefab._manualViewID != 0)
        {
            Debug.LogError("Prefab viewID must be set to Allocated or Unassigned", prefab);
            return;
        }

        parent = new GameObject(name + "-Pool").transform;

        for (int i = 0; i < minSize; i++)
        {
            var instance = (NetworkView) Instantiate(prefab);

#if UNITY_4_0
            instance.gameObject.SetActive(false);
#else
			instance.gameObject.SetActiveRecursively(false);
#endif

            instance.transform.parent = parent;

            pool.Push(instance);
        }

        NetworkInstantiator.Add(prefab.name, Creator, Destroyer);
    }

    public void DestroyPool()
    {
        NetworkInstantiator.Remove(prefab.name);
        pool.Clear();

        // This is being done due to a race condition. Switching rooms might result the parent object being destroyed earlier.
        if (parent != null)
        {
            Destroy(parent.gameObject);
            parent = null;
        }
    }

    private NetworkView Creator(string prefabName, NetworkInstantiateArgs args, NetworkMessageInfo info)
    {
        NetworkView instance;

        if (pool.Count > 0)
        {
            instance = pool.Pop();

            args.SetupNetworkView(instance);

#if UNITY_4_0
            instance.gameObject.SetActive(true);
#else
			instance.gameObject.SetActiveRecursively(true);
#endif
        }
        else
        {
            instance = NetworkInstantiatorUtility.Instantiate(prefab, args);
        }

        NetworkInstantiatorUtility.BroadcastOnNetworkInstantiate(instance, info);

        return instance;
    }

    private void Destroyer(NetworkView instance)
    {
#if UNITY_4_0
        instance.gameObject.SetActive(false);
#else
		instance.gameObject.SetActiveRecursively(false);
#endif

        pool.Push(instance);
    }
}