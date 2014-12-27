using UnityEngine;

public class ComboBox
{
    private static bool forceToUnShow;
    private static int useControlID = -1;
    private readonly string boxStyle;
    private readonly string buttonStyle;
    private readonly GUIContent[] listContent;
    private readonly GUIStyle listStyle;
    private GUIContent buttonContent;
    private bool isClickedComboButton;
    private Rect rect;
    private int selectedItemIndex;

    public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
    {
        this.rect = rect;
        this.buttonContent = buttonContent;
        this.listContent = listContent;
        buttonStyle = "button";
        boxStyle = "box";
        this.listStyle = listStyle;
    }

    public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle,
        GUIStyle listStyle)
    {
        this.rect = rect;
        this.buttonContent = buttonContent;
        this.listContent = listContent;
        this.buttonStyle = buttonStyle;
        this.boxStyle = boxStyle;
        this.listStyle = listStyle;
    }

    public int SelectedItemIndex
    {
        get { return selectedItemIndex; }
        set { selectedItemIndex = value; }
    }

    public int Show()
    {
        if (forceToUnShow)
        {
            forceToUnShow = false;
            isClickedComboButton = false;
        }

        bool done = false;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.mouseUp:
            {
                if (isClickedComboButton)
                {
                    done = true;
                }
            }
                break;
        }

        if (GUI.Button(rect, buttonContent, buttonStyle))
        {
            if (useControlID == -1)
            {
                useControlID = controlID;
                isClickedComboButton = false;
            }

            if (useControlID != controlID)
            {
                forceToUnShow = true;
                useControlID = controlID;
            }
            isClickedComboButton = true;
        }

        if (isClickedComboButton)
        {
            var listRect = new Rect(rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f),
                rect.width, listStyle.CalcHeight(listContent[0], 1.0f)*listContent.Length);

            GUI.Box(listRect, "", boxStyle);
            int newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
            if (newSelectedItemIndex != selectedItemIndex)
            {
                selectedItemIndex = newSelectedItemIndex;
                buttonContent = listContent[selectedItemIndex];
            }
        }

        if (done)
            isClickedComboButton = false;

        return selectedItemIndex;
    }
}