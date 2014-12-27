using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private readonly GameObject[] panelsGO = new GameObject[9];
    //private StoredPlayerPrefs[] clientStoredPlayerPrefs = new StoredPlayerPrefs[3];
    private GameObject anchor = GameObject.Find("Anchor");
    private string[] randomNames = new string[20];

    private bool[] clientPlayerList =
    {
        true,
        false,
        false
    };

    private int clientPlayerSelected = 0;
    public int ClientPlayerSelected
    {
        get
        {
            return clientPlayerSelected;
        }
        set
        {
            loginDataHolder.GetComponent<StoredPlayerPrefs>().playerChoice = value;
            clientPlayerSelected = value;
        }
    }
    private GameObject loginDataHolder;
    private void Start()
    {
        loginDataHolder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        loginDataHolder.name = "LoginData";
        loginDataHolder.transform.position = new Vector3(100f, 100f);
        loginDataHolder.AddComponent<StoredPlayerPrefs>();
        GameObject.DontDestroyOnLoad(loginDataHolder);
        
        panelsGO[0] = GameObject.Find("Audio Settings Panel");
        panelsGO[1] = GameObject.Find("Character Editor Panel");
        panelsGO[2] = GameObject.Find("Game Settings Panel");
        panelsGO[3] = GameObject.Find("Main Menu Panel");
        panelsGO[4] = GameObject.Find("Options Panel");
        panelsGO[5] = GameObject.Find("Video Settings Panel");
        panelsGO[6] = GameObject.Find("Sound Settings Panel");
        panelsGO[7] = GameObject.Find("Character Appearance");
        panelsGO[8] = GameObject.Find("Job Prefrences");
        for (int index = 0; index < panelsGO.Length; index++)
        {
            if (panelsGO[index].name == "Main Menu Panel")
            {
            }
            else
            {
                panelsGO[index].transform.position = new Vector3(0, 100, 0);
            }
        }
    }

    public void SendClientPlayerPrefs()
    {
        //uLink.Network.RPC();   
        //uLink.Network.loginData()
    }

    #region Character Editor
    private void CharEditorOnSaveClicked()
    {
        
                foreach (Transform childTransform in anchor.transform)
                {
                    if (childTransform.name == "Character Editor Panel")
                    {
                        foreach (Transform charEditorChildTransform in childTransform)
                        {
                            if (charEditorChildTransform.name == "Name")
                            {
                                foreach (Transform nameChildTransform in charEditorChildTransform)
                                {
                                    if (nameChildTransform.name == "Label")
                                    {
                                        loginDataHolder.GetComponent<StoredPlayerPrefs>().name[clientPlayerSelected] =
                                            nameChildTransform.GetComponent<UILabel>().text;
                                        break;
                                    }
                                }
                                break;
                            }
                            if (charEditorChildTransform.name == "Gender")
                            {
                                foreach (Transform nameChildTransform in charEditorChildTransform)
                                {
                                    if (nameChildTransform.name == "Label")
                                    {
                                        loginDataHolder.GetComponent<StoredPlayerPrefs>().gender[clientPlayerSelected] =
                                            nameChildTransform.GetComponent<UILabel>().text;
                                        break;
                                    }
                                }
                                break;
                            }
                            if (charEditorChildTransform.name == "Age")
                            {
                                foreach (Transform nameChildTransform in charEditorChildTransform)
                                {
                                    if (nameChildTransform.name == "Label")
                                    {
                                        loginDataHolder.GetComponent<StoredPlayerPrefs>().age[clientPlayerSelected] =
                                            nameChildTransform.GetComponent<UILabel>().text;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
        panelsGO[1].transform.position = new Vector3(0, 100, 0);
        panelsGO[3].transform.position = new Vector3(0, 0, 0);
    }
    #endregion

    private void OnCreateLobbyClicked()
    {
        Application.LoadLevel("ServerMainGame");
    }

    private void OnJoinLobbyClicked()
    {
       
       // loginDataHolder.AddComponent()
        //GameObject.DontDestroyOnLoad(loginDataHolder);
        Application.LoadLevel("ClientMainGame");
    }

    private void OptionsOnReturnToMenuClicked()
    {
        panelsGO[4].transform.position = new Vector3(0, 100, 0);
        panelsGO[3].transform.position = new Vector3(0, 0, 0);
    }


    #region Physical Appearance
    private void PhysicalAppearanceOnSaveClicked()
    {
        foreach (Transform childTransform in anchor.transform)
        {
            if (childTransform.name == "Character Appearance")
            {
                foreach (Transform charEditorChildTransform in childTransform)
                {
                    if (charEditorChildTransform.name == "Facial Hair")
                    {
                        foreach (Transform nameChildTransform in charEditorChildTransform)
                        {
                            if (nameChildTransform.name == "Choice")
                            {
                                loginDataHolder.GetComponent<StoredPlayerPrefs>().facialHair[clientPlayerSelected] =
                                    nameChildTransform.GetComponent<UILabel>().text;
                                break;
                            }
                        }
                        break;
                    }
                    if (charEditorChildTransform.name == "Hair Style")
                    {
                        foreach (Transform nameChildTransform in charEditorChildTransform)
                        {
                            if (nameChildTransform.name == "Choice")
                            {
                                loginDataHolder.GetComponent<StoredPlayerPrefs>().hair[clientPlayerSelected] =
                                    nameChildTransform.GetComponent<UILabel>().text;
                                break;
                            }
                        }
                        break;
                    }
                    if (charEditorChildTransform.name == "Underwear")
                    {
                        foreach (Transform nameChildTransform in charEditorChildTransform)
                        {
                            if (nameChildTransform.name == "Choice")
                            {
                                loginDataHolder.GetComponent<StoredPlayerPrefs>().underwear[clientPlayerSelected] =
                                    nameChildTransform.GetComponent<UILabel>().text;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        panelsGO[7].transform.position = new Vector3(0, 100, 0);
        panelsGO[1].transform.position = new Vector3(0, 0, 0);
    }
    #endregion
    private void VideoSettingsOnSaveClicked()
    {
        panelsGO[5].transform.position = new Vector3(0, 100, 0);
        panelsGO[4].transform.position = new Vector3(0, 0, 0);
    }

    private void SoundSettingsOnSaveClicked()
    {
        panelsGO[6].transform.position = new Vector3(0, 100, 0);
        panelsGO[4].transform.position = new Vector3(0, 0, 0);
    }

    private void OnPhysicalAppearanceClicked()
    {
        panelsGO[1].transform.position = new Vector3(0, 100, 0);
        panelsGO[7].transform.position = new Vector3(0, 0, 0);
    }

    private void GameSettingsOnSaveClicked()
    {
        panelsGO[2].transform.position = new Vector3(0, 100, 0);
        panelsGO[4].transform.position = new Vector3(0, 0, 0);
    }

    private void GameSettingsOnUndoClicked()
    {
        panelsGO[2].transform.position = new Vector3(0, 100, 0);
        panelsGO[4].transform.position = new Vector3(0, 0, 0);
    }

    private void OnCharacterEditorClicked()
    {
        panelsGO[3].transform.position = new Vector3(0, 100, 0);
        panelsGO[1].transform.position = new Vector3(0, 0, 0);
    }

    private void OnOptionsClicked()
    {
        panelsGO[3].transform.position = new Vector3(0, 100, 0);
        panelsGO[4].transform.position = new Vector3(0, 0, 0);
    }

    private void OnBrightnessSlider()
    {
        float brightness = GameObject.Find("Brightness Slider").GetComponent<UIScrollBar>().scrollValue;
        print(brightness);
        RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1.0f);
    }

    private void OnGameOptionsClicked()
    {
        panelsGO[4].transform.position = new Vector3(0, 100, 0);
        panelsGO[2].transform.position = new Vector3(0, 0, 0);
    }

    private void OnVideoOptionsClicked()
    {
        panelsGO[4].transform.position = new Vector3(0, 100, 0);
        panelsGO[5].transform.position = new Vector3(0, 0, 0);
    }

    private void OnSoundOptionsClicked()
    {
        panelsGO[4].transform.position = new Vector3(0, 100, 0);
        panelsGO[6].transform.position = new Vector3(0, 0, 0);
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }

    private void Update()
    {
        //float brightness = GameObject.Find("Brightness Slider").GetComponent<UIScrollBar>().scrollValue;
        //RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1.0f);
    }

    private void OnQualityChange(string quality)
    {
        int qualityInt = 3;
        foreach (string qualityValue in Enum.GetNames(typeof (qualityValues)))
        {
            if (qualityValue == quality)
            {
                qualityInt = (int) Enum.Parse(typeof (qualityValues), qualityValue);
            }
        }
        QualitySettings.SetQualityLevel(qualityInt);
    }

    private void OnResolutionChange(string resolution)
    {
        int breakLocation = resolution.IndexOf('x', 0);
        int horz = Convert.ToInt32(resolution.Substring(0, breakLocation));
        int vert = Convert.ToInt32(resolution.Substring(breakLocation + 1));
        Screen.SetResolution(horz, vert, false);
    }

    private enum qualityValues
    {
        Fastest,
        Fast,
        Good,
        High,
        Premium,
        Ultra
    }
}