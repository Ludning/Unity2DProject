using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_OverlayController : UI_Controller
{
    private Vector2 size = Vector2.zero;
    RectTransform rectTransform;
    [SerializeField]
    TextMeshProUGUI textComponent;
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
    private void LateUpdate()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width, Screen.height) * 0.5f + size * 0.6f;
        transform.localPosition = mousePosition;
    }
    public override void SetContext(string context)
    {
        textComponent.text = context;
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
