using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class LayoutGenerator : MonoBehaviour
{
    public Vector2 charCellSize = new Vector2(50, 50);
    public Vector2 gateSize = new Vector2(100, 100);
    public Vector2 originOffset = new Vector2(320, -50);
    public Vector2 gateGridOffset = Vector2.zero;           // Нужно вычислять в рантайме
    public Vector2 additionalBusOffset = Vector2.zero;      // Нужно вычислять в рантайме

    public float inputBusVerticalDistance = 20f;
    public float inputBusSpacing = 60f;
    public float invertedBusOffsetMul = 0.8f;


    private int inputBusCount = 1;
    private int gateGridWidth = 1;
    private int gateGridHeight = 1;



    //public Vector2 indirectInputNodeGridOffset = Vector2.zero;      
    //public Vector2 inputNodeGridSpacing = new Vector2(40, -80);
    //public Vector2 gateGridOffset = new Vector2(50, -50);
    //public Vector2 gateNodeRelativeOffset = new Vector2(80, 25);    // Нужно вычислять исходя из размера гейта
    //public float inputBusWidth = 15f;
    //public float nodeSize = 25f;
    //private int inputNodeGridWidth = 1;
    //private int inputNodeGridHeight = 1;


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
    
    List<Gate> gateList;
    List<UILineRenerer> connectionsList;
    List<GameObject> textCellList;

    private void Awake()
    {
        gateList = new List<Gate>();
        connectionsList = new List<UILineRenerer>();
        textCellList = new List<GameObject>();

        additionalBusOffset = new Vector2(inputBusSpacing * invertedBusOffsetMul, 0);
    }

    // Очистка панели
    private void ClearLayout()
    {
        foreach (Gate gate in gateList)
        {
            Destroy(gate.gameObject);
        }
        foreach (UILineRenerer u in connectionsList)
        {
            Destroy(u.gameObject);
        }
        foreach (GameObject go in textCellList)
        {
            Destroy(go);
        }
        gateList.Clear();
        connectionsList.Clear();
        textCellList.Clear();
    }

    // Создание сеток расположения гейтов и входных нодов
    private void CreateGrids()
    {
        // Создание сетки гейтов
        gateGridWidth = Math.ClosesHigherPowerOfTwo(settings.GetInputsCount) + settings.GetInputsCount;
        int devisionResult = settings.GetInputsCount / 2;
        int difference = settings.GetInputsCount % 2;
        gateGridHeight = Math.Pow2(settings.GetInputsCount) * (difference == 0 ? devisionResult : devisionResult + 1);

        // Расчёт количества входных шин
        inputBusCount = settings.GetInputsCount * 2;

        // Расчёт отступа для начала отрисовки сетки гейтов
        float width = inputBusCount * inputBusSpacing + additionalBusOffset.x + gateSize.x;
        float height = charCellSize.y + gateSize.y + (inputBusCount * inputBusVerticalDistance);
        gateGridOffset = new Vector2(width, -height) + originOffset;
    }

    // Расстановка всех элементов
    private void PopulateGrids()
    {

    }

    // Отрисовка всех элементов
    private void RenderGrids()
    {
        // Отрисовка индексов входов
        for (int i = 0; i < inputBusCount; i++)
        {
            GameObject go = Instantiate(emptyCell);
            Text text = go.GetComponentInChildren<Text>();
            bool isNotInvertedInput = i < (inputBusCount / 2);
            text.text += isNotInvertedInput ? "X" : "!X";
            text.text += isNotInvertedInput ? (i + 1).ToString() : ((i + 1) - (inputBusCount / 2)).ToString();
            go.transform.SetParent(contentPanel);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, charCellSize.x);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, charCellSize.y);
            Vector2 newPos = originOffset + new Vector2(i * inputBusSpacing, 0) + (isNotInvertedInput ? Vector2.zero : additionalBusOffset);
            rect.localPosition = newPos;

            textCellList.Add(go);
        }

        // Отрисовка входных шин







        // TEST!
        List<NodeHandler> buses = new List<NodeHandler>();

        for (int i = 0; i < settings.GetInputsCount; i++)
        {
            GameObject go = Instantiate(bus);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rect.localPosition = new Vector2(originOffset.x + i * inputBusSpacing, originOffset.y - charCellSize.y);
            Bus b = go.GetComponent<Bus>();
            b.lineRenerer.showNodes = false;
            b.lineRenerer.ConstructLine(Vector2.zero, new Vector2(0, -700));
            buses.Add(b);
        }

        GateGroup GG = new GateGroup(andGate.GetComponent<Gate>(), connectionLine.GetComponent<UILineRenerer>(), gateSize.x * 2);
        GG.ConstructGateGroup(buses, new Vector2(1000 , -250), transform);

        //GameObject gObj1 = Instantiate(connectionLine.gameObject);
        //UILineRenerer lR = gObj1.GetComponent<UILineRenerer>();
        //RectTransform rect1 = gObj1.GetComponent<RectTransform>();
        //gObj1.transform.SetParent(contentPanel);
        //rect1.localScale = Vector3.one;
        //lR.ConstructLine(gateGridOffset, gateGridOffset + new Vector2(0, -100));
        //connectionsList.Add(lR);



        // Отрисовка входных шин
        //    // Вертикальные шины
        //    float bottomLine = inputBusGrid.GetLength(1) * inputNodeGridSpacing.y + originOffset.y + charCellSize.y;         // Нижняя точка шин
        //    for (int i = 0; i < busCount; i++)
        //    {
        //        bool isNotInvertedInput = i < (busCount / 2);
        //        GameObject go = Instantiate(connectionLine);
        //        UILineRenerer lR = go.GetComponent<UILineRenerer>();
        //        RectTransform rect = go.GetComponent<RectTransform>();
        //        go.transform.SetParent(contentPanel);
        //        rect.localScale = Vector3.one;
        //        //Vector2[] positions = new Vector2[(settings.GetInputsCount * 4) + (settings.GetInputsCount * 3)];
        //        Vector2[] vertexes = new Vector2[2];
        //   Vector2 additionalHorizontalOffset = isNotInvertedInput ? Vector2.zero : new Vector2(inputNodeGridSpacing.x * invertedBusOffsetMul, 0);
        //        float additionalVerticalOffset = isNotInvertedInput ? 0 : (settings.GetInputsCount + 1) * (inputNodeGridSpacing.y * 0.5f);
        //        vertexes[0] = originOffset + new Vector2(i * inputNodeGridSpacing.x,
        //            originOffset.y + charCellSize.y - (charCellSize.y * 0.5f) + additionalVerticalOffset) + additionalHorizontalOffset;
        //        vertexes[1] = originOffset + new Vector2(i * inputNodeGridSpacing.x, bottomLine) + additionalHorizontalOffset;
        //        lR.ConstructLine(vertexes);
        //        connectionsList.Add(lR);
        //    }
        //    // Инвертирующие шины
        //    for (int i = 0; i < settings.GetInputsCount; i++)
        //    {
        //        GameObject go = Instantiate(connectionLine);
        //        UILineRenerer lR = go.GetComponent<UILineRenerer>();
        //        RectTransform rect = go.GetComponent<RectTransform>();
        //        go.transform.SetParent(contentPanel);
        //        rect.localScale = Vector3.one;
        //        Vector2[] vertexes = new Vector2[3];
        //        float additionalVerticalOffset = (settings.GetInputsCount + 1) * (inputNodeGridSpacing.y * 0.5f);
        //        vertexes[0] = originOffset + new Vector2(i * inputNodeGridSpacing.x,
        //            originOffset.y + charCellSize.y - (charCellSize.y * 0.5f) + additionalVerticalOffset);
        //        //vertexes[2] = originOffset + new Vector2(i * inputNodeGridSpacing.x, bottomLine);
        //    
        //        //vertexes[0] = originOffset + (inputNodeGridSpacing * i) - new Vector2(0, charCellSize.y - (charCellSize.y * 0.5f));
        //        vertexes[2] = originOffset + new Vector2(inputNodeGridSpacing.x * invertedBusOffsetMul, 0) + 
        //            new Vector2(inputNodeGridSpacing.x * (settings.GetInputsCount + i),
        //            inputNodeGridSpacing.y * (settings.GetInputsCount + 1) - charCellSize.y - (charCellSize.y * 0.5f) + additionalVerticalOffset);
        //    
        //        vertexes[1] = new Vector2(vertexes[2].x, vertexes[0].y);
        //    
        //        lR.ConstructLine(vertexes);
        //        connectionsList.Add(lR);
        //    }
        // Отрисовка нодов входной шины (для дебаггинга)

    }


    public void GenerateLayout()
    {
        ClearLayout();
        CreateGrids();
        PopulateGrids();
        RenderGrids();
    }

    private void SpawnBuses()
    {

    }



    /*
    public void GenerateLayout123123()
    {
        // Очистка панели
        if (gatesGrid != null)
        {
            for (int y = 0; y < gatesGrid.GetLength(1); y++)
            {
                for (int x = 0; x < gatesGrid.GetLength(0); x++)
                {
                    if (gatesGrid[x, y] != null)
                        Destroy(gatesGrid[x, y].gameObject);
                }
            }
        }
        //foreach (Node node in nodeList)
        //{
        //    if (node != null)
        //        Destroy(node.gameObject);
        //}
        foreach (UILineRenerer c in connectionsList)
        {
            if (c != null)
                Destroy(c.gameObject);
        }
        gatesGrid = null;
        gateList.Clear();
        //nodeList.Clear();
        connectionsList.Clear();

        // Создание сетки
        gridWidth = Math.ClosesHigherPowerOfTwo(settings.GetInputsCount) + settings.GetInputsCount;
        int devisionResult = settings.GetInputsCount / 2;
        int difference = settings.GetInputsCount % 2;
        gridHeight = Math.Pow2(settings.GetInputsCount) * (difference == 0 ? devisionResult : devisionResult + 1);
        gatesGrid = new Gate[gridWidth, gridHeight];

        // Подгонка размера родительской панели
        contentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (gridWidth * gridCellSize.x) + (originOffset.x * 2));
        contentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (gridHeight * gridCellSize.y) + (originOffset.y * 2));

        // Создание входной шины
        for (int i = 0; i < settings.GetInputsCount; i++)
        {
            GameObject go = Instantiate(connectionLine);
            UILineRenerer lR = go.GetComponent<UILineRenerer>();
            go.transform.SetParent(contentPanel);
            //Vector2[] positions = {new Vector2(i * busSpacing + originOffset.x, originOffset.y),
            //    new Vector2(i * busSpacing + originOffset.x, -contentPanel.rect.size.y - originOffset.y) };
            Vector2[] positions = { new Vector2(i * busSpacing + originOffset.x, originOffset.y), new Vector2(i * busSpacing + originOffset.x,
                -contentPanel.rect.size.y - originOffset.y) };
            lR.SetLineWidth(10);
            lR.ConstructLine(positions);
            //lR.SetPosition(0, contentPanel.transform.TransformPoint(new Vector3(i * busSpacing + originOffset.x, originOffset.y, -1)));
            //lR.SetPosition(1, contentPanel.transform.TransformPoint(new Vector3(i * busSpacing + originOffset.x,
            //    -contentPanel.rect.size.y - originOffset.y, -1)));
            connectionsList.Add(go.GetComponent<UILineRenerer>());
        }
        
        //Размещение тестовых гейтов на сетке
        //for (int y = 0; y < gridHeight; y++)
        //{
        //    GameObject go = Instantiate(andGate);
        //    RectTransform recttrans = go.GetComponent<RectTransform>();
        //    recttrans.SetParent(contentPanel);
        //    Vector2 newPos = new Vector2(0, (-y * gridCellSize.y));
        //    recttrans.localPosition = newPos + originOffset;
        //    recttrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gateSize.y);
        //    recttrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gateSize.x);
        //
        //    gatesGrid[0, y] = go.GetComponent<Gate>();
        //    gateList.Add(go.GetComponent<Gate>());
        //}
    }*/
}
