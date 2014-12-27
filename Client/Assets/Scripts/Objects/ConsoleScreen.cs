using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ConsoleScreen : MonoBehaviour
{
    static string _inputString;
    bool status;
    Color[] blanks = new Color[63 * 63];
    Color blank = new Color(0, 0, 0, 0);
    private static UnityEngine.Object fontSheet;
    static Texture2D fontTextureSheet;
    //static int[][] charArray;
    static int[] charData;
    public static Dictionary<int, float[]> charDict = new Dictionary<int, float[]>();
    public float speed = 100.0F;
    public List<int> charDataList;

    static int numXQuads = 130;
    static int numYQuads = 33;
    static int vertStartIndexPos;
    static Vector3[] newVertices;
    static Vector2[] newUV;
    static int[] newIndices;
    static Vector3[] normals;
    static Quaternion rotation;

    static int consoleLineNumber = 0;

    // Use this for initialization
    [RPC]
    protected void Start()
    {

        Online();
        
        newVertices = new Vector3[numXQuads * numYQuads * 4];
        newUV = new Vector2[numXQuads * numYQuads * 4];
        newIndices = new int[numXQuads * numYQuads * 6];
        normals = new Vector3[newVertices.Length];
        rotation = Quaternion.AngleAxis(Time.deltaTime * speed, Vector3.up);
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
                newVertices[vidx++] = new Vector3(4 * ((charSpacingX * qx) - charDimX) / 9, 9 * ((charSpacingY * qy) - charDimY) / 9, 0.0f);
                newVertices[vidx++] = new Vector3(4 * ((charSpacingX * qx) + charDimX) / 9, 9 * ((charSpacingY * qy) - charDimY) / 9, 0.0f);
                newVertices[vidx++] = new Vector3(4 * ((charSpacingX * qx) + charDimX) / 9, 9 * ((charSpacingY * qy) + charDimY) / 9, 0.0f);
                newVertices[vidx++] = new Vector3(4 * ((charSpacingX * qx) - charDimX) / 9, 9 * ((charSpacingY * qy) + charDimY) / 9, 0.0f);

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
        //charArray = new int[charCount * charCount][];
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
                List<float> charDataList = new List<float>();
                minY = ((15 - i) + 0.0125f) / sheetSizeCharacters;
                maxY = (minY - 0.0125f) + (height / sheetSizeCharacters);
                minX = (j + 0.25f) / sheetSizeCharacters;
                maxX = minX + ((width - 0.28f) / sheetSizeCharacters);

                charDataList.Add(minX);
                charDataList.Add(minY);
                charDataList.Add(maxX);
                charDataList.Add(maxY);
                float[] charDataArray = charDataList.ToArray();

                ConsoleScreen.charDict.Add(index, charDataArray);
                index++;
            }
        }


        string powerOnString = "Welcome to CentOS(C)2013. This is a CSharp interpeter for in-game code writing!";

        float uVstartX;
        float uVstartY;
        float uVendX;
        float uVendY;
        float[] keyCoord;
        vertStartIndexPos = 0;
        foreach (char c in powerOnString)//Begins writing characters to the screen
        {
            keyint = System.Convert.ToInt32(c);
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
            if (vertStartIndexPos % (numXQuads * 4) == 0)
            {
                consoleLineNumber++;//Pseudo increment line number;
            }
        }


        keyint = 0;
        charDict.TryGetValue(keyint, out keyCoord);
        uVstartX = keyCoord[0];
        uVstartY = keyCoord[1];
        uVendX = keyCoord[2];
        uVendY = keyCoord[3];
        for (int indexWrite = vertStartIndexPos; indexWrite < newUV.Length; indexWrite += 4) //Begins blanking the rest of the screen
        {
            newUV[indexWrite + 2] = new Vector2(uVendX, uVstartY);
            newUV[indexWrite + 1] = new Vector2(uVendX, uVendY);
            newUV[indexWrite + 3] = new Vector2(uVstartX, uVstartY);
            newUV[indexWrite + 0] = new Vector2(uVstartX, uVendY);
            //indexWrite += 4;
        }
        vertStartIndexPos = (numXQuads * 4) * 2 + consoleLineNumber * (numXQuads * 4);//2 new lines after intro text;
        consoleLineNumber += 2;//Pseudo line counting

        GameObject consoleObject = new GameObject("Console");
        consoleObject.AddComponent("MeshFilter");
        consoleObject.AddComponent("MeshRenderer");
        //consoleObject.AddComponent("MeshCollider");
        normals = consoleObject.GetComponent<MeshFilter>().mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = rotation * normals[i];
        }
        consoleObject.GetComponent<MeshFilter>().mesh.normals = normals;
        consoleObject.GetComponent<MeshRenderer>().material = consoleMat;
        Mesh mesh = consoleObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newIndices;

        consoleObject.transform.localScale += new Vector3(-0.75f, -0.75f, -0.75f);
        consoleObject.transform.position = new Vector3(.5f, 112.5f, 0);
        //consoleObject.transform.Rotate(0f, 0f, 180f, Space.Self);
        consoleObject.transform.rotation = new Quaternion(0,0,180,0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        consoleObject.AddComponent<BoxCollider>();
        var boxCollider = consoleObject.GetComponent<BoxCollider>() as BoxCollider;
        boxCollider.size += new Vector3(0, 0, 1);

        consoleObject.AddComponent("CSharpInterpreter");
    }

    private string _oldInputString;
    static int keyint;
    static int previousKeyInt;
    static Texture2D keyTexture;
    public int tick = 0;
    private Material consoleMat;
    private Material tempConsoleMat;
    private Texture2D consoleTexture;
    const int startPosX = 65;
    const int startPosY = 775;
    static uint positionX = 0;
    static int positionY = 0;
    const int scale = 56 / 30;
    public static bool pressedE = false;
    public static string consoleString;

    public static void WriteExternalString(string inString)
    {
        positionX = 0;
        inString = inString.Trim();
        Texture2D consoleText = new Texture2D(0, 1024);
        //Material consoleMat = new Material(Shader.Find("Diffuse"));
        consoleText = (Texture2D)GameObject.Find("Console").gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        positionY++;
        //inString = ">>>" + inString.Trim();
        float uVstartX;
        float uVstartY;
        float uVendX;
        float uVendY;
        float[] keyCoord;
        if (inString != string.Empty)
            foreach (char c in inString)//Begins writing characters to the screen
            {
                Vector2[] _newUV = new Vector2[newUV.Length];
                keyint = System.Convert.ToInt32(c);
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
                if (vertStartIndexPos % (numXQuads * 4) == 0)
                {
                    consoleLineNumber++;
                }
            }
        vertStartIndexPos = (numXQuads * 4) * 1 + consoleLineNumber * (numXQuads * 4);//1 new line(s) after write text;
        consoleLineNumber += 2;//Pseudo line counting
        GameObject consoleObject = GameObject.Find("Console");
        Mesh mesh = consoleObject.GetComponent<MeshFilter>().mesh;
        //mesh.Clear();
        mesh.uv = newUV;


    }

    // Update is called once per frame
    void Update()
    {
        if (pressedE)
        {
            float uVstartX;
            float uVstartY;
            float uVendX;
            float uVendY;
            float[] keyCoord;

            //vertStartIndexPos = 0;
            //PlayerInput.DisableMotor();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
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
                foreach (char c in inString)//Begins writing characters to the screen
                {
                    keyint = System.Convert.ToInt32(c);
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
