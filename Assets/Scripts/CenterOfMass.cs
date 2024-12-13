using System.Collections.Generic;
using UnityEngine;

public static class CenterOfMass
{
    private static Vector3 CalculatingCenterOfMass(IReadOnlyCollection<Transform> copyFrom)
    {
        Vector3 _tempCenter = default;

        foreach (var t in copyFrom)
        {
            _tempCenter += t.transform.localPosition;
        }

        _tempCenter /= copyFrom.Count;

        return _tempCenter;
    }

}