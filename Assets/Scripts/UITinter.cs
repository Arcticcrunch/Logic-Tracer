using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITinter : MonoBehaviour
{
    public bool activateOnStart = false;
    public bool revealOnStart = false;
    public float revealTime = 2f;
    public TintRevealType revealType = TintRevealType.Liniar;
    public Image img;

    private TinterState state = TinterState.Tinted;
    private float tintAlpha = 1;

    private void Awake()
    {
        if (img == null)
            img = GetComponent<Image>();
        if (activateOnStart)
            img.enabled = true;
    }
    private void Start()
    {
        Hide();
    }
    private void Update()
    {
        if (state == TinterState.Hiding)
        {
            tintAlpha -= Time.deltaTime / revealTime;
            if (tintAlpha < 0)
            {
                state = TinterState.Hidden;
                tintAlpha = 0;
            }

            Color c = img.color;
            c.a = tintAlpha;
            img.color = c;
        }
        else if (state == TinterState.Tinting)
        {
            tintAlpha += Time.deltaTime / revealTime;
            if (tintAlpha > 1)
            {
                state = TinterState.Tinted;
                tintAlpha = 1;
            }

            Color c = img.color;
            c.a = tintAlpha;
            img.color = c;
        }
    }

    public void Toggle()
    {
        if (state == TinterState.Tinted)
        {
            Hide();
        }
        else if (state == TinterState.Hidden)
        {
            Tint();
        }
    }
    public void Hide()
    {
        tintAlpha = 1;
        state = TinterState.Hiding;
    }
    public void Tint()
    {
        tintAlpha = 0;
        state = TinterState.Tinting;
    }

    public TinterState State
    {
        get { return state; }
    }
    public float TintAlpha
    {
        get
        {
            return tintAlpha;
        }
    }
}

public enum TinterState
{
    Hidden, Tinted, Hiding, Tinting
}
public enum TintRevealType
{
    Liniar, Sqrt
}
