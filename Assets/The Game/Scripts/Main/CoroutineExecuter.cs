using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion;
using UnityEngine;

public class CoroutineExecuter : MonoBehaviour
{
    private readonly Dictionary<string, Coroutine> _activeCoroutines = new();

    private void Awake()
    {
        MirrorServer.Instance.ActionOnHostStop += StopAll;
    }

    private void OnDestroy()
    {
        MirrorServer.Instance.ActionOnHostStop -= StopAll;
    }

    public void Execute(string key, IEnumerator routine)
    {
        if (_activeCoroutines.ContainsKey(key))
        {
            Debug.LogWarning($"Coroutine with key '{key}' is already running.");
        }

        _activeCoroutines[key] = StartCoroutine(routine);
    }

    public void Stop(string key)
    {
        if (!_activeCoroutines.TryGetValue(key, out Coroutine coroutine))
            return;

        StopCoroutine(coroutine);
        _activeCoroutines.Remove(key);
    }

    public void Stop(Coroutine coroutine)
    {
        foreach (var pair in _activeCoroutines.Where(pair => pair.Value == coroutine))
        {
            StopCoroutine(coroutine);
            _activeCoroutines.Remove(pair.Key);
            return;
        }
    }
    
    public void StopAll()
    {
        foreach (var pair in _activeCoroutines.Where(pair => pair.Value != null))
        {
            StopCoroutine(pair.Value);
        }
        
        _activeCoroutines.Clear();
        
        StopAllCoroutines();
    }

    public bool IsRunning(string key) => _activeCoroutines.ContainsKey(key);
}