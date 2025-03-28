using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoroutineExecuter : MonoBehaviour
{
    private readonly Dictionary<string, Coroutine> _activeCoroutines = new Dictionary<string, Coroutine>();

    public Coroutine Execute(string key, IEnumerator routine)
    {
        if (_activeCoroutines.ContainsKey(key))
        {
            Debug.LogWarning($"Coroutine with key '{key}' is already running.");
            return _activeCoroutines[key];
        }

        Coroutine coroutine = StartCoroutine(WrapCoroutine(key, routine));
        _activeCoroutines[key] = coroutine;
        return coroutine;
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

    public bool IsRunning(string key) => _activeCoroutines.ContainsKey(key);

    private IEnumerator WrapCoroutine(string key, IEnumerator routine)
    {
        yield return StartCoroutine(routine);
        _activeCoroutines.Remove(key);
    }
}