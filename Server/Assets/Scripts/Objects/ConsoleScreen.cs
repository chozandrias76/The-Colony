#region License

// // ConsoleScreen.cs
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
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ConsoleScreen : MonoBehaviour
{
    private const int startPosX = 65;
    private const int startPosY = 775;
    private const int scale = 56/30;
    //private static string _inputString;
    //private static Object fontSheet;
    private static Texture2D fontTextureSheet;
    //private static int[] charData;
    public static Dictionary<int, float[]> charDict = new Dictionary<int, float[]>();

    private static int numXQuads = 130;
    private static int numYQuads = 33;
    private static int vertStartIndexPos;
    private static Vector3[] newVertices;
    private static Vector2[] newUV;
    private static int[] newIndices;
    private static Vector3[] normals;
    private static Quaternion rotation;

    private static int consoleLineNumber;

    // Use this for initialization
    private static int keyint;
    private static int previousKeyInt = 0;
    //private static Texture2D keyTexture;
    //private static uint positionX = 0;
    private static int positionY;
    public static bool currentlyUsing = false;
    public static string consoleString;
    //private string _oldInputString;
    //private Color blank = new Color(0, 0, 0, 0);
    //private Color[] blanks = new Color[63*63];
    public List<int> charDataList;
    private Material consoleMat;
    //private Texture2D consoleTexture;
    public float speed = 100.0F;
    //private bool status;
    //private Material tempConsoleMat;
    public BaseTerminal terminalHookedTo = new BaseTerminal();
    public int tick = 0;

    protected void Start()
    {
        Online();

        newVertices = new Vector3[numXQuads*numYQuads*4];
        newUV = new Vector2[numXQuads*numYQuads*4];
        newIndices = new int[numXQuads*numYQuads*6];
        normals = new Vector3[newVertices.Length];
        rotation = Quaternion.AngleAxis(Time.deltaTime*speed, Vector3.up);
        int iidx = 0;
        int vidx = 0;
        int uvidx = 0;
        float charDimX = 0.05f;
        float charDimY = 0.05f;
        float charSpacingX = 0.1f;
        float charSpacingY = 0.1f;

        for (int qy = 0; qy < numYQuads; qy++)
        {
            for (int qx = 0; qx < numXQuads; qx++)
            {
                int vbase = vidx;
                newVertices[vidx++] = new Vector3(4*((charSpacingX*qx) - charDimX)/9, 9*((charSpacingY*qy) - charDimY)/9,
                    0.0f);
                newVertices[vidx++] = new Vector3(4*((charSpacingX*qx) + charDimX)/9, 9*((charSpacingY*qy) - charDimY)/9,
                    0.0f);
                newVertices[vidx++] = new Vector3(4*((charSpacingX*qx) + charDimX)/9, 9*((charSpacingY*qy) + charDimY)/9,
                    0.0f);
                newVertices[vidx++] = new Vector3(4*((charSpacingX*qx) - charDimX)/9, 9*((charSpacingY*qy) + charDimY)/9,
                    0.0f);

                newUV[uvidx++] = new Vector2(0, 1);
                newUV[uvidx++] = new Vector2(1, 1);
                newUV[uvidx++] = new Vector2(1, 0);
                newUV[uvidx++] = new Vector2(0, 0);

                newIndices[iidx++] = vbase + 0;
                newIndices[iidx++] = vbase + 1;
                newIndices[iidx++] = vbase + 2;
                newIndices[iidx++] = vbase + 2;
                newIndices[iidx++] = vbase + 3;
                newIndices[iidx++] = vbase + 0;
            }
        }
        int charCount = 16;
        float width = 1;
        float height = 1;
        int sheetSizeCharacters = charCount;

        //<summary>
        //The following loops creates the dictionary that sets the character locations and their size
        //Refractored to work with current UV map (0-1) positions
        //</summary>
        int index = 0;
        float minX;
        float maxX;
        float minY;
        float maxY;
        for (float i = 0; i < 16; i++)
        {
            for (float j = 0; j < 16; j++)
            {
                var charDataList = new List<float>();
                var charDataArray = new float[4];

                minY = ((15 - i) + 0.0125f)/sheetSizeCharacters;
                maxY = (minY - 0.0125f) + (height/sheetSizeCharacters);
                minX = (j + 0.25f)/sheetSizeCharacters;
                maxX = minX + ((width - 0.28f)/sheetSizeCharacters);

                charDataList.Add(minX);
                charDataList.Add(minY);
                charDataList.Add(maxX);
                charDataList.Add(maxY);
                charDataArray = charDataList.ToArray();
                try
                {
                    charDict.Add(index, charDataArray);
                    index++;
                }
                catch (ArgumentException)
                {
                    Debug.LogError("A key already exists with value " + index +
                                   "Maybe you have more than 1 console script in your scene");
                }
            }
        }


        string powerOnString = "Welcome to _OS(C)2013.";
		WriteExternalString(powerOnString);
        float uVstartX;
        float uVstartY;
        float uVendX;
        float uVendY;
       float[] keyCoord;
        vertStartIndexPos = 0;
//        foreach (char c in powerOnString) //Begins writing characters to the screen
//        {
//            keyint = Convert.ToInt32(c);
//            charDict.TryGetValue(keyint, out keyCoord);
//            uVstartX = keyCoord[0];
//            uVstartY = keyCoord[1];
//            uVendX = keyCoord[2];
//            uVendY = keyCoord[3];
//
//            newUV[vertStartIndexPos + 2] = new Vector2(uVendX, uVstartY);
//            newUV[vertStartIndexPos + 1] = new Vector2(uVendX, uVendY);
//            newUV[vertStartIndexPos + 3] = new Vector2(uVstartX, uVstartY);
//            newUV[vertStartIndexPos + 0] = new Vector2(uVstartX, uVendY);
//            vertStartIndexPos += 4;
//            if (vertStartIndexPos%(numXQuads*4) == 0)
//            {
//                consoleLineNumber++; //Pseudo increment line number;
//            }
//        }
        if (terminalHookedTo.isaTerminal)
        {
//            switch (terminalHookedTo._terminalType)
//            {
//                case 0:
//
//                    //ConsoleScreen.WriteExternalString("This is a general use terminal. Please insert your ID badge to begin");
//                    break;
//            }
        }


        keyint = 0;
        charDict.TryGetValue(keyint, out keyCoord);
        uVstartX = keyCoord[0];
        uVstartY = keyCoord[1];
        uVendX = keyCoord[2];
        uVendY = keyCoord[3];
        for (int indexWrite = powerOnString.Length*4; indexWrite < newUV.Length; indexWrite += 4)
            //Begins blanking the rest of the screen
        {
            newUV[indexWrite + 2] = new Vector2(uVendX, uVstartY);
            newUV[indexWrite + 1] = new Vector2(uVendX, uVendY);
            newUV[indexWrite + 3] = new Vector2(uVstartX, uVstartY);
            newUV[indexWrite + 0] = new Vector2(uVstartX, uVendY);
            //indexWrite += 4;
        }
        vertStartIndexPos = (numXQuads*4)*2 + consoleLineNumber*(numXQuads*4); //2 new lines after intro text;
        consoleLineNumber += 2; //Pseudo line counting

        var consoleObject = new GameObject("Console");
        consoleObject.AddComponent("MeshFilter");
        consoleObject.AddComponent("MeshRenderer");
        //consoleObject.AddComponent("MeshCollider");
        normals = consoleObject.GetComponent<MeshFilter>().mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = rotation*normals[i];
        }
        consoleObject.GetComponent<MeshFilter>().mesh.normals = normals;
        consoleObject.GetComponent<MeshRenderer>().material = consoleMat;
        Mesh mesh = consoleObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newIndices;

        //consoleObject.transform.localScale += new Vector3(-0.75f, -0.75f, -0.75f);
        //consoleObject.transform.position = new Vector3(.5f, 112.5f, 0);
        //consoleObject.transform.Rotate(0f, 0f, 180f, Space.Self);
        consoleObject.transform.rotation = new Quaternion(0, 0, 180, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        consoleObject.AddComponent<BoxCollider>();
        var boxCollider = consoleObject.GetComponent<BoxCollider>();
        boxCollider.size += new Vector3(0, 0, 1);

        consoleObject.AddComponent("CSharpInterpreter");
    }
    //Texture2D consoleText = new Texture2D(0, 1024);

    public void WriteExternalString(string inString)
    {
        //positionX = 0;
        inString = inString.Trim();
        //var consoleText = new Texture2D(0, 1024);
        //consoleText =
            //(Texture2D) GameObject.Find("Console").gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        positionY++;
        float uVstartX;
        float uVstartY;
        float uVendX;
        float uVendY;
        float[] keyCoord;
        if (inString != string.Empty)
            foreach (char c in inString) //Begins writing characters to the screen
            {
                //var _newUV = new Vector2[newUV.Length];
                keyint = Convert.ToInt32(c);
                charDict.TryGetValue(keyint, out keyCoord);
                uVstartX = keyCoord[0];
                uVstartY = keyCoord[1];
                uVendX = keyCoord[2];
                uVendY = keyCoord[3];

                newUV[vertStartIndexPos + 2] = new Vector2(uVendX, uVstartY);
                newUV[vertStartIndexPos + 1] = new Vector2(uVendX, uVendY);
                newUV[vertStartIndexPos + 3] = new Vector2(uVstartX, uVstartY);
                newUV[vertStartIndexPos + 0] = new Vector2(uVstartX, uVendY);
                vertStartIndexPos += 4;
                if (vertStartIndexPos%(numXQuads*4) == 0)
                {
                    consoleLineNumber++;
                }
            }
        vertStartIndexPos = (numXQuads*4)*1 + consoleLineNumber*(numXQuads*4); //1 new line(s) after write text;
        consoleLineNumber += 2; //Pseudo line counting
        GameObject consoleObject = GameObject.Find("Console");
        Mesh mesh = consoleObject.GetComponent<MeshFilter>().mesh;
        //mesh.Clear();
        mesh.uv = newUV;
    }

    public void ClearScreen()
    {
        //positionX = 0;
        //consoleText =
            //(Texture2D) GameObject.Find("Console").gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        positionY = 0;
        float uVstartX;
        float uVstartY;
        float uVendX;
        float uVendY;
        //float[] keyCoord;
        for (int i = 0; i <= newUV.Length; i++) //Begins writing characters to the screen
        {
            //var _newUV = new Vector2[newUV.Length];
            uVstartX = 0;
            uVstartY = 0;
            uVendX = 0;
            uVendY = 0;

            newUV[vertStartIndexPos + 2] = new Vector2(uVendX, uVstartY);
            newUV[vertStartIndexPos + 1] = new Vector2(uVendX, uVendY);
            newUV[vertStartIndexPos + 3] = new Vector2(uVstartX, uVstartY);
            newUV[vertStartIndexPos + 0] = new Vector2(uVstartX, uVendY);
            vertStartIndexPos += 4;
            if (vertStartIndexPos%(numXQuads*4) == 0)
            {
                consoleLineNumber++;
            }
        }
        vertStartIndexPos = (numXQuads*4)*1 + consoleLineNumber*(numXQuads*4); //1 new line(s) after write text;
        consoleLineNumber += 2; //Pseudo line counting
        GameObject consoleObject = GameObject.Find("Console");
        Mesh mesh = consoleObject.GetComponent<MeshFilter>().mesh;
        mesh.uv = newUV;
        //positionX = 0;
        positionY = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentlyUsing)
        {
            float uVstartX;
            float uVstartY;
            float uVendX;
            float uVendY;
            float[] keyCoord;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
				currentlyUsing = false;
                //GameObject.Find("playerPrefab(Clone)").GetComponent<CharacterMotor>().enabled = false;
            }
            GameObject consoleObject = GameObject.Find("Console");
            Mesh mesh = consoleObject.GetComponent<MeshFilter>().mesh;
            if (vertStartIndexPos%(numXQuads*4) == 0)
            {
                keyint = 62;
                if (vertStartIndexPos + 8 < newUV.Length)
                {
                    charDict.TryGetValue(keyint, out keyCoord);
                    uVstartX = keyCoord[0];
                    uVstartY = keyCoord[1];
                    uVendX = keyCoord[2];
                    uVendY = keyCoord[3];

                    newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                    newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                    newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                    newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);
                    vertStartIndexPos += 4;
                    newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                    newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                    newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                    newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);
                    vertStartIndexPos += 4;
                }
            }

            string inString = Input.inputString;
            if (inString != string.Empty)
                foreach (char c in inString) //Begins writing characters to the screen
                {
                    keyint = Convert.ToInt32(c);
                    if (vertStartIndexPos + 4 < newUV.Length)
                    {
                        if (keyint > 32 && keyint != 127)
                        {
                            charDict.TryGetValue(keyint, out keyCoord);
                            uVstartX = keyCoord[0];
                            uVstartY = keyCoord[1];
                            uVendX = keyCoord[2];
                            uVendY = keyCoord[3];

                            newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                            newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                            newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                            newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);
                            vertStartIndexPos += 4;

                            consoleString += c;

                            if (vertStartIndexPos%(numXQuads*4) == 0)
                            {
                                consoleLineNumber++;
                            }
                            mesh.uv = newUV;
                        }
                        else if (keyint == 32) //For Space inputs
                        {
                            keyint = 0;
                            charDict.TryGetValue(keyint, out keyCoord);
                            uVstartX = keyCoord[0];
                            uVstartY = keyCoord[1];
                            uVendX = keyCoord[2];
                            uVendY = keyCoord[3];

                            newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                            newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                            newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                            newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);
                            vertStartIndexPos += 4;

                            consoleString += c;
                            mesh.uv = newUV;
                        }
                        else if (keyint == 13 && previousKeyInt == 17) //Press return and shift for new line
                        {
                            vertStartIndexPos = (numXQuads*4)*1 + consoleLineNumber*(numXQuads*4);
                            consoleLineNumber++;
                        }
                        else if (keyint == 13) //Execute the Input
                        {
                            vertStartIndexPos = (numXQuads*4)*1 + consoleLineNumber*(numXQuads*4);
                            consoleLineNumber++;
                            consoleString.Trim();
                            CSharpInterpreter.OnExecuteInput();
                            consoleString = string.Empty;
                        }
                        else if (keyint == 8 || keyint == 16 || keyint == 127) //Press a delete button(return/delete)
                        {
                            keyint = 0;
                            charDict.TryGetValue(keyint, out keyCoord);
                            uVstartX = keyCoord[0];
                            uVstartY = keyCoord[1];
                            uVendX = keyCoord[2];
                            uVendY = keyCoord[3];
                            newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                            newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                            newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                            newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);
                            vertStartIndexPos += -4;
                            newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                            newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                            newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                            newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);

                            consoleString = consoleString.Substring(0, consoleString.Length - 1);
                            mesh.uv = newUV;
                        }
                    }
                    inString = string.Empty;
                }
            else if (inString == string.Empty)
            {
                if (vertStartIndexPos + 3 < newUV.Length)
                {
                    if (tick%25 == 0)
                    {
                        keyint = 166;
                        charDict.TryGetValue(keyint, out keyCoord);
                        uVstartX = keyCoord[0];
                        uVstartY = keyCoord[1];
                        uVendX = keyCoord[2];
                        uVendY = keyCoord[3];
                        newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                        newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                        newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                        newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);

                        mesh.uv = newUV;
                    }
                    else if (tick%74 == 0)
                    {
                        keyint = 0;
                        charDict.TryGetValue(keyint, out keyCoord);
                        uVstartX = keyCoord[0];
                        uVstartY = keyCoord[1];
                        uVendX = keyCoord[2];
                        uVendY = keyCoord[3];
                        newUV[vertStartIndexPos + 2].Set(uVendX, uVstartY);
                        newUV[vertStartIndexPos + 1].Set(uVendX, uVendY);
                        newUV[vertStartIndexPos + 3].Set(uVstartX, uVstartY);
                        newUV[vertStartIndexPos + 0].Set(uVstartX, uVendY);
                        mesh.uv = newUV;
                    }
                }
            }

            tick++;
        }
    }

    public void Online()
    {
        fontTextureSheet = Resources.Load("FontSpriteSheet02") as Texture2D;
        consoleMat = new Material(Shader.Find("Unlit/Texture"));
        consoleMat.mainTexture = fontTextureSheet;
        consoleMat.SetTexture(fontTextureSheet.ToString(), fontTextureSheet);
    }
}