using System;
using System.Collections;
using UnityEngine;

public class CoroutineExecuter : MonoBehaviour
{
    public Coroutine Execute(IEnumerator coroutine) => StartCoroutine(coroutine);

    public void Stop(IEnumerator coroutine)
    {
        try
        {
            StopCoroutine(coroutine);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}