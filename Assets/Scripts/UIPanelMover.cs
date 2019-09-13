using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMover : MonoBehaviour
{
    private RectTransform rect;
    private PanelState state = PanelState.On;
    private float translation = 0;
    public float movementSpeed = 2f;
    public MovementMethod movementMethod = MovementMethod.Liniar;
    public Vector2 posOn = Vector2.zero;
    public Vector2 posOff = Vector2.zero;

    private void Awake()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (state == PanelState.TurningOff)
        {
            switch (movementMethod)
            {
                case MovementMethod.Liniar:
                    translation += movementSpeed * Time.deltaTime;
                    break;
                case MovementMethod.Sqrt:
                    break;
                default:
                    break;
            }
            if (translation >= 1)
            {
                translation = 1;
                state = PanelState.Off;
            }
            rect.anchoredPosition = Vector3.Lerp(posOn, posOff, translation);
        }
        else if (state == PanelState.TurningOn)
        {
            switch (movementMethod)
            {
                case MovementMethod.Liniar:
                    translation += movementSpeed * Time.deltaTime;
                    break;
                case MovementMethod.Sqrt:
                    break;
                default:
                    break;
            }
            if (translation >= 1)
            {
                translation = 1;
                state = PanelState.On;
            }
            rect.anchoredPosition = Vector3.Lerp(posOff, posOn, translation);
        }
    }

    public void Toggle()
    {
        if (state == PanelState.On)
        {
            state = PanelState.TurningOff;
            translation = 0;
        }
        else if (state == PanelState.Off)
        {
            state = PanelState.TurningOn;
            translation = 0;
        }
    }
    public void TurnOn()
    {
        if (state == PanelState.Off)
        {
            state = PanelState.TurningOn;
            translation = 0;
        }
    }
    public void TurnOff()
    {
        if (state == PanelState.On)
        {
            state = PanelState.TurningOff;
            translation = 0;
        }
    }



    public enum PanelState
    {
        On, Off, TurningOn, TurningOff
    }

    public enum MovementMethod
    {
        Liniar, Sqrt
    }
}
