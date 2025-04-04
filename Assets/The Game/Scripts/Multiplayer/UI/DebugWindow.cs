using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    [SerializeField] 
    private int fontSize = 18;
    
    [SerializeField] 
    private bool isDontDestroyOnLoad;
    
    private float width;
    private float height;
    private float x;
    private float y;
    private string myLog = "";
    private bool isEnabled;
    private Vector2 scrollPosition;
    
    private readonly object logLock = new();
    private readonly System.Text.StringBuilder logBuilder = new();

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
        if (isDontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        width = Screen.width / 3;
        height = Screen.height / 2;
        x = Screen.width - width - 20;
        y = 10;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isEnabled = !isEnabled;
        }
    }

    private void Log(string logString, string stackTrace, LogType type)
    {
        Color logColor = type switch
        {
            LogType.Error => Color.red,
            LogType.Assert => Color.yellow,
            LogType.Warning => Color.yellow,
            LogType.Log => Color.white,
            LogType.Exception => Color.red,
            _ => Color.white
        };

        lock (logLock)
        {
            logBuilder.AppendLine($"<color=#{ColorUtility.ToHtmlStringRGB(logColor)}>{logString}</color>");
            if (logBuilder.Length > 5000)
            {
                logBuilder.Remove(0, logBuilder.Length - 4000);
            }
            myLog = logBuilder.ToString();
        }
    }

    private void OnGUI()
    {
        if (!isEnabled)
            return;

        GUIStyle textStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            fontSize = fontSize,
            alignment = TextAnchor.UpperLeft,
            wordWrap = true
        };

        GUILayout.BeginArea(new Rect(x, y, width, height), GUI.skin.box);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));
        GUILayout.Label(myLog, textStyle);
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        // Автопрокрутка вниз без наложения текста
        if (Event.current.type == EventType.Repaint)
        {
            scrollPosition.y = Mathf.Max(0, float.MaxValue);
        }
    }
}
