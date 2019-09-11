using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateNode : MonoBehaviour
{
    public Gate gate;

    private void Awake()
    {
        if (gate == null)
        {
            gate = GetComponentInParent<Gate>();
        }
    }
    public Vector2 GetPosInParentCoordinateSystem()
    {
        return transform.localPosition;
    }
    public Vector2 GetPosInParentCoordinateSystem(Transform parent)
    {
        return parent.InverseTransformPoint(transform.position);
    }
}
