#region License

// // MainMenuUI.cs
// //  
// //  Author:
// //        <colin.p.swensonh@gmail.com>
// // 
// //  Copyright (c) 2013 swensonhcp
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private readonly GameObject[] panelsGO = new GameObject[9];

    private void Start()
    {
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

    private void OnCreateLobbyClicked()
    {
        Application.LoadLevel("ServerMainGame");
    }

    private void OnJoinLobbyClicked()
    {
        Application.LoadLevel("ClientMainGame");
    }

    private void OptionsOnReturnToMenuClicked()
    {
        panelsGO[4].transform.position = new Vector3(0, 100, 0);
        panelsGO[3].transform.position = new Vector3(0, 0, 0);
    }

    private void CharEditorOnSaveClicked()
    {
        panelsGO[1].transform.position = new Vector3(0, 100, 0);
        panelsGO[3].transform.position = new Vector3(0, 0, 0);
    }

    private void PhysicalAppearanceOnSaveClicked()
    {
        panelsGO[7].transform.position = new Vector3(0, 100, 0);
        panelsGO[1].transform.position = new Vector3(0, 0, 0);
    }

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
        //float brightness = GameObject.Find("Brightness Slider").GetComponent<UIScrollBar>().scrollValue;
        //print(brightness);
        //RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1.0f);
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