using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZoomer : MonoBehaviour
{
    public RectTransform scrollView;
    public RectTransform parentPanel;
    public RectTransform targetPanel;
    public Slider slider;
    public float maxZoom = 3f;
    public float minZoom = 0.25f;

    private void Awake()
    {
        slider.maxValue = maxZoom;
        slider.minValue = minZoom;
        slider.value = 1;
    }

    public void Start()
    {
        UpdateZoom();
    }
    public void UpdateZoom()
    {
        if (parentPanel != null)
        {
            parentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollView.rect.width * slider.value);
            parentPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollView.rect.width * slider.value);
        }
        targetPanel.localScale = new Vector3(slider.value, slider.value, 1);
    }
}
