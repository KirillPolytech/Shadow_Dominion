using System;
using System.Collections;
using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    [Range(0f, 10f)] [SerializeField] private float delayUpdate = 2.5f;

    [SerializeField] private bool IsDontDestroyOnLoad;

    private float _width;
    private float _height;
    private float _x;
    private float _y;
    private string _myLog = "";
    private string _output;
    private bool _isEnabled;
    private Coroutine _coroutine;
    private Color _textColor;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;

        _coroutine = StartCoroutine(StartLog());

        if (IsDontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        _width = Screen.width / 4;
        _height = Screen.height / 2;
        _x = Screen.width / 1.7f;
        _y = 10;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _isEnabled = !_isEnabled;
        }
    }

    private IEnumerator StartLog()
    {
        while (true)
        {
            //Debug.Log(_myLog);

            yield return new WaitForSeconds(delayUpdate);
        }
    }

    private void Log(string logString, string stackTrace, LogType type)
    {
        _output = type switch
        {
            LogType.Error => $"{logString}",
            LogType.Assert => $"{logString}",
            LogType.Warning => $"{logString}",
            LogType.Log => $"{logString}",
            LogType.Exception => $"{logString}",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        switch (type)
        {
            case LogType.Error:
                _textColor = Color.red;
                break;
            case LogType.Assert:
                _textColor = Color.yellow;
                break;
            case LogType.Warning:
                _textColor = Color.yellow;
                break;
            case LogType.Log:
                _textColor = Color.white;
                break;
            case LogType.Exception:
                _textColor = Color.red;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        _myLog = $"{_output}\n{_myLog}";
        if (_myLog.Length > 5000)
        {
            _myLog = _myLog[..4000];
        }
    }

    private void OnGUI()
    {
        if (!_isEnabled)
            return;

        GUIStyle style = new GUIStyle(GUI.skin.textArea);
        style.normal.textColor = _textColor;

        _myLog = GUI.TextArea(new Rect(_x, _y, _width, _height), _myLog, style);
    }
}