//-----------------------------------------------------------------------
// <summary>
// CSI: A simple C# interpreter
// </summary>
// <copyright file="CSharpInterpreter.cs" company="Tiaan.com">
//   Copyright (c) 2010 Tiaan Geldenhuys
//
//   Permission is hereby granted, free of charge, to any person
//   obtaining a copy of this software and associated documentation
//   files (the "Software"), to deal in the Software without
//   restriction, including without limitation the rights to use,
//   copy, modify, merge, publish, distribute, sublicense, and/or
//   sell copies of the Software, and to permit persons to whom the
//   Software is furnished to do so, subject to the following
//   conditions:
//
//   The above copyright notice and this permission notice shall be
//   included in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//   EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//   OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//   NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//   HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//   WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//   FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using CSI;
using Microsoft.CSharp;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

/// <summary>
///     Implements a hosting environment for the C# Interpreter as a Component that can be attached to a GameObject in
///     Unity 3D.
/// </summary>
////[ExecuteInEditMode]
public sealed class CSharpInterpreter : MonoBehaviour, IConsole
{
    public delegate void Action();

    public const string Version = "1.0.0.1";

    private const string PromptStart = ">>>";
    private const string PromptExtra = "...";
    private const string PromptBlank = "----";
    private const string InputTextBoxName = "CsiInputTextBox";

    private const string EmptyCacheSlot = "{7ewyutPloEyVoQ0lPYfsWw}";
        // Cannot use null, since Unity resets it to empty during rebuilds

    private static Interpreter csharpEngine;
    public float bottomMargin;
    private int currentHistoryIndex;

    public Object includeAsset;
    public string includeFile;
    private Interpreter.InputHandler inputHandler;
    private Vector2 inputScrollPosition;
    private string inputText;
    private List<string> inputTextCache;
    private List<string> inputTextHistory = new List<string>();
    public float leftMargin;
    public int maxHistorySize;
    public int maxOutputLineCount;
    public int maxOutputLineWidth;
    public int maxOutputSize;
    private Vector2 outputScrollPosition;
    private StringBuilder outputStringBuilder;
    private string outputText;
    private string promptText;
    public Object queuedAsset;
    public float rightMargin;
    public bool showInteractiveGUI;
    public bool showOutputAsEditorSelection;
    public bool showOutputText;
    public bool showTooltipText;
    public float splitterFraction;
    public int toolboxWidth;
    public float topMargin;
    private Assembly unityEditorAssembly;


    /// <summary>
    ///     Gets the C# interpreter instance that is currently active.
    /// </summary>
    /// <value>The current CSI instance.</value>
    public static CSharpInterpreter Current
    {
        get { return (Interpreter.Console as CSharpInterpreter); }
    }

    public bool IsEditorAvailable()
    {
        return (unityEditorAssembly != null);
    }

    private static string GetDefaultIncludeFilename()
    {
        string filename;
        try
        {
            filename =
                new StackTrace(true).GetFrame(0).GetFileName();
            filename = Path.Combine(
                Path.GetDirectoryName(filename),
                Path.GetFileNameWithoutExtension(filename) + "_Include.txt");
            if ((!File.Exists(filename)) ||
                string.IsNullOrEmpty(Application.dataPath))
            {
                return null;
            }
        }
        catch
        {
            return null;
        }

        filename = filename.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        string dataPath = Application.dataPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        if ((filename.StartsWith(dataPath, StringComparison.OrdinalIgnoreCase)))
        {
            filename = filename.Substring(dataPath.Length).TrimStart(Path.DirectorySeparatorChar);
        }

        return filename;
    }

    //public CSharpInterpreter()
    private void Awake()
    {
        Reset();
    }

    /// <summary>
    ///     Performs one-time initialization of this instance; called by Unity.
    /// </summary>
    private void Start()
    {
        promptText = string.Empty;
        inputText = string.Empty;
        outputText = string.Empty;
        outputScrollPosition = Vector2.zero;
        inputScrollPosition = Vector2.zero;

        Reinitialize();
    }

    private void Reset()
    {
        includeFile = GetDefaultIncludeFilename();
        includeAsset = null;
        queuedAsset = null;
        maxHistorySize = 100;
        maxOutputSize = 15250;
            // Seems to be okay to avoid errors like these from the Debug inspector: "Optimized GUI Block text buffer too large. Not appending further text."
        showInteractiveGUI = true;
        showOutputText = true;
        showOutputAsEditorSelection = true;
        showTooltipText = true;
        leftMargin = float.NaN;
        topMargin = float.NaN;
        rightMargin = float.NaN;
        bottomMargin = float.NaN;
        toolboxWidth = 45;
        splitterFraction = 75f;
        maxOutputLineWidth = 32000;
        maxOutputLineCount = 80;
        currentHistoryIndex = (inputTextHistory == null)
            ? 0
            : Math.Max(0, inputTextHistory.Count - 1);
    }

