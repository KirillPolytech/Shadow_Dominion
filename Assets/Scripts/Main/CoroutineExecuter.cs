using System;
using System.Collections;
using UnityEngine;

public class CoroutineExecuter : MonoBehaviour
{
    public void StartCoroutine(float waitTime, Action callBack)
    {
        StartCoroutine(ExecutingCoroutine(waitTime, callBack));
    }

    private IEnumerator ExecutingCoroutine(float waitTime, Action callBack)
    {
        yield return new WaitForSeconds(waitTime);

        callBack?.Invoke();
    }
}