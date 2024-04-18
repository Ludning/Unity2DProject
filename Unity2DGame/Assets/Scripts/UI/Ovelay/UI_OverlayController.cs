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
        size = new Vector2(rectTransform.rect.width, -rectTransform.rect.height);
    }
    public override void OnEnableElements()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width, Screen.height) * 0.5f + size * 0.6f;
        transform.localPosition = mousePosition;
    }
    private void FixedUpdate()
    {
        //화면의 왼쪽에 있으면 오른쪽으로 출력
        //화면의 오른쪽에 있으면 왼쪽에 출력
        Debug.Log(Mouse.current.position.ReadValue());
        Debug.Log(size);
        Vector2 mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width, Screen.height) * 0.5f + size * 0.6f;
        transform.localPosition = mousePosition;
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
