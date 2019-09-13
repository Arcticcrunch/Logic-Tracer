using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruthtableCheckbox : MonoBehaviour
{
    public int index = 0;
    public TruthtableGenerator truthtableGenerator;
    public Toggle checkBox;

    private void Awake()
    {
        if (checkBox == null)
            checkBox = GetComponent<Toggle>();
    }

    public void SetValueToTable()
    {
        if (truthtableGenerator != null)
            truthtableGenerator.SetCellTruth(index, checkBox.isOn);
    }
}
