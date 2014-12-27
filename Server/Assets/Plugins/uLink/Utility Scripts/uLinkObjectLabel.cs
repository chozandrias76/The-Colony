// (c)2011 MuchDifferent. All Rights Reserved.

using UnityEngine;
using MonoBehaviour = uLink.MonoBehaviour;
using NetworkMessageInfo = uLink.NetworkMessageInfo;

[AddComponentMenu("uLink Utilities/Object Label")]
public class uLinkObjectLabel : MonoBehaviour
{
    public float clampBorderSize = 0.05f;
        // How much viewport space to leave at the borders when a label is being clamped

    public bool clampToScreen = false; // If true, label will be visible even if object is off screen

    public Color color = Color.white;

    private GUIText instantiatedLabel;
    public bool manualUpdate = false;
    public float maxDistance = 500;
    public float minDistance = 1;
    public Vector3 offset = new Vector3(0, 2, 0); // Units in world space to offset; 1 unit above object by default
    public GUIText prefabLabel;

    public bool useInitialData = false;

    private void uLink_OnNetworkInstantiate(NetworkMessageInfo info)
    {
        if (!enabled) return;

        string text;

        if (!useInitialData)
        {
            text = info.networkView.owner.ToString();
        }
        if (!info.networkView.initialData.TryRead(out text))
        {
            // TODO: log error
            return;
        }

        instantiatedLabel = (GUIText) Instantiate(prefabLabel, Vector3.zero, Quaternion.identity);
        instantiatedLabel.text = text;
        instantiatedLabel.material.color = color;
    }

    private void OnDisable()
    {
        if (instantiatedLabel != null)
        {
            DestroyImmediate(instantiatedLabel.gameObject);
            instantiatedLabel = null;
        }
    }

    private void LateUpdate()
    {
        if (manualUpdate) return;

        ManualUpdate();
    }

    public void ManualUpdate()
    {
        if (instantiatedLabel == null || Camera.main == null) return;

        Vector3 pos;

        if (clampToScreen)
        {
            Vector3 rel = Camera.main.transform.InverseTransformPoint(transform.position);
            rel.z = Mathf.Max(rel.z, 1.0f);

            pos = Camera.main.WorldToViewportPoint(Camera.main.transform.TransformPoint(rel + offset));
            pos = new Vector3(
                Mathf.Clamp(pos.x, clampBorderSize, 1.0f - clampBorderSize),
                Mathf.Clamp(pos.y, clampBorderSize, 1.0f - clampBorderSize),
                pos.z);
        }
        else
        {
            pos = Camera.main.WorldToViewportPoint(transform.position + offset);
        }

        instantiatedLabel.transform.position = pos;
        instantiatedLabel.enabled = (pos.z >= minDistance && pos.z <= maxDistance);
    }

    public static void ManualUpdateAll()
    {
        var labels = (uLinkObjectLabel[]) FindObjectsOfType(typeof (uLinkObjectLabel));

        foreach (uLinkObjectLabel label in labels)
        {
            label.ManualUpdate();
        }
    }
}