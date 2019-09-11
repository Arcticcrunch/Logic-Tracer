using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class TruthtableGenerator : MonoBehaviour
{
    public float cellSize = 50f;
    public Vector2 originOffset = new Vector2(10, -10);
    private int matrixWidth = 1;
    private int matrixHeight = 1;
    private bool[,] matrix;
    private bool[] truthColumn;

    public Settings settings;
    public RectTransform contentPanel;
    public List<GameObject> cellsList;

    #region Prefabs
    public RectTransform checkbox;
    public RectTransform emptyCell;
    //public RectTransform cellOne;
    //public RectTransform cellZero;
    #endregion

    public void Start()
    {
        cellsList = new List<GameObject>();
    }
    public void GenerateTruthtable()
    {
        matrixWidth = settings.GetInputsCount;
        matrixHeight = (int)Mathf.Pow(2, matrixWidth);
        matrix = new bool[matrixWidth, matrixHeight];
        truthColumn = new bool[matrixHeight];

        // Заполнение матрицы
        for (int y = 0; y < matrixHeight; y++)
        {
            for (int x = 1; x < matrixWidth + 1; x++)
            {
                int pos = Math.ClampMin(0, x - 1);
                int reference = y >> pos;
                reference = reference << pos;
                int temp = y >> x;
                temp = temp << x;
                matrix[matrixWidth - x, y] = reference - temp == 0 ? false : true;
            }
        }

        // Отрисовка матрицы
        if (cellsList != null)
        {
            // Очистка существующей матрицы
            foreach (GameObject go in cellsList)
            {
                GameObject.Destroy(go);
            }
            cellsList.Clear();
        }
        // Изменение размера родительской панели
        contentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (originOffset.x * 2) + (matrixWidth * cellSize) + cellSize);
        contentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (originOffset.y * 2) + (matrixHeight * cellSize) + cellSize);
        
        for (int y = 0; y < matrixHeight; y++)
        {
            for (int x = 0; x < matrixWidth; x++)
            {
                GameObject go = (GameObject)GameObject.Instantiate(emptyCell.gameObject);
                cellsList.Add(go);
                RectTransform rectTrans = go.GetComponent<RectTransform>();
                rectTrans.SetParent(contentPanel);
                rectTrans.localScale = Vector3.one;
                rectTrans.localPosition = originOffset + new Vector2(x * cellSize, -y * cellSize) + new Vector2(cellSize, 0);
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
                string txt = matrix[x,y] == true ? "1" : "0";
                go.GetComponentInChildren<Text>().text = txt;
            }
        }
        for (int i = 0; i < matrixHeight; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(checkbox.gameObject);
            cellsList.Add(go);
            RectTransform rectTrans = go.GetComponent<RectTransform>();
            rectTrans.SetParent(contentPanel);
            rectTrans.localScale = Vector3.one;
            rectTrans.localPosition = originOffset + new Vector2(0, -i * cellSize);
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
        }
    }

    public int GetMatrixWidth()
    {
        return matrixWidth;
    }
    public int GetMatrixHeight()
    {
        return matrixHeight;
    }
    public bool[,] GetMatrix()
    {
        return matrix;
    }
    public bool[] GetTruthColumn()
    {
        return truthColumn;
    }

    public void SetCellTruth(int pos, bool state)
    {
        if (truthColumn != null)
        {
            if (Math.IsInArrayRange(pos, 0, matrixHeight))
            {
                truthColumn[pos] = state;
            }
            else Debug.LogError("Число вне границ массива!");
        }
        else Debug.LogError("Колонка истиннности равна null!");
    }
}
