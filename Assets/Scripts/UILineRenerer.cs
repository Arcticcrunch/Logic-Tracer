using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenerer : MonoBehaviour
{
    private RectTransform thisRect;
    private float lineWidth = 10;
    private float nodeSize = 25;
    private Color lineColor = new Color(0, 255, 2);

    public bool autoUpdate = false;
    public bool showNodes = true;
    public bool nodeOnEachVertex = false;

    #region Prefabs
    public RectTransform line;
    public RectTransform joint;
    public RectTransform node;
    #endregion

    public Vector2[] vertexArr;
    public RectTransform[] lineArr;
    public RectTransform[] jointArr;
    public RectTransform[] nodeArr;

    private void Awake()
    {
        thisRect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        thisRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        thisRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
    }
    private void Update()
    {
        if (autoUpdate && vertexArr != null)
            ConstructLine(vertexArr);
    }

    public void ConstructLine(Vector2 start, Vector2 end, bool alignHorizontal = true)
    {
        Vector2 middlePos = alignHorizontal ? new Vector2(end.x, start.y) : new Vector2(start.x, end.y);
        vertexArr = new Vector2[] { start, middlePos, end };
        ConstructLine(vertexArr);
    }
    public void ConstructLine(Vector2[] positions)
    {
        int vertexCount = positions.Length;
        if (vertexCount < 2)
        {
            Debug.LogError("Невозможно построить линию менее чем из двух вершин!");
            return;
        }
        vertexArr = positions;

        // Удаление существующей линии
        DestroyLine();
        
        // Создание линии
        lineArr = new RectTransform[vertexCount - 1];
        jointArr = new RectTransform[vertexCount];
        nodeArr = new RectTransform[vertexCount];

        // Создание вертексов
        for (int i = 0; i < vertexCount; i++)
        {
            GameObject go = Instantiate(joint.gameObject);
            go.transform.SetParent(gameObject.transform);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.localScale = Vector2.one;
            // HACK: изменить -2 на -1
            if (i < vertexCount - 1)
            {
                Vector2 velocity = positions[i + 1] - positions[i];
                if (i < vertexCount - 2)
                {
                    if (velocity.magnitude < Vector2.kEpsilon)
                    {
                        Vector2 v = positions[i + 2] - positions[i];
                        positions[i + 1] = (v * 0.5f) + positions[i];
                    }
                }
                else
                {
                    if (velocity.magnitude < Vector2.kEpsilon)
                    {
                        Vector2 v = positions[i + 1] - positions[i - 1];
                        positions[i] = (v * 0.5f) + positions[i - 1];
                    }
                }

            }
            rect.localPosition = positions[i];
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineWidth);
            jointArr[i] = rect;
        }

        // Создание линий
        for (int i = 0; i < vertexCount - 1; i++)
        {
            GameObject go = Instantiate(line.gameObject);
            go.transform.SetParent(gameObject.transform);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.localScale = Vector2.one;
            rect.localPosition = positions[i];
            lineArr[i] = rect;

            // Разворот и подгонка линий по размеру
            Vector2 velocity = positions[i + 1] - positions[i];
            float angle = Mathf.Atan(velocity.y / velocity.x) * Mathf.Rad2Deg;
            if (positions[i + 1].x < positions[i].x)
                angle -= 180;
            rect.Rotate(0, 0, angle, Space.Self);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, velocity.magnitude);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
        }

        // Создание нодов
        if (showNodes)
        {
            if (nodeOnEachVertex)
            {
                for (int i = 0; i < vertexArr.Length; i++)
                {
                    GameObject go = Instantiate(node.gameObject);
                    go.transform.SetParent(gameObject.transform);
                    RectTransform rect = go.GetComponent<RectTransform>();
                    rect.localScale = Vector2.one;
                    rect.localPosition = vertexArr[i];
                    nodeArr[i] = rect;
                }
            }
            else
            {
                for (int i = 0; i < vertexArr.Length; i += vertexArr.Length - 1)
                {
                    GameObject go = Instantiate(node.gameObject);
                    go.transform.SetParent(gameObject.transform);
                    RectTransform rect = go.GetComponent<RectTransform>();
                    rect.localScale = Vector2.one;
                    rect.localPosition = vertexArr[i];
                    nodeArr[i] = rect;
                }
            }
        }
        SetLineWidth(lineWidth);
        SetNodeSize(nodeSize);
    }
    private void DestroyLine()
    {
        foreach (RectTransform rectTransform in lineArr)
        {
            Destroy(rectTransform.gameObject);
        }
        foreach (RectTransform rectTransform in jointArr)
        {
            Destroy(rectTransform.gameObject);
        }
        foreach (RectTransform rect in nodeArr)
        {
            Destroy(rect.gameObject);
        }
        lineArr = null;
        jointArr = null;
        nodeArr = null;
    }

    public void SetLineWidth(float width)
    {
        if (width > 0)
        {
            lineWidth = width;
            foreach (RectTransform j in jointArr)
            {
                j.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
                j.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineWidth);
            }
            foreach (RectTransform l in lineArr)
            {
                l.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            }
        }
        else Debug.LogError("Ширина линии не может быть отрицательной!");
    }
    public void SetNodeSize(float size)
    {
        if (size > 0)
        {
            nodeSize = size;
            foreach (RectTransform rect in nodeArr)
            {
                if (rect != null)
                {
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, nodeSize);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, nodeSize);
                }
            }
        }
        else Debug.LogError("Размер node не может быть отрицательным!");
    }
    public void SetColor(Color color)
    {
        lineColor = color;
        foreach (RectTransform rect in nodeArr)
        {
            rect.GetComponent<Image>().color = color;
        }
        foreach (RectTransform rect in jointArr)
        {
            rect.GetComponent<Image>().color = color;
        }
        foreach (RectTransform rect in lineArr)
        {
            rect.GetComponent<Image>().color = color;
        }
    }

    public float GetLineWidth()
    {
        return lineWidth;
    }
    public float GetNodeSize()
    {
        return nodeSize;
    }
    public Color GetLineColor()
    {
        return lineColor;
    }
}