    /// <summary>
    ///     Performs initialization of this instance, which can be called at startup or in play mode when the Unity Editor
    ///     rebuilds scripts.
    /// </summary>
    private bool Reinitialize()
    {
        if ((csharpEngine != null /* Has an interpreter */) &&
            (Interpreter.Console != null /* Has a console */) &&
            (!ReferenceEquals(Interpreter.Console, this) /* Console is another object */))
        {
            Object otherUnityObject = null;
            if ((!(Interpreter.Console is Object) /* Not a Unity object */) ||
                (otherUnityObject = Interpreter.Console as Object))
            {
                enabled = false;
                if (otherUnityObject)
                {
                    Debug.LogWarning(
                        "Only one C# Interpreter may be created per scene; " +
                        "use the one on the object named: " + otherUnityObject.name, this);
                    if (otherUnityObject is Behaviour)
                    {
                        ((Behaviour) otherUnityObject).enabled = true;
                    }
                }
                else
                {
                    Debug.LogWarning(
                        "Only one C# Interpreter console may be created!", this);
                }

                return false; // Not initialized
            }
        }

        EnforceParameterLimits();
        if (string.IsNullOrEmpty(outputText))
        {
            outputText = "Welcome to CentOS. Please enter your C# commands below to compile using standard C# syntax";
        }
        else
        {
            outputText += string.Format("{0}[CSI reloaded and reset some data @ {1}]", Environment.NewLine,
                DateTime.Now.ToLongTimeString());
        }

        outputStringBuilder = new StringBuilder(outputText + Environment.NewLine);
        outputScrollPosition.y = Mathf.Infinity;

        if (inputTextCache == null)
        {
            inputTextCache = new List<string>(maxHistorySize + 1);
        }

        if (inputTextHistory == null)
        {
            inputTextHistory = new List<string>(maxHistorySize + 1);
            inputTextCache.Clear();
        }

        currentHistoryIndex = Math.Max(0, inputTextHistory.Count - 1);

        InitializeCompilerForUnity3D.RunOnce();
        csharpEngine = new Interpreter();
        csharpEngine.OnGetUnknownItem += OnGetUnknownItem;

        Interpreter.Console = this;
        string libraryPath = null;
        if (Application.isEditor)
        {
            // For Editor: Seach project's "Library\ScriptAssemblies" directory
            libraryPath = Path.GetDirectoryName(
                csharpEngine.FullExecutablePath());
        }
        else
        {
#if UNITY_2_6
    // Unity 2.6.1 Player: Seach "<ApplicationName>_Data"
            libraryPath = Application.dataPath;
#else // i.e., 3.0 and greater
            // Players of Unity 3.0.0 and 3.1.0: Seach "<ApplicationName>_Data\Managed"
            try
            {
                libraryPath = Path.GetDirectoryName(GetFullPathOfAssembly(typeof (int).Assembly));
            }
            catch
            {
                libraryPath = Path.Combine(Application.dataPath ?? string.Empty, "Managed");
            }
#endif
        }

        try
        {
            // Add DLLs from the project's "Library\ScriptAssemblies" directory
            if (!string.IsNullOrEmpty(libraryPath))
            {
                foreach (string reference in
#if UNITY_2_6
                    Directory.GetFiles(libraryPath, "Assembly - *.dll"))
#else // i.e., 3.0 and greater
                    Directory.GetFiles(libraryPath, "Assembly-*.dll"))
#endif
                {
                    // When using Unity 2.6.1 convention:
                    //  * "Assembly - CSharp.dll"
                    //  * "Assembly - CSharp - Editor.dll"
                    //  * "Assembly - CSharp - first pass.dll"
                    //  * "Assembly - UnityScript - first pass.dll"
                    // When using Unity 3.0.0 and 3.1.0 convention:
                    //  * "Assembly-CSharp.dll"
                    //  * "Assembly-CSharp-firstpass.dll"
                    //  * "Assembly-UnityScript-firstpass.dll"
                    csharpEngine.AddReference(reference);
                }
            }

            string includeFile = this.includeFile;
            if (!string.IsNullOrEmpty(includeFile))
            {
                string cachedFilename = includeFile;
                includeFile = ResolveFilename(includeFile);
                if (!csharpEngine.ReadIncludeFile(includeFile))
                {
                    ForceWarning(
                        "CSI include-file not loaded (" + cachedFilename + ")", this);
                }
            }

            string includeAssetName;
            string includeCode = GetAssetText(includeAsset, out includeAssetName);
            if ((!string.IsNullOrEmpty(includeCode)) &&
                (!csharpEngine.ReadIncludeCode(includeCode)))
            {
                ForceWarning(
                    "CSI include-asset not loaded: " + (includeAssetName ?? string.Empty), this);
            }

            Assembly unityEngineAssembly = null;
            string fullAssemblyPath = GetFullPathOfAssembly(
                typeof (GameObject).Assembly);
            if (File.Exists(fullAssemblyPath))
            {
                // Adds "UnityEngine.dll", or rather "UnityEngine-Debug.dll", for the 
                // Editor, which is located in the "...\Unity\Editor\Data\lib" directory.
                // However, this does not work for the Standalone Windows Player, which 
                // uses the same mechanism for UnityEngine as for UnityEditor (below).
                unityEngineAssembly = typeof (GameObject).Assembly;
            }

            // Add the Unity Editor's assembly only when available
            unityEditorAssembly = null;
            foreach (Assembly assembly in
                AppDomain.CurrentDomain.GetAssemblies())
            {
                if (unityEditorAssembly == null)
                {
                    try
                    {
                        if ((assembly.FullName.StartsWith("UnityEditor,", StringComparison.OrdinalIgnoreCase)) &&
                            (assembly.GetType("UnityEditor.EditorApplication") != null))
                        {
                            unityEditorAssembly = assembly;
                        }
                    }
                    catch
                    {
                        // Skip problematic assemblies
                    }
                }

                if (unityEngineAssembly == null)
                {
                    try
                    {
                        if (((assembly.FullName.StartsWith("UnityEngine,", StringComparison.OrdinalIgnoreCase)) ||
                             (assembly.FullName.StartsWith("UnityEngine-Debug,", StringComparison.OrdinalIgnoreCase))) &&
                            (assembly.GetType("UnityEngine.GameObject") != null))
                        {
                            unityEngineAssembly = assembly;
                        }
                    }
                    catch
                    {
                        // Skip problematic assemblies
                    }
                }

                if ((unityEditorAssembly != null) &&
                    (unityEngineAssembly != null))
                {
                    break;
                }
            }

            if (unityEngineAssembly != null)
            {
                // Include "UnityEngine.dll" or "UnityEngine-Debug.dll"
                string filename = GetFullPathOfAssembly(unityEngineAssembly);
#if !UNITY_2_6 // i.e., 3.0 and greater
                if (!File.Exists(filename))
                {
                    try
                    {
                        filename = GetFullPathOfAssembly(typeof (int).Assembly);
                        filename = Path.Combine(
                            Path.GetDirectoryName(filename) ?? string.Empty,
                            "UnityEngine.dll");
                    }
                    catch
                    {
                        filename = null;
                    }
                }
#endif

                if (File.Exists(filename))
                {
                    csharpEngine.AddReference(filename);
                    csharpEngine.AddNamespace("UnityEngine");
                }
                else
                {
                    unityEngineAssembly = null;
                }
            }

            if (unityEngineAssembly == null)
            {
                ForceWarning("UnityEngine is not referenced!");
            }

            if (unityEditorAssembly != null)
            {
                // Include "UnityEditor.dll"
                string filename =
                    GetFullPathOfAssembly(unityEditorAssembly);
                if (File.Exists(filename))
                {
                    csharpEngine.AddReference(filename);
                    csharpEngine.AddNamespace("UnityEditor");
                }
                else
                {
                    unityEditorAssembly = null;
                }
            }

            if ((unityEditorAssembly == null)
                && Application.isEditor)
            {
                Debug.LogWarning("UnityEditor is not referenced!");
            }

            PromptForInput(PromptStart);
            AddGlobal("csi", this);
            return true; // Initialized successfully
        }
        catch (IOException exception)
        {
            // Probably running in the web player without required rights
            Debug.LogError(
                "CSI failed to initialize (web player not supported): " + exception.Message, this);
            return false;
        }
    }

    private static void ForceWarning(string message)
    {
        ForceWarning(message, null /* context */);
    }

    private static void ForceWarning(string message, Object context)
    {
        if (Application.isEditor)
        {
            if (context == null)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.LogWarning(message, context);
            }
        }
        else
        {
            Utils.Print("Warning: " + message);
        }
    }

    private static string GetFullPathOfAssembly(Assembly assembly)
    {
        if (assembly == null)
        {
            return null;
        }

        string codeBase = assembly.CodeBase;
        if (string.IsNullOrEmpty(codeBase))
        {
            return null;
        }

        string filename = new Uri(codeBase).LocalPath;
        if (!File.Exists(filename))
        {
            string tempName = assembly.FullName ?? string.Empty;
            int index = tempName.IndexOf(',');
            if (index > 0)
            {
                tempName = Path.Combine(
                    Path.GetDirectoryName(filename) ?? string.Empty,
                    Path.ChangeExtension(tempName.Substring(0, index), ".dll"));
                if (File.Exists(tempName))
                {
                    filename = tempName;
                }
            }
        }

        return filename;
    }

    private static string GetAssetText(object asset, out string assetName)
    {
        assetName = null;
        string includeCode = null;
        if (asset != null)
        {
            // Handle any Unity object (dead or alive)
            if (asset is Object)
            {
                Object unityObject;
                if (unityObject = asset as Object)
                {
                    // Handle live Unity objects
                    assetName = unityObject.name;
                    if (unityObject is TextAsset)
                    {
                        includeCode = ((TextAsset) unityObject).text;
                    }
                }
            }
            else if (asset is string)
            {
                includeCode = (string) asset;
            }
        }

        return includeCode;
    }

    private void EnforceParameterLimits()
    {
        // Auto-size the window layout area, if needed...
        if (float.IsNaN(leftMargin))
        {
            leftMargin = 800f/Screen.width;
        }

        if (float.IsNaN(topMargin))
        {
            topMargin = 800f/Screen.height;
        }

        if (float.IsNaN(rightMargin))
        {
            rightMargin = 6500f/Screen.width;
        }

        if (float.IsNaN(bottomMargin))
        {
            bottomMargin = 800f/Screen.height;
        }

        // Clip parameters within bounds...
        if (maxHistorySize < 1)
        {
            maxHistorySize = 1;
        }
        else if (maxHistorySize > 9999)
        {
            maxHistorySize = 9999;
        }

        if (maxOutputSize < 2048)
        {
            maxOutputSize = 2048;
        }
        else if (maxOutputSize > 16380)
        {
            // Almost 16KB seems to be the upper limit of a Unity TextArea
            maxOutputSize = 16380;
        }

        if (splitterFraction < 15f)
        {
            splitterFraction = 15f;
        }
        else if (splitterFraction > 77.5f)
        {
            splitterFraction = 77.5f;
        }

        if (toolboxWidth < 35)
        {
            toolboxWidth = 35;
        }

        if (maxOutputLineWidth < 20)
        {
            maxOutputLineWidth = 20;
        }

        if (maxOutputLineCount < 3)
        {
            maxOutputLineCount = 3;
        }
    }

    private object OnGetUnknownItem(object key)
    {
        if (key is string)
        {
            var stringKey = (string) key;
            try
            {
                GameObject gameObject = GameObject.Find(stringKey);
                if (gameObject)
                {
                    return gameObject;
                }
            }
            catch
            {
                // Interpret any error as not finding the item
            }

            try
            {
                GameObject[] gameObjects =
                    GameObject.FindGameObjectsWithTag(stringKey);
                if ((gameObjects != null) &&
                    (gameObjects.Length > 0))
                {
                    return gameObjects;
                }
            }
            catch
            {
                // Interpret any error as not finding the items
            }

            try
            {
                Type type = Utils.GetType(stringKey);
                if (type != null)
                {
                    key = type;
                }
            }
            catch
            {
                // Ignore
            }
        }

        if (key is Type)
        {
            var typeKey = (Type) key;
            try
            {
                Object[] objects =
                    FindObjectsOfType(typeKey);
                if ((objects != null) &&
                    (objects.Length > 0))
                {
                    return objects;
                }
            }
            catch
            {
                // Interpret any error as not finding the item
            }
        }

        return null;
    }

    private static string ResolveFilename(string filename)
    {
        try
        {
            if (string.IsNullOrEmpty(filename))
            {
                return filename;
            }

            if (File.Exists(filename))
            {
                return Path.GetFullPath(filename);
            }

            filename = Path.Combine(Application.dataPath ?? string.Empty, filename);
            if (File.Exists(filename))
            {
                return filename;
            }
        }
        catch (IOException)
        {
            // Probably running in the web player without required rights
        }

        return null;
    }

    /// <summary>
    ///     Adds the specified global variable to the interpreter environment.
    /// </summary>
    public object AddGlobal(string name, object value)
    {
        csharpEngine.SetValue(name, value);
        return value;
    }

    public bool HasGlobal(string name)
    {
        return csharpEngine.VarTable.ContainsKey(name);
    }

    public bool RemoveGlobal(string name)
    {
        if (HasGlobal(name))
        {
            csharpEngine.VarTable.Remove(name);
            return true;
        }

        return false;
    }

    public void ClearOutput()
    {
        string outputText;
        ClearOutput(out outputText);
    }

    public void ClearOutput(out string outputText)
    {
        outputText = this.outputText;
        this.outputText = string.Empty;
        outputStringBuilder.Length = 0;
    }

    public string GetOutput()
    {
        return outputText;
    }

    public string[] GetHistory()
    {
        return inputTextHistory.ToArray();
    }

    public void ClearHistory()
    {
        string[] history;
        ClearHistory(out history);
    }

    public void ClearHistory(out string[] history)
    {
        history = GetHistory();
        inputTextHistory.Clear();
        inputTextCache.Clear();
        currentHistoryIndex = 0;
    }

    public object GetLastExecuteResult()
    {
        if ((csharpEngine != null) &&
            (csharpEngine.returnsValue))
        {
            return csharpEngine.VarTable["_"];
        }

        return null;
    }

    /// <summary>
    ///     Execute the specified code in the interpreter environment.
    /// </summary>
    public bool ExecuteCode(string inputText)
    {
        try
        {
            if (csharpEngine.ProcessLine(inputText))
            {
                PromptForInput((csharpEngine.BlockLevel > 0) ? PromptExtra : PromptStart);

                if (showOutputAsEditorSelection &&
                    IsEditorAvailable())
                {
                    try
                    {
                        object result = GetLastExecuteResult();
                        if (result != null)
                        {
                            Select(result);
                        }
                    }
                    catch
                    {
                        // Ignore error during result selection
                    }
                }

                return true;
            }
        }
        catch (Exception exception)
        {
            Interpreter.Console.Write("ERROR: " + exception.Message);
            ////CSI.Interpreter.Console.Write("ERROR: " + exception.ToString());
            Interpreter.Console.Write(Environment.NewLine);
            PromptForInput(PromptStart);
        }

        return false;
    }

    /// <summary>
    ///     Execute the specified file in the interpreter environment.
    /// </summary>
    public bool ExecuteFile(string inputFilename)
    {
        inputFilename = ResolveFilename(inputFilename);
        if (File.Exists(inputFilename))
        {
            string inputText = File.ReadAllText(inputFilename);
            return ExecuteCode(inputText);
        }

        return false;
    }

    public bool Select(object obj)
    {
        if ((obj == null) ||
            (!IsEditorAvailable()))
        {
            return false;
        }

        if (obj is GameObject)
        {
            return Select((GameObject) obj);
        }

        if (obj is Transform)
        {
            return Select((Transform) obj);
        }

        if (obj is IEnumerable<Object>)
        {
            return Select((IEnumerable<Object>) obj);
        }

        if (obj is Object)
        {
            return Select((Object) obj);
        }

        return false;
    }

    public bool Select(GameObject gameObject)
    {
        return Select("activeGameObject", gameObject);
    }

    public bool Select(Transform transform)
    {
        return Select("activeTransform", transform);
    }

    /// <summary>
    ///     Implements a helper method that can be used to execute a statement and hide the result from the interpreter output
    ///     windows.
    /// </summary>
    /// <param playerName="action">The action delegate to be executed.</param>
    public void Invoke(Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    public bool Select(Object unityObject)
    {
        return Select("activeObject", unityObject);
    }

    public bool Select(IEnumerable<Object> unityObjects)
    {
        if ((unityObjects == null) ||
            (!IsEditorAvailable()))
        {
            return false;
        }

        var objects =
            new List<Object>(unityObjects);
        if (objects.Count <= 0)
        {
            return false;
        }

        if ((objects.Count == 1) &&
            Select((object) objects[0]))
        {
            return true;
        }

        // For Component objects, select the containing game-object 
        // instead, so that the items would be highlighted in the editor
        Component component;
        GameObject gameObject;
        for (int index = objects.Count - 1; index >= 0; index--)
        {
            component = objects[index] as Component;
            if (!component)
            {
                continue;
            }

            gameObject = component.gameObject;
            if (!gameObject)
            {
                continue;
            }

            objects[index] = gameObject;
        }

        return Select("objects", objects.ToArray());
    }

    private bool Select(
        string selectionPropertyName, object selectionPropertyValue)
    {
        if ((selectionPropertyValue == null) ||
            (!IsEditorAvailable()))
        {
            return false;
        }

        return SetUnityEditorProperty(
            "UnityEditor.Selection",
            selectionPropertyName,
            selectionPropertyValue);
    }

    private bool SetUnityEditorProperty(
        string objectTypeName, string propertyName, object propertyValue)
    {
        const object ObjectInstance = null; // Static property
        if (unityEditorAssembly != null)
        {
            Type type = unityEditorAssembly.GetType(objectTypeName);
            if (type != null)
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property != null)
                {
                    property.SetValue(ObjectInstance, propertyValue, null);
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    ///     Perform the update logic; called by Unity on every frame.
    /// </summary>
    private void Update()
    {
        // If any code is queued for execution, run it
        Object queuedAsset = this.queuedAsset;
        if (queuedAsset != null)
        {
            this.queuedAsset = null;
            string queuedAssetName;
            string queuedCode = GetAssetText(queuedAsset, out queuedAssetName);
            if ((!string.IsNullOrEmpty(queuedCode)) ||
                (!string.IsNullOrEmpty(queuedAssetName)))
            {
                if (!string.IsNullOrEmpty(queuedCode))
                {
                    ExecuteCode(queuedCode);
                    outputScrollPosition.y = Mathf.Infinity;
                }
                else if (!string.IsNullOrEmpty(queuedAssetName))
                {
                    Debug.LogWarning(
                        "CSI queued-asset not executed: " + queuedAssetName, this);
                }
            }
        }
    }


    /// <summary>
    ///     Draws the GUI and execute its interaction logic; called by Unity on a frequent basis.
    /// </summary>
    private void OnGUI()
    {
        if ((csharpEngine == null) ||
            (!ReferenceEquals(Interpreter.Console, this)))
        {
            // Detect and re-initialize after a rebuild 
            // or when the component has been re-enabled
            if (!Reinitialize())
            {
                return; // Cannot have multiple active consoles
            }
        }

        // Process keyboard input
        EnforceParameterLimits();
        Event currentEvent = Event.current;
        if ((currentEvent.isKey) &&
            (!currentEvent.control) &&
            (!currentEvent.shift))
        {
            bool isKeyDown = (currentEvent.type == EventType.KeyDown);
            if (currentEvent.alt)
            {
                if (currentEvent.keyCode == KeyCode.F2)
                {
                    if (isKeyDown)
                    {
                        // For Alt+F2, toggle whether the GUI gets displayed
                        showInteractiveGUI = !showInteractiveGUI;
                    }

                    currentEvent.Use();
                }
            }

            if (GUI.GetNameOfFocusedControl() == InputTextBoxName)
            {
                if (currentEvent.alt)
                {
                    if (currentEvent.keyCode == KeyCode.F1)
                    {
                        if (isKeyDown)
                        {
                            // For Alt+F1, display metadata of the last result
                            OnMetaRequest();
                        }

                        currentEvent.Use();
                    }
                }
                else
                {
                    while (inputTextHistory.Count <= currentHistoryIndex)
                    {
                        inputTextHistory.Add(string.Empty);
                    }

                    while (inputTextCache.Count <= currentHistoryIndex)
                    {
                        inputTextCache.Add(EmptyCacheSlot);
                    }

                    if ((currentEvent.keyCode == KeyCode.UpArrow) ||
                        (currentEvent.keyCode == KeyCode.DownArrow) ||
                        (currentEvent.keyCode == KeyCode.Escape))
                    {
                        // Navigate the input history
                        // NOTE: Holding down Caps Lock would bypass navigation
                        if (!currentEvent.capsLock)
                        {
                            if (isKeyDown)
                            {
                                KeyCode keyCode = currentEvent.keyCode;
                                bool? useTrueForOlderOrFalseForNewerOrNullForUndo =
                                    (keyCode == KeyCode.UpArrow)
                                        ? true
                                        : (keyCode == KeyCode.DownArrow)
                                            ? (bool?) false
                                            : null;
                                OnNavigateHistory(
                                    useTrueForOlderOrFalseForNewerOrNullForUndo);
                            }

                            currentEvent.Use();
                        }
                    }
                    else if ((inputHandler != null) &&
                             (currentEvent.keyCode == KeyCode.Return))
                    {
                        // Handle the enter key; process the input text
                        currentEvent.Use();
                        if (isKeyDown)
                        {
                            OnExecuteInput();
                        }
                    }
                }
            }
        }

        // Draw the GUI
        try
        {
            //this.OnDrawGUI();
        }
        catch (ArgumentException exception)
        {
            // Ignore known exceptions that can happen during shutdown
            if (!exception.Message.Contains("repaint"))
            {
                throw; // Rethrow unknow exceptions
            }
        }

        // Prevent extra whitespace at the start of the edit box
        if (GUI.changed)
        {
            inputText = inputText.TrimStart();
        }
    }

    /// <summary>
    ///     Called when the history needs ot be navigated (e.g., when the up-arrow, down-arrow or escape key is pressed).
    /// </summary>
    /// <param playerName="useTrueForOlderOrFalseForNewerOrNullForUndo">
    ///     Specify how to navigate the history: <c>true</c> for older
    ///     history, <c>false</c> for newer history, or <c>null</c> for to undo some history editing or navigation.
    /// </param>
    private void OnNavigateHistory(
        bool? useTrueForOlderOrFalseForNewerOrNullForUndo)
    {
        // Save the current input text into its slot in the cache
        inputTextCache[currentHistoryIndex] = inputText;
        if (useTrueForOlderOrFalseForNewerOrNullForUndo.HasValue)
        {
            if (useTrueForOlderOrFalseForNewerOrNullForUndo.Value)
            {
                if (--currentHistoryIndex < 0)
                {
                    currentHistoryIndex += inputTextHistory.Count;
                }
            }
            else
            {
                if (++currentHistoryIndex >= inputTextHistory.Count)
                {
                    currentHistoryIndex -= inputTextHistory.Count;
                }
            }
        }
        else
        {
            // For the escape, "undo" in steps depending on the current state
            if (!string.Equals(
                inputText,
                inputTextHistory[currentHistoryIndex],
                StringComparison.Ordinal))
            {
                // Revert the text to the unmodified version
                inputTextCache[currentHistoryIndex] = EmptyCacheSlot;
            }
            else
            {
                // Go to the latest history item
                currentHistoryIndex = inputTextHistory.Count - 1;
            }
        }

        // Load the current input text from the historic slot
        inputText = inputTextCache[currentHistoryIndex];
        if (inputText == EmptyCacheSlot)
        {
            inputText = inputTextHistory[currentHistoryIndex];
        }
    }

    /// <summary>
    ///     Called when the GUI needs to be drawn.
    /// </summary>
    private void OnDrawGUI()
    {
        // Draw the GUI
        const int SpacePixelCount = 4;
        bool showAutoSelectToggle = IsEditorAvailable();
        bool showBlankPrompt = string.IsNullOrEmpty(promptText) || (inputHandler == null);
        float splitHeight = Screen.height*(1f - ((Mathf.Min(0f, topMargin) + Mathf.Min(0f, bottomMargin))/100f));
        var areaRect = new Rect(
            Screen.width*(leftMargin/100f),
            Screen.height*(topMargin/100f),
            Screen.width*(1f - ((leftMargin + rightMargin)/100f)),
            Screen.height*(1f - ((topMargin + bottomMargin)/100f)));
        GUILayout.BeginArea(areaRect);
        GUILayout.BeginVertical(
            GUILayout.MinHeight(60f),
            GUILayout.MaxHeight((splitHeight*(splitterFraction/100f)) - (SpacePixelCount >> 1)),
            GUILayout.Width(areaRect.width));
        GUILayout.FlexibleSpace();
        outputScrollPosition =
            GUILayout.BeginScrollView(outputScrollPosition);
        if (showInteractiveGUI && showOutputText)
        {
            // Ignore changes to text to make it read-only
            GUILayout.TextArea(outputText, outputText.Length);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.Space(SpacePixelCount);
        GUILayout.BeginHorizontal(
            GUILayout.MinHeight(Mathf.Max(90f, ((splitHeight*((100f - splitterFraction)/100f)) - (SpacePixelCount >> 1)))),
            GUILayout.Width(areaRect.width));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        showInteractiveGUI = GUILayout.Toggle(showInteractiveGUI,
            new GUIContent(showBlankPrompt ? PromptBlank : promptText,
                (showInteractiveGUI ? "Click to hide the C# interpreter GUI" : "Click to show the C# interpreter GUI")),
            "Button");
        GUILayout.EndHorizontal();
        if (showInteractiveGUI)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(string.Empty, "Click to clear the output text window"), "Toggle"))
            {
                ClearOutput();
            }

            showOutputText = GUILayout.Toggle(showOutputText,
                new GUIContent(string.Empty,
                    (showOutputText ? "Click to hide the output text window" : "Click to show the output text window")));
            GUILayout.EndHorizontal();
            if (showAutoSelectToggle)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                showOutputAsEditorSelection = GUILayout.Toggle(showOutputAsEditorSelection,
                    new GUIContent(string.Empty,
                        (showOutputAsEditorSelection
                            ? "Click to disable automatic selection of results in the editor"
                            : "Click to enable automatic selection of results in the editor")));
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            showTooltipText = GUILayout.Toggle(showTooltipText,
                new GUIContent(string.Empty,
                    (showTooltipText ? "Click to hide the tooltip text bar" : "Click to display the tooltip text bar")));
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        inputScrollPosition = GUILayout.BeginScrollView(
            inputScrollPosition,
            GUILayout.MaxWidth(areaRect.width - toolboxWidth));
        if (showInteractiveGUI)
        {
            GUI.SetNextControlName(InputTextBoxName);
            inputText = GUILayout.TextArea(inputText);
        }

        GUILayout.EndScrollView();
        if (showInteractiveGUI && showTooltipText)
        {
            GUILayout.Label(GUI.tooltip, "TextField");
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    /// <summary>
    ///     Called when the input text need to be executed (e.g., when Enter is pressed).
    /// </summary>
    public static void OnExecuteInput()
    {
        //string inputText = this.inputText.Trim();
        //Debug.Log(String.Format("Executing: {0}", ConsoleScreen.consoleString.Trim()));
        string inputText = ConsoleScreen.consoleString.Trim();
        //inputText = string.Empty;

        if (inputText == string.Empty)
        {
            // Repeat the previous command, if any

            for (int index = GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count - 1;
                index >= 0;
                index--)
            {
                if (
                    !string.IsNullOrEmpty(
                        GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory[index]))
                {
                    GameObject.Find("Console")
                        .GetComponent<CSharpInterpreter>()
                        .ExecuteCode(
                            GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory[index]);
                    GameObject.Find("Console").GetComponent<CSharpInterpreter>().outputScrollPosition.y = Mathf.Infinity;
                    break;
                }
            }

            return;
        }

        // Move the text from the input to output console
        /// <things to change>
        /// No seprate output field so write output below entered command.
        /// </things to change>
        //CSI.Interpreter.Console.Write(
        //this.promptText + "  " + inputText + Environment.NewLine);
        /*
        GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputScrollPosition = Vector2.zero;
        GameObject.Find("Console").GetComponent<CSharpInterpreter>().outputScrollPosition.y = Mathf.Infinity;

        // Update the input history

        GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextCache.Clear();
        if ((GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count > 0) ||
            (GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory[GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count - 1] == string.Empty))
        {
            GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory[GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count - 1] = inputText;
        }
        else
        {
            GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Add(inputText);
        }

        if (GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count > GameObject.Find("Console").GetComponent<CSharpInterpreter>().maxHistorySize)
        {
            GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.RemoveRange(
                0, (GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count - GameObject.Find("Console").GetComponent<CSharpInterpreter>().maxHistorySize));
        }
        else if ((GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count > 0) &&
            (GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory[0] == string.Empty))
        {
            GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.RemoveAt(0);
        }

        GameObject.Find("Console").GetComponent<CSharpInterpreter>().currentHistoryIndex = GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputTextHistory.Count;
        */
        // Notify the async-handler of the keyboard input
        Interpreter.InputHandler inputHandler =
            GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputHandler;

        GameObject.Find("Console").GetComponent<CSharpInterpreter>().inputHandler = null;
        inputHandler(inputText);
    }

    /// <summary>
    ///     Called when metadata should be displayed (e.g., when Alt+F1 is pressed).
    /// </summary>
    private bool OnMetaRequest()
    {
        object lastResult = GetLastExecuteResult();
        if (lastResult == null)
        {
            return false;
        }

        string inputText = this.inputText.Trim();
        string memberNameFilter;
        int seachTextStartIndex;
        if (string.IsNullOrEmpty(inputText))
        {
            memberNameFilter = null;
            seachTextStartIndex = -1;
        }
        else
        {
            for (seachTextStartIndex = inputText.Length - 1;
                seachTextStartIndex >= 0;
                seachTextStartIndex--)
            {
                if ((!char.IsLetterOrDigit(inputText[seachTextStartIndex])) &&
                    (inputText[seachTextStartIndex] != '_'))
                {
                    break;
                }
            }

            seachTextStartIndex = seachTextStartIndex + 1;
            memberNameFilter = inputText.Substring(seachTextStartIndex);
        }

        string[] memberNames = Utils.GetMeta(lastResult, memberNameFilter);
        if ((memberNames == null) ||
            (memberNames.Length <= 0))
        {
            return false;
        }

        IConsole console = Interpreter.Console;
        string separationLine = new string('-', Math.Min(35, (1 + (console.GetLineWidth() >> 1)))) + Environment.NewLine;
        string memberNameForDetail = null;
        if (memberNames.Length == 1)
        {
            // Incorporate the single found entry's playerName into the 
            // input text, which provides some kind of auto-completion
            memberNameForDetail = memberNames[0];
            if (seachTextStartIndex < 0)
            {
                inputText += memberNameForDetail;
            }
            else
            {
                inputText = inputText.Substring(0, seachTextStartIndex) +
                            memberNameForDetail;
            }

            this.inputText = inputText;
        }
        else
        {
            // Look for the longest substring shared among results
            int matchTextStopIndex = 0;
            while (true)
            {
                if (matchTextStopIndex >= memberNames[0].Length)
                {
                    break;
                }

                char matchChar = memberNames[0][matchTextStopIndex];
                bool isDone = false;
                for (int memberIndex = memberNames.Length - 1; memberIndex > 0; memberIndex--)
                {
                    if ((matchTextStopIndex >= memberNames[memberIndex].Length) ||
                        (matchChar != memberNames[memberIndex][matchTextStopIndex]))
                    {
                        if (matchTextStopIndex == memberNames[memberIndex].Length)
                        {
                            memberNameForDetail = memberNames[memberIndex];
                        }

                        isDone = true;
                        break;
                    }
                }

                if (isDone)
                {
                    break;
                }

                matchTextStopIndex++;
            }

            matchTextStopIndex--;
            if ((matchTextStopIndex >= 0) &&
                ((seachTextStartIndex < 0) ||
                 (matchTextStopIndex >= (inputText.Length - seachTextStartIndex))))
            {
                string commonText = memberNames[0].Substring(0, matchTextStopIndex + 1);
                if (seachTextStartIndex < 0)
                {
                    inputText += commonText;
                }
                else
                {
                    inputText =
                        inputText.Substring(0, seachTextStartIndex) + commonText;
                }

                if (!string.Equals(
                    commonText,
                    memberNameForDetail,
                    StringComparison.Ordinal))
                {
                    memberNameForDetail = null;
                }

                this.inputText = inputText;
            }
            else
            {
                memberNameForDetail = null;
            }

            console.Write(separationLine);
            foreach (string memberName in memberNames)
            {
                console.Write(memberName);
                console.Write(" ");
            }

            console.Write(Environment.NewLine);
        }

        if (!string.IsNullOrEmpty(memberNameForDetail))
        {
            console.Write(separationLine);
            try
            {
                Utils.MInfo(lastResult, memberNameForDetail);
            }
            catch
            {
                // Ignore exceptions while trying to auto-complete
            }
        }

        outputScrollPosition.y = Mathf.Infinity;
        return true;
    }

    private void PromptForInput(string prompt)
    {
        promptText = prompt;
        Interpreter.Console.ReadLineAsync(ExecuteCode);
    }

    #region IConsole Members

    void IConsole.ReadLineAsync(Interpreter.InputHandler callback)
    {
        Interpreter.InputHandler inputHandler = this.inputHandler;
        this.inputHandler = callback; // Register new callback
        if (inputHandler != null)
        {
            inputHandler(null); // Cancel previous handler
        }
    }

    string IConsole.Write(string s)
    {
        outputStringBuilder.Append(s);
        EnforceParameterLimits();
        if (outputStringBuilder.Length > maxOutputSize)
        {
            outputText = outputStringBuilder.ToString();
            outputStringBuilder.Remove(0, (outputStringBuilder.Length - (maxOutputSize >> 1)));
        }

        outputText = outputStringBuilder.ToString().TrimEnd();
        return s;
    }

    int IConsole.GetLineWidth()
    {
        return maxOutputLineWidth;
    }

    int IConsole.GetMaxLines()
    {
        return maxOutputLineCount;
    }

    #endregion

    /// <summary>
    ///     Performs initialization of the C# compiler.
    /// </summary>
    /// <remarks>
    ///     Most of this method's code is a hack as a workaround for the wrong GAC path and compiler search logic that is baked
    ///     into Unity.
    /// </remarks>
    private static class InitializeCompilerForUnity3D
    {
        private static bool didRunOnce;

        public static void RunOnce()
        {
            if (didRunOnce)
            {
                return;
            }

            try
            {
                didRunOnce = true;
                Type monoCompilerType = null;
                foreach (Type type in
                    typeof (CSharpCodeProvider).Assembly.GetTypes())
                {
                    if (type.FullName == "Mono.CSharp.CSharpCodeCompiler")
                    {
                        monoCompilerType = type;
                        break;
                    }
                }

                if (monoCompilerType == null)
                {
                    Debug.LogWarning(
                        "The C# compiler may not yet work on this version of " +
                        "Unity!  Please provide feedback about test results.");
                    return;
                }

                if (Path.DirectorySeparatorChar != '\\')
                {
                    Debug.LogWarning(
                        "The C# compiler may not yet work on this operating " +
                        "system!  Please provide feedback about test results.");
                    return;
                }

                // This begins a hack to bypass the static constructor of 
                // Mono.CSharp.CSharpCodeCompiler and initialize that data 
                // type in an alternative way for Unity 3D (v.2.6.1); it 
                // attempts to locate the Mono and MCS executables correctly
                const BindingFlags StaticNonPublicBindingFlags =
                    BindingFlags.NonPublic | BindingFlags.Static;
                const BindingFlags StaticPublicBindingFlags =
                    BindingFlags.Public | BindingFlags.Static;
                const string EnvVarMonoPath = "MONO_PATH";
                const string EnvVarCsiCompPath = "CSI_COMPILER_PATH";
#if UNITY_2_6
                const string CompilerDirectoryName = "MonoCompiler.framework";
                const string RuntimeDirectoryName = CompilerDirectoryName;
#else // i.e., 3.0 and greater
                const string CompilerDirectoryName = "Mono/lib/mono/2.0";
                const string RuntimeDirectoryName = "Mono/bin";
#endif
                const string DataRootProgramGuessPath = "Unity/Editor/Data/";
                const string CompilerProgramGuessPath = DataRootProgramGuessPath + CompilerDirectoryName;
                const string RuntimeProgramGuessPath = DataRootProgramGuessPath + RuntimeDirectoryName;
                string mcsPath, monoPath;
                string envValMonoPath =
                    Environment.GetEnvironmentVariable(EnvVarMonoPath);
                string[] envSplitMonoPath =
                    string.IsNullOrEmpty(envValMonoPath)
                        ? new string[0]
                        : envValMonoPath.Split(Path.PathSeparator);
                FieldInfo mcsPathField = monoCompilerType.GetField(
                    "windowsMcsPath", StaticNonPublicBindingFlags);
                FieldInfo monoPathField = monoCompilerType.GetField(
                    "windowsMonoPath", StaticNonPublicBindingFlags);
                FieldInfo directorySeparatorCharField = typeof (Path).
                    GetField("DirectorySeparatorChar", StaticPublicBindingFlags);
                if ((mcsPathField == null) ||
                    (monoPathField == null) ||
                    (directorySeparatorCharField == null))
                {
                    Debug.LogWarning(
                        "The C# compiler may not yet work on this version " +
                        "of Mono, since some of the expected fields were not " +
                        "found!  Please provide feedback about test results.");
                    return;
                }

                // To bypass the problematic initialization 
                // code, pretend we're running of Unix
                directorySeparatorCharField.SetValue(null, '/');
                try
                {
                    // Now access a static member to ensure that 
                    // the static constructor has been executed
                    mcsPath = (string) mcsPathField.GetValue(null);
                    monoPath = (string) monoPathField.GetValue(null);
                }
                finally
                {
                    // Restore the bypass mechanism made earlier
                    directorySeparatorCharField.SetValue(null, '\\');
                }

                // Attempt to locate the MCS and Mono executables for Unity
                if ((!File.Exists(mcsPath)) ||
                    (!File.Exists(monoPath)))
                {
                    string compilerPath =
                        GetFullPathOfAssembly(Assembly.GetEntryAssembly()) ?? // Unity 2.6.1
                        GetFullPathOfAssembly(typeof (Uri).Assembly); // Unity 3.x
                    string runtimePath = compilerPath;
                    if (!string.IsNullOrEmpty(compilerPath))
                    {
                        compilerPath = Path.GetFullPath(compilerPath);
                        do
                        {
                            compilerPath = Path.GetDirectoryName(compilerPath);
                            if (compilerPath == null)
                            {
                                break;
                            }

                            if (Directory.Exists(Path.Combine(
                                compilerPath, CompilerDirectoryName)))
                            {
                                compilerPath = Path.Combine(
                                    compilerPath, CompilerDirectoryName);
                                break;
                            }
                        } while (Directory.Exists(compilerPath));
                    }

                    if (string.Equals(
                        CompilerDirectoryName,
                        RuntimeDirectoryName,
                        StringComparison.Ordinal))
                    {
                        runtimePath = null;
                    }
                    else if (!string.IsNullOrEmpty(runtimePath))
                    {
                        runtimePath = Path.GetFullPath(runtimePath);
                        do
                        {
                            runtimePath = Path.GetDirectoryName(runtimePath);
                            if (runtimePath == null)
                            {
                                break;
                            }

                            if (Directory.Exists(Path.Combine(
                                runtimePath, RuntimeDirectoryName)))
                            {
                                runtimePath = Path.Combine(
                                    runtimePath, RuntimeDirectoryName);
                                break;
                            }
                        } while (Directory.Exists(runtimePath));
                    }

                    string envValCsiCompPath =
                        Environment.GetEnvironmentVariable(EnvVarCsiCompPath);
                    string[] envSplitCsiCompPath =
                        string.IsNullOrEmpty(envValCsiCompPath)
                            ? new string[0]
                            : envValCsiCompPath.Split(Path.PathSeparator);
                    var searchPaths = new List<string>();
                    searchPaths.Add(Directory.GetCurrentDirectory());
                    searchPaths.AddRange(envSplitCsiCompPath);
                    searchPaths.Add(compilerPath);
                    searchPaths.Add(runtimePath);
                    searchPaths.AddRange(envSplitMonoPath);
                    foreach (string programFilesRoot in new[]
                    {
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
                        Environment.GetEnvironmentVariable("ProgramFiles"),
                        Environment.GetEnvironmentVariable("ProgramFiles(x86)")
                    })
                    {
                        if (!string.IsNullOrEmpty(programFilesRoot))
                        {
                            searchPaths.Add(Path.Combine(
                                programFilesRoot, CompilerProgramGuessPath));
                            searchPaths.Add(Path.Combine(
                                programFilesRoot, RuntimeProgramGuessPath));
                        }
                    }

                    if (!File.Exists(mcsPath))
                    {
                        mcsPath = SearchForFullPath("gmcs", searchPaths, ".exe", ".bat"); // Must be EXE for Unity 3.x
                    }

                    if (!File.Exists(monoPath))
                    {
                        monoPath = SearchForFullPath("mono", searchPaths, ".bat", ".exe");
                    }

                    if ((!File.Exists(mcsPath)) ||
                        (!File.Exists(monoPath)))
                    {
                        // Attempt to revert to calling the bypassed static constructor
                        ConstructorInfo staticConstructor =
                            monoCompilerType.TypeInitializer;
                        if (staticConstructor == null)
                        {
                            Debug.LogWarning(
                                "The C# compiler may not yet work on " +
                                "this version of Mono, since some of " +
                                "the paths are still missing!  Please " +
                                "provide feedback about test results.");
                        }
                        else
                        {
                            staticConstructor.Invoke(null, null);
                            var alternativePath =
                                (string) mcsPathField.GetValue(null);
                            if (File.Exists(alternativePath))
                            {
                                mcsPath = alternativePath;
                            }

                            alternativePath =
                                (string) monoPathField.GetValue(null);
                            if (File.Exists(monoPath))
                            {
                                monoPath = alternativePath;
                            }
                        }
                    }

                    // Keep any valid paths that were located
                    if (File.Exists(mcsPath))
                    {
                        mcsPathField.SetValue(null, mcsPath);
                    }

                    if (File.Exists(monoPath))
                    {
                        monoPathField.SetValue(null, monoPath);
                    }
                }

#if UNITY_2_6
    // Ensure that the Mono-path environment-variable exists, 
    // since the C# compiler needs this to find mscorlib.dll.
    // NOTE: Since Unity 3.0.0, this is apparently no longer required.
                if ((envSplitMonoPath.Length <= 0) ||
                    (/* single path that doesn't exist */ (envSplitMonoPath.Length == 1) &&
                        (!Directory.Exists(envSplitMonoPath[0]))))
                {
                    if (File.Exists(mcsPath))
                    {
                        envValMonoPath = Path.GetDirectoryName(mcsPath);
                    }

                    if ((!Directory.Exists(envValMonoPath)) &&
                        File.Exists(monoPath))
                    {
                        envValMonoPath = Path.GetDirectoryName(monoPath);
                    }

                    if (!Directory.Exists(envValMonoPath))
                    {
                        envValMonoPath = Path.GetDirectoryName(
                            GetFullPathOfAssembly(typeof(int).Assembly));
                    }

                    if (Directory.Exists(envValMonoPath))
                    {
                        Environment.SetEnvironmentVariable(
                            EnvVarMonoPath, envValMonoPath);
                    }
                }
#endif
            }
            catch (IOException)
            {
                // Probably running in the web player without required rights
            }
        }

        private static string SearchForFullPath(
            string fileNameWithoutExtension,
            IEnumerable<string> searchPaths,
            params string[] fileExtensions)
        {
            if (searchPaths == null)
            {
                return null;
            }

            string applicationPath;
            foreach (string searchPath in searchPaths)
            {
                if (string.IsNullOrEmpty(searchPath))
                {
                    continue;
                }

                applicationPath = Path.Combine(
                    searchPath, fileNameWithoutExtension);
                if ((fileExtensions == null) ||
                    (fileExtensions.Length <= 0))
                {
                    if (File.Exists(applicationPath))
                    {
                        return applicationPath;
                    }
                }
                else
                {
                    foreach (string fileExtension in fileExtensions)
                    {
                        applicationPath = Path.ChangeExtension(
                            applicationPath, fileExtension);
                        if (File.Exists(applicationPath))
                        {
                            return applicationPath;
                        }
                    }
                }
            }

            return null;
        }
    }
}