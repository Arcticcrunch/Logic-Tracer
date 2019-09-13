using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, NodeHandler
{
    private Vector2 position = Vector2.zero;

    public string logicOperators;
    public GateNode[] nodeList;

    public NodeHandlerType HandlerType { get { return NodeHandlerType.Gate; } set { } }

    public Vector2 GetPosition()
    {
        return position;
    }
    public void SetPosition(Vector2 pos)
    {
        position = pos;
        GetComponent<RectTransform>().localPosition = pos;
    }
    public RectTransform GetParent()
    {
        return GetComponent<RectTransform>();
    }
    public void SetParent(RectTransform newParent)
    {
        GetComponent<RectTransform>().SetParent(newParent);
    }
    public string GetLogicOperators()
    {
        return logicOperators;
    }

    public Vector2 GetNodePosition(int nodeIndex, Transform relativeTransform)
    {
        return nodeList[nodeIndex].GetPosInParentCoordinateSystem(relativeTransform);
    }
    public GateNode[] GetNodeArray()
    {
        return nodeList;
    }

    public GateNode GetLastNode()
    {
        return nodeList[nodeList.Length - 1];
    }

    public Vector2 AllocateNewPos(Transform relativeTransform)
    {
        throw new System.NotImplementedException();
    }

    public Vector2 AllocateNewPos()
    {
        throw new System.NotImplementedException();
    }

    public void DestroyNodeHandler()
    {
        Destroy(this.gameObject);
    }
}