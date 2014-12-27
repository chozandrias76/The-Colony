// Popup list created by Eric Haines
// Popup list Extended by John Hamilton. john@nutypeinc.com

using UnityEngine;

public class Popup
{
    // Delegate
    public delegate void ListCallBack();

    private static readonly int popupListHash = "PopupList".GetHashCode();


    public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, object[] list,
        GUIStyle listStyle, ListCallBack callBack)
    {
        return List(position, ref showList, ref listEntry, buttonContent, list, "button", "box", listStyle, callBack);
    }

    public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, object[] list,
        GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle, ListCallBack callBack)
    {
        int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
        bool done = false;
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.mouseDown:
                if (position.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = controlID;
                    showList = true;
                }
                break;
            case EventType.mouseUp:
                if (showList)
                {
                    done = true;
                    // Call our delegate method
                    callBack();
                }
                break;
        }

        GUI.Label(position, buttonContent, buttonStyle);
        if (showList)
        {
            // Get our list of strings
            var text = new string[list.Length];
            // convert to string
            for (int i = 0; i < list.Length; i++)
            {
                text[i] = list[i].ToString();
            }

            var listRect = new Rect(position.x, position.y, position.width, list.Length*20);
            GUI.Box(listRect, "", boxStyle);
            listEntry = GUI.SelectionGrid(listRect, listEntry, text, 1, listStyle);
        }
        if (done)
        {
            showList = false;
        }
        return done;
    }
}