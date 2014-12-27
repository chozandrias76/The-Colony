using UnityEngine;
using System.Collections;
using uLink;

public class MainPlayerGUI : uLink.MonoBehaviour
{
    public MainPlayerGUI()
    {
    }

    public float boxDim;
    public GUISkin mainSkin;
    bool GUIEnabled = false;
    bool mouseLookEnabled = false;
    bool rollOut = false;
    public float speed = 0.1f;
    private Transform playerCellLocation;
    private float playerOxygenValue;
    private float playerRadiationValue;
    private float playerPressureValue;
    private float playerHeatValue;
    private string playerOxygenString;
    private string[] playerOxygenStrings = {
        "Safe",
        "Poor",
        "Unsafe",
        "Dangerous",
        "Death Imminent"
    };
    private int oxygenStringValue;
    private string playerRadiationString;
    private string playerPressureString;
    private string playerHeatString;
    private bool playerInternalsValue = false;
    private string playerInternalsString;
    private static CellData playerCellData;
    private GameObject goData;
    public Vector3 location;


    void Start()
    {
        Screen.showCursor = false;
    }
    void Update()
    {
        CreatePlayerCellData _createPlayerCellData = GetComponent<CreatePlayerCellData>();

        location = _createPlayerCellData.playerData.location;
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (GUIEnabled == false)
            {
                //this.gameObject.GetComponent<MouseLook>().enabled = false;
                //this.gameObject.GetComponentInChildren<Camera>().GetComponent<MouseLook>().enabled = false;
                //this.gameObject.GetComponentInChildren<Camera>().transform.rotation = this.gameObject.GetComponent("Camera Orientation2").transform.rotation;
                //this.gameObject.GetComponentInChildren<Camera>().transform.position = this.gameObject.GetComponent("Camera Orientation2").transform.position;
                this.gameObject.GetComponentInChildren<Camera>().transform.position += this.gameObject.GetComponentInChildren<Camera>().transform.TransformDirection(Vector3.back * 3);
                this.gameObject.GetComponentInChildren<Camera>().camera.cullingMask = ~0;
                Screen.showCursor = true;
                GUIEnabled = true;
            }
            else if (GUIEnabled == true)
            {
                //this.gameObject.GetComponent<MouseLook>().enabled = true;
                //this.gameObject.GetComponentInChildren<Camera>().GetComponent<MouseLook>().enabled = true;

                //this.gameObject.GetComponentInChildren<Camera>().transform.rotation = this.gameObject.GetComponent("Camera Orientation1").transform.rotation;
                this.gameObject.GetComponentInChildren<Camera>().transform.position += this.gameObject.GetComponentInChildren<Camera>().transform.TransformDirection(Vector3.forward * 3);
                this.gameObject.GetComponentInChildren<Camera>().camera.cullingMask = 1 << 0;
                Screen.showCursor = true;
                GUIEnabled = false;
            }
        }


        playerRadiationValue = _createPlayerCellData.playerData.rad;
        playerRadiationString = System.String.Format("{0:F2} Rads", playerRadiationValue);
        playerOxygenValue = _createPlayerCellData.playerData.oxy;
        playerOxygenValue = playerOxygenValue * 100;

        playerOxygenString = System.String.Format("{0:F1}%\n Oxygen ", playerOxygenValue);
        if (playerOxygenValue >= 18 && playerOxygenValue < 25)
            oxygenStringValue = 0;
        if (playerOxygenValue > 14 && playerOxygenValue < 18)
            oxygenStringValue = 1;
        if (playerOxygenValue >= 10 && playerOxygenValue < 14)
            oxygenStringValue = 2;
        if (playerOxygenValue >= 6 && playerOxygenValue < 10)
            oxygenStringValue = 3;
        if (playerOxygenValue >= 0 && playerOxygenValue < 6)
            oxygenStringValue = 4;


        if (playerInternalsValue)
            playerInternalsString = "Online";
        else
            playerInternalsString = "Offline";
        GameObject.Find("Oxygen Value").GetComponent<UILabel>().text = playerOxygenString;
        GameObject.Find("Heat Value").GetComponent<UILabel>().text = playerHeatString;
        GameObject.Find("Radiation Value").GetComponent<UILabel>().text = playerRadiationString;
        GameObject.Find("Internals Value").GetComponent<UILabel>().text = playerInternalsString;
    }

}