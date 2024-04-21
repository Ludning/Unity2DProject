using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField]
    protected CanvasType canvasType;
    public CanvasType CanvasType => canvasType;

    private void Awake()
    {
        UIManager.Instance.Init();
    }

    public virtual void OnEnableElements()
    {

    }
    public virtual void SetContext(string context)
    {

    }

}
