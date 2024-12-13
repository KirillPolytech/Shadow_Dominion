using System.Collections;
using UnityEngine;

public class DebugWindow : MonoBehaviour
{
    [Range(0f, 10f)] [SerializeField] private float delayUpdate = 2.5f;

    private string _myLog = "";
    private string _output;
    private bool _isEnabled;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;

        _coroutine = StartCoroutine(StartLog());
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
        _output = logString;
        _myLog = $"{_output}\n{_myLog}";
        if (_myLog.Length > 5000)
        {
            _myLog = _myLog[..4000];
        }
    }

    private void OnGUI()
    {
        if (_isEnabled == false)
            return;

        _myLog = GUI.TextArea(new Rect(Screen.width / 1.7f, 10, Screen.width / 4, Screen.height / 2), _myLog);
    }
}