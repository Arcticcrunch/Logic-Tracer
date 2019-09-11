using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface NodeHandler
{
    Vector2 GetNodePosition(int nodeIndex, Transform relativeTransform);
    Vector2 AllocateNewPos(Transform relativeTransform);
    Vector2 AllocateNewPos();
    GateNode[] GetNodeArray();
    GateNode GetLastNode();
    NodeHandlerType HandlerType { get; set; }
}

public enum NodeHandlerType
{
    Gate, Bus
}
