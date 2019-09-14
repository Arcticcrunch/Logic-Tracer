using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public const int MAX_INPUTS = 8;
    public const int MAX_OUTPUTS = 4;

    private int inputsCount = 1;
    private int outputsCount = 1;
    private bool autoUpdate = false;
    private FormulaType formulaType = FormulaType.Conjunction;

    public TruthtableGenerator generator;
    
    public InputField inputCountField;
    public InputField outputCountField;
    public Dropdown formulaTypeDropdown;
    public Toggle autoUpdateToggle;

    public int GetInputsCount
    {
        get
        {
            return inputsCount;
        }
        
    }
    public int GetOutputsCount
    {
        get
        {
            return outputsCount;
        }
    }
    public bool GetAutoUpdateState
    {
        get
        {
            return autoUpdate;
        }
    }
    public FormulaType GetFormulaType
    {
        get
        {
            return formulaType;
        }
    }

    private void Start()
    {
        UpdateInputCountFunc();
        UpdateOutputCountFunc();
        UpdateAutoUpdFunc();
        UpdateFormulaTypeFunc();

        generator.GenerateTruthtable();
    }

    public void UpdateInputCountFunc()
    {
        int value = Convert.ToInt32(inputCountField.text);
        inputsCount = Mathf.Clamp(value, 1, MAX_INPUTS);
        inputCountField.text = inputsCount.ToString();
    }
    public void UpdateOutputCountFunc()
    {
        int value = Convert.ToInt32(outputCountField.text);
        outputsCount = Mathf.Clamp(value, 1, MAX_OUTPUTS);
        outputCountField.text = outputsCount.ToString();
    }
    public void UpdateAutoUpdFunc()
    {
        autoUpdate = autoUpdateToggle.isOn;
    }
    public void UpdateFormulaTypeFunc()
    {
        formulaType = (FormulaType)formulaTypeDropdown.value;
    }
    
    public void Export()
    {

    }
}

public enum FormulaType
{
    Conjunction, Disjunction, Auto
}
