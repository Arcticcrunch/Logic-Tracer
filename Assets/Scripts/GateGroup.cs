using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGroup
{
    // HACK: максимальное значение итераций обработки слоёв может быть больше.(Данное число актуально для дебаггинга)
    private const int MAX_LAYER_ITERATIONS = 10000; 
    private List<NodeHandler> inputList;
    private List<Gate> gateList;
    private List<UILineRenerer> lineList;
    private float gateSpacing = 300f;
    private Gate gatePrefab;
    private UILineRenerer lineRendererPrefab;

    public GateGroup(Gate gatePrefab, UILineRenerer lineRendererPrefab, float gateSpacing)
    {
        inputList = new List<NodeHandler>();
        gateList = new List<Gate>();
        lineList = new List<UILineRenerer>();
        this.gatePrefab = gatePrefab;
        this.lineRendererPrefab = lineRendererPrefab;
        this.gateSpacing = gateSpacing;
    }
    
    public void ClearLists()
    {
        inputList.Clear();
        gateList.Clear();
    }

    public void ConstructGateGroup(List<NodeHandler> inputs, Vector2 origin, Transform parent)
    {
        inputList = inputs;
        List<NodeHandler> uncheckedInputs = inputs;
        List<NodeHandler> nextLayer = new List<NodeHandler>();
        int layerIterationCounter = 0;
        
        // Обработка слоя
        while(layerIterationCounter < MAX_LAYER_ITERATIONS)
        {
            bool pairFound = false;
            int inputsToCheck = uncheckedInputs.Count;
            for (int i = 0; i < inputsToCheck; i++)
            {
                if (pairFound == false)
                {
                    if (i == uncheckedInputs.Count - 1)
                    {
                        nextLayer.Add(uncheckedInputs[i]);
                    }
                    else pairFound = true;
                }
                else
                {
                    NodeHandler nHLast = uncheckedInputs[i - 1];
                    NodeHandler nHCurent = uncheckedInputs[i];
                    Vector2 lastPos = nHLast.HandlerType == NodeHandlerType.Bus  ? nHLast.AllocateNewPos()
                        : nHLast.GetLastNode().GetPosInParentCoordinateSystem(parent);
                    Vector2 curPos = nHCurent.HandlerType == NodeHandlerType.Bus ? nHCurent.AllocateNewPos()
                        : nHCurent.GetLastNode().GetPosInParentCoordinateSystem(parent);
                    Gate g = SpawnGate(gatePrefab, origin + new Vector2(layerIterationCounter * gateSpacing, 0), lastPos,
                        curPos, parent);
                    UILineRenerer r1 = SpawnLine(lineRendererPrefab, lastPos, g.GetNodePosition(0, parent), parent);
                    UILineRenerer r2 = SpawnLine(lineRendererPrefab, curPos, g.GetNodePosition(1, parent), parent);

                    lineList.Add(r1);
                    lineList.Add(r2);
                    gateList.Add(g);

                    pairFound = false;
                    nextLayer.Add(g);
                }
            }
            if (nextLayer.Count == 1)
            {
                break;
            }
            uncheckedInputs = nextLayer;
            nextLayer = new List<NodeHandler>();
            layerIterationCounter++;
        }
    }
    private Gate SpawnGate(Gate gateToSpawn, Vector2 position, Vector2 inputOne, Vector2 inputTwo, Transform parent)
    {
        GameObject go = GameObject.Instantiate(gateToSpawn.gameObject);
        go.transform.SetParent(parent);
        go.transform.localScale = Vector3.one;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = new Vector2(position.x, (inputTwo.y - inputOne.y) * 0.5f + inputOne.y);
        Gate g = go.GetComponent<Gate>();
        return g;
    }
    private UILineRenerer SpawnLine(UILineRenerer lineRenderer, Vector2 inputOne, Vector2 inputTwo, Transform parent)
    {
        GameObject go = GameObject.Instantiate(lineRenderer.gameObject);
        go.transform.SetParent(parent);
        go.transform.localScale = Vector3.one;
        UILineRenerer r = go.GetComponent<UILineRenerer>();
        r.ConstructLine(inputOne, inputTwo);
        return r;
    }

    public GateNode GetLastNodeOutput()
    {
        return gateList[gateList.Count - 1].GetLastNode();
    }
}
