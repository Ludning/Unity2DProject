using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_OverlayController : UI_Controller
{
    private Vector2 size = Vector2.zero;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }
    public void SetPosition(Vector3 vec)
    {

    }
    public Vector3 LeftPosition()
    {
        return Vector3.zero;
    }
    public Vector3 RightPosition()
    {
        return Vector3.zero;
    }
}
