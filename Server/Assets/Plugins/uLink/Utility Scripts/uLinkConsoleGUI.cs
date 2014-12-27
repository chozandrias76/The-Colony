// (c)2011 MuchDifferent. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("uLink Utilities/Console GUI")]
internal class uLinkConsoleGUI : MonoBehaviour
{
    public enum LogLevel
    {
        None,
        ErrorOnly,
        ErrorAndWarning,
        All,
    }

    public enum Position
    {
        Bottom,
        Top,
    }

    private const float WINDOW_MARGIN_X = 10;
    private const float WINDOW_MARGIN_Y = 10;

    private static readonly Color[] levelColors =
    {
        Color.red,
        Color.yellow,
        Color.white
    };

    private static readonly string[] levelNames =
    {
        "Errors Only",
        "Errors And Warnings",
        "All"
    };

    private static readonly Color[] typeColors =
    {
        Color.red,
        Color.magenta,
        Color.yellow,
        Color.white,
        Color.red
    };

    [SerializeField] private bool _isVisible = true;
    public bool autoScroll = true;
    public LogLevel autoShowOnLogLevel = LogLevel.ErrorAndWarning;

    public LogLevel captureLogLevel = LogLevel.All;
    public bool dontDestroyOnLoad = false;
    private List<Entry> entries;
    private int errorCount;
    public LogLevel filterLogLevel = LogLevel.All;
    public int guiDepth = 0;
    public GUISkin guiSkin = null;

    public int maxEntries = 1000;
    private bool oldLockCursor;
    public Position position = Position.Bottom;
    private Vector2 scrollPosition;

    public KeyCode showByKey = KeyCode.Tab;

    public bool unlockCursorWhenVisible = true;

    private int warningCount;
    public int windowheight = 150;

    public bool isVisible
    {
        get { return _isVisible; }
        set { SetVisible(value); }
    }

    private void Awake()
    {
        entries = new List<Entry>(maxEntries);

        if (_isVisible)
        {
            _isVisible = false;
            SetVisible(true);
        }

        UnityEngine.Application.RegisterLogCallback(CaptureLog);
    }

    private void Update()
    {
        if (showByKey != KeyCode.None && Input.GetKeyDown(showByKey))
        {
            SetVisible(!_isVisible);
        }
    }

    private void OnGUI()
    {
        if (!_isVisible) return;

        GUISkin oldSkin = GUI.skin;
        int oldDepth = GUI.depth;
        Color oldColor = GUI.color;

        GUI.skin = guiSkin;
        GUI.depth = guiDepth;
        GUI.color = Color.white;

        float y = (position == Position.Bottom) ? Screen.height - windowheight - WINDOW_MARGIN_Y : WINDOW_MARGIN_Y;

        GUILayout.BeginArea(new Rect(WINDOW_MARGIN_X, y, Screen.width - WINDOW_MARGIN_X*2, windowheight), GUI.skin.box);
        DrawGUI();
        GUILayout.EndArea();

        GUI.skin = oldSkin;
        GUI.depth = oldDepth;
        GUI.color = oldColor;
    }

    private void DrawGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear", GUILayout.Width(50)))
        {
            Clear();
        }

        GUILayout.Space(15);

        GUILayout.Label("Filter: ", GUILayout.ExpandWidth(false));
        GUILayout.Space(5);

        GUI.color = levelColors[filterLogLevel - LogLevel.ErrorOnly];

        if (GUILayout.Button(levelNames[filterLogLevel - LogLevel.ErrorOnly], GUILayout.Width(150)))
        {
            filterLogLevel = filterLogLevel == LogLevel.All ? LogLevel.ErrorOnly : filterLogLevel + 1;
        }

        GUI.color = Color.white;

        GUILayout.Space(15);

        autoScroll = GUILayout.Toggle(autoScroll, "Auto Scroll");

        GUILayout.Space(15);

        if (warningCount != 0)
        {
            GUI.color = typeColors[(int) LogType.Warning];
            GUILayout.Label(warningCount + " Warning(s)", GUILayout.ExpandWidth(false));
            GUI.color = Color.white;
        }
        else
        {
            GUILayout.Label("0 Warning(s)", GUILayout.ExpandWidth(false));
        }

        GUILayout.Space(5);

        if (errorCount != 0)
        {
            GUI.color = typeColors[(int) LogType.Error];
            GUILayout.Label(errorCount + " Error(s)", GUILayout.ExpandWidth(false));
            GUI.color = Color.white;
        }
        else
        {
            GUILayout.Label("0 Error(s)", GUILayout.ExpandWidth(false));
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);

        for (int i = 0; i < entries.Count; i++)
        {
            Entry entry = entries[i];

            if (entry.level <= filterLogLevel)
            {
                GUI.color = typeColors[(int) entry.type];

                GUILayout.BeginHorizontal();

                if (GUILayout.Button((!entry.expanded) ? ">" : "<", GUILayout.Width(20f)))
                {
                    entry.expanded = !entry.expanded;
                }

                GUILayout.Label(entry.log);
                GUILayout.EndHorizontal();

                if (entry.expanded)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30f);
                    GUILayout.Label(entry.stacktrace);
                    GUILayout.EndHorizontal();
                }
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    public void SetVisible(bool visibility)
    {
        if (_isVisible == visibility) return;
        _isVisible = visibility;

        if (unlockCursorWhenVisible)
        {
            if (visibility)
            {
                oldLockCursor = Screen.lockCursor;
            }
            else
            {
                Screen.lockCursor = oldLockCursor;
            }
        }
    }

    public void Clear()
    {
        entries.Clear();
        warningCount = 0;
        errorCount = 0;
    }

    private void CaptureLog(string log, string stacktrace, LogType type)
    {
        LogLevel level =
            (type <= LogType.Assert || type == LogType.Exception)
                ? LogLevel.ErrorOnly
                : (type == LogType.Warning) ? LogLevel.ErrorAndWarning : LogLevel.All;

        if (level > captureLogLevel) return;

        if (level <= autoShowOnLogLevel)
        {
            isVisible = true;
        }

        if (entries.Count == maxEntries)
        {
            LogLevel lastLevel = entries[0].level;
            entries.RemoveAt(0);

            if (lastLevel == LogLevel.ErrorAndWarning) warningCount--;
            else if (lastLevel == LogLevel.ErrorOnly) errorCount--;
        }

        entries.Add(new Entry {log = log, stacktrace = stacktrace, type = type, level = level, expanded = false});

        if (level == LogLevel.ErrorAndWarning) warningCount++;
        else if (level == LogLevel.ErrorOnly) errorCount++;

        if (autoScroll) scrollPosition.y = float.MaxValue;
    }

    private class Entry
    {
        public bool expanded;
        public LogLevel level;
        public string log;
        public string stacktrace;
        public LogType type;
    }
}