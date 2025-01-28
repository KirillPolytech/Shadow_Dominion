using System;
using UnityEngine;

public class DirectionLIghtRotator : MonoBehaviour
{
    public Vector3 dir;
    
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        _transform.Rotate(dir);
    }
}
