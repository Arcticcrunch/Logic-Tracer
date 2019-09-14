using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bus : MonoBehaviour, NodeHandler
{
    private RectTransform rect;
    public static int connectedToBus = 0;
    public static float verticalBusDistance = 60f;
    public UILineRenerer lineRenerer;
    
    public NodeHandlerType HandlerType { get { return NodeHandlerType.Bus; } set { } }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (lineRenerer == null)
            lineRenerer = GetComponentInChildren<UILineRenerer>();
    }

    public Vector2 GetNodePosition(int nodeIndex, Transform relativeTransform)
    {
        throw new System.NotImplementedException();
    }

    public GateNode[] GetNodeArray()
    {
        throw new System.NotImplementedException();
    }

    public GateNode GetLastNode()
    {
        throw new System.NotImplementedException();
    }
    public static float GetLastNodeYPosition()
    {
        return -connectedToBus * verticalBusDistance;
    }
    public static float GetBottomBusPosition()
    {
        return -(connectedToBus + 1) * verticalBusDistance;
    }

    public Vector2 AllocateNewPos(Transform relativeTransform)
    {
        connectedToBus++;
        //return relativeTransform.InverseTransformPoint(new Vector2(transform.position.x, connectedToBus * verticalBusDistance));
        return relativeTransform.InverseTransformPoint(new Vector2(rect.anchoredPosition.x, connectedToBus * verticalBusDistance + rect.anchoredPosition.y));
    }

    public Vector2 AllocateNewPos()
    {
        connectedToBus++;
        //return new Vector2(transform.position.x, -(connectedToBus * verticalBusDistance));
        return new Vector2(rect.anchoredPosition.x, -(connectedToBus * verticalBusDistance) + rect.anchoredPosition.y);
    }

    public void DestroyNodeHandler()
    {
        Destroy(lineRenerer.gameObject);
        Destroy(this.gameObject);
        connectedToBus = 0;
    }
}
