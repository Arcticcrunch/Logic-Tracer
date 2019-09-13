using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateNode : MonoBehaviour
{
    //private RectTransform rect;
    public Gate gate;

    private void Awake()
    {
        if (gate == null)
            gate = GetComponentInParent<Gate>();
        //rect = GetComponent<RectTransform>();
    }
    public Vector2 GetPosInParentCanvas()
    {
        return transform.localPosition;
    }
    public Vector2 GetPosInParentCoordinateSystem(Transform parent)
    {
        return parent.InverseTransformPoint(transform.position);
    }
}
