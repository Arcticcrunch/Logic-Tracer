using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGenerator : MonoBehaviour
{
    public Vector2 originOffset = new Vector2(320, -50);    //Отступ от начала отрисовки всего Layout`a
    public Vector2 gateSize = new Vector2(100, 100);        //Размер гейта
    public Vector2 labelCellSize = new Vector2(50, 50);     //Размер ярлыка с индексом шины
    public float busWidth = 15f;                            //Ширина шины
    public float gateSpacing = 130f;                        //Растояние по Х между гейтами
    public float busSpacing = 50f;                          //Расстояние по Х между шинами
    public float busNodeDistance = 40f;                     //Промежуток по Y между нодами шин
    public float gateGridXOffset = 120f;                    //Отступ начала сетки гейтов от последней шины
    

    private List<Gate> gatesList = new List<Gate>();
    private List<UILineRenerer> linesList = new List<UILineRenerer>();
    private List<GameObject> labelsList = new List<GameObject>();
    private List<NodeHandler> buses = new List<NodeHandler>();
    private List<GateGroup> gateGroups = new List<GateGroup>();
    private float[] busXPosPositions;
    
    // Сервисные переменные
    private int totalBusCount = 0;
    private float busYPos = 0;

    #region Reference
    public Settings settings;
    public TruthtableGenerator truthtableGenerator;
    public RectTransform contentPanel;
    #endregion

    #region Prefabs
    public GameObject andGate;
    public GameObject notGate;
    public GameObject orGate;
    public GameObject node;
    public GameObject connectionLine;
    public GameObject emptyCell;
    public GameObject bus;
    #endregion

    public void CreateLayout()
    {
        ClearLayout();

        switch (settings.GetFormulaType)
        {
            case FormulaType.Conjunction:
                CreateConjunctionLayout();
                break;
            case FormulaType.Disjunction:
                CreateDisjunctionLayout();
                break;
            case FormulaType.Auto:
                bool[] arr = truthtableGenerator.GetTruthColumn();
                int trueCount = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    trueCount += arr[i] == true ? 1 : 0;
                }
                if (trueCount <= arr.Length / 2)
                    CreateDisjunctionLayout();
                else CreateConjunctionLayout();
                break;
            default:
                break;
        }
    }

    private void CreateDisjunctionLayout()
    {
        CreateBuses();
        
        bool[] truthTable = truthtableGenerator.GetTruthColumn();
        bool[,] matrix = truthtableGenerator.GetMatrix();
        List<int> truthIndexes = new List<int>();
        for (int i = 0; i < truthTable.Length; i++)
        {
            if (truthTable[i])
                truthIndexes.Add(i);
        }
        if (truthIndexes.Count == 0)
            return;
        
        NodeHandler[,] busesOutputs = new NodeHandler[truthIndexes.Count, settings.GetInputsCount];
        for (int x = 0; x < truthIndexes.Count; x++)
        {
            for (int y = 0; y < settings.GetInputsCount; y++)
            {
                bool isCurrentRankTrue = matrix[y, truthIndexes[x]];
                busesOutputs[x, y] = isCurrentRankTrue ? buses[y] : buses[y + settings.GetInputsCount];
            }
        }
        List<GateGroup> firstLayer = new List<GateGroup>();
        Vector2 gateGridXPosStart = new Vector2(busXPosPositions[busXPosPositions.Length - 1] + gateGridXOffset, 0);
        Vector2 mostDistantNodePos = Vector2.zero;
        for (int i = 0; i < busesOutputs.GetLength(0); i++)
        {
            GateGroup gg = new GateGroup(andGate.GetComponent<Gate>(), connectionLine.GetComponent<UILineRenerer>(), gateSpacing);
            List<NodeHandler> inputs = new List<NodeHandler>();
            for (int z = 0; z < busesOutputs.GetLength(1); z++)
            {
                inputs.Add(busesOutputs[i, z]);
            }
            gg.ConstructGateGroup(inputs, gateGridXPosStart, contentPanel, busWidth);
            Vector2 pos = gg.GetLastNodeOutput().GetPosInParentCoordinateSystem(contentPanel);
            if (pos.x > mostDistantNodePos.x)
                mostDistantNodePos = pos;
            firstLayer.Add(gg);
            gateGroups.Add(gg);
        }
        
        GateGroup orGroup = new GateGroup(orGate.GetComponent<Gate>(), connectionLine.GetComponent<UILineRenerer>(), gateSpacing);
        List<NodeHandler> layerOne = new List<NodeHandler>();
        for (int z = 0; z < firstLayer.Count; z++)
        {
            layerOne.Add(firstLayer[z].GetLastGate());
        }
        orGroup.ConstructGateGroup(layerOne, mostDistantNodePos + new Vector2(gateSpacing, 0), contentPanel, busWidth);
        gateGroups.Add(orGroup);


        CreateBusLabels();
        CreatePostOutline();
    }
    private void CreateConjunctionLayout()
    {
        CreateBuses();
        CreateBusLabels();
    }

    private void CreateBuses()
    {
        totalBusCount = settings.GetInputsCount * 2;
        busXPosPositions = new float[totalBusCount];
        for (int i = 0; i < busXPosPositions.Length; i++)
        {
            if (i >= busXPosPositions.Length / 2)
                //TODO: Добавить переменную для контроля отступа между обычными и инвертироваными шинами
                busXPosPositions[i] = originOffset.x + (busSpacing * i) + (gateSize.x + busSpacing);
            else busXPosPositions[i] = originOffset.x + (busSpacing * i);
        }
        busYPos = originOffset.y + (settings.GetInputsCount * (gateSize.y + busNodeDistance) * (-1));

        for (int i = 0; i < totalBusCount; i++)
        {
            CreateBus(new Vector2(busXPosPositions[i], busYPos));
        }
    }

    private void CreateBus(Vector2 pos)
    {
        GameObject go = Instantiate(bus);
        go.transform.SetParent(contentPanel);
        go.transform.localScale = Vector3.one;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);

        rect.anchoredPosition = pos;
        Bus b = go.GetComponent<Bus>();
        b.lineRenerer.showNodes = false;
        // TODO: Заменить второй вектор для коректного расчёта длины шины
        b.lineRenerer.ConstructLine(Vector2.zero, new Vector2(0, -1200), busWidth);
        buses.Add(b);
    }
    private void CreateBusLabels()
    {
        for (int i = 0; i < totalBusCount; i++)
        {
            bool isNotInvertedInput = i < (totalBusCount / 2);
            string str = "";
            str += isNotInvertedInput ? "X" : "!X";
            str += isNotInvertedInput ? (i + 1).ToString() : ((i + 1) - (totalBusCount / 2)).ToString();
            Vector2 newPos = new Vector2(busXPosPositions[i] - (labelCellSize.x * 0.5f), originOffset.y + labelCellSize.y);
            CreateLabel(str, labelCellSize.x, labelCellSize.y, newPos, contentPanel);
        }

        Vector2 outputLabelPos = gateGroups[gateGroups.Count - 1].GetLastNodeOutput().GetPosInParentCoordinateSystem(contentPanel);
        outputLabelPos += new Vector2(labelCellSize.x * 3 + (labelCellSize.x * 0.5f), labelCellSize.y + labelCellSize.y * 0.5f);
        CreateLabel("Y", labelCellSize.x, labelCellSize.y, outputLabelPos, contentPanel);
    }
    private RectTransform CreateLabel(string text, float width, float height, Vector2 pos, Transform parent)
    {
        GameObject go = Instantiate(emptyCell);
        Text textScrpt = go.GetComponentInChildren<Text>();
        RectTransform rect = go.GetComponent<RectTransform>();
        go.transform.SetParent(parent);
        rect.localScale = Vector3.one;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rect.anchoredPosition = pos;
        textScrpt.text = text;
        labelsList.Add(go);
        return rect;
    }
    private void CreatePostOutline()
    {
        Vector2 outputLinelStartPos = gateGroups[gateGroups.Count - 1].GetLastNodeOutput().GetPosInParentCoordinateSystem(contentPanel);
        Vector2 outputLinelEndPos = outputLinelStartPos + new Vector2(labelCellSize.x * 4, 0);
        CreateLine(outputLinelStartPos, outputLinelEndPos, busWidth, contentPanel);
    }
    private void CreateLine(Vector2 startPos, Vector2 endPos, float lineWidth, Transform parent)
    {
        GameObject go = Instantiate(connectionLine);
        go.transform.SetParent(parent);
        go.transform.localScale = Vector3.one;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.anchoredPosition = startPos;
        UILineRenerer lR = go.GetComponent<UILineRenerer>();
        lR.ConstructLine(startPos, endPos, lineWidth);
        linesList.Add(lR);
    }

    // Удаление всех объектов и очистка всех списков
    public void ClearLayout()
    {
        foreach (Gate item in gatesList)
        {
            Destroy(item.gameObject);
        }
        foreach (UILineRenerer item in linesList)
        {
            Destroy(item.gameObject);
        }
        foreach (GameObject item in labelsList)
        {
            Destroy(item.gameObject);
        }
        foreach (NodeHandler item in buses)
        {
            item.DestroyNodeHandler();
        }
        foreach (GateGroup item in gateGroups)
        {
            item.DestroyGateGroup();
        }
        gatesList.Clear();
        linesList.Clear();
        labelsList.Clear();
        buses.Clear();
        gateGroups.Clear();
    }
}
