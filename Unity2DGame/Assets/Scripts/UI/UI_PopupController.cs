using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_PopupController : UI_Controller
{
    [SerializeField]
    protected GameObject backElement;
    public void SetBackElement(GameObject go)
    {
        backElement = go;
    }
    public abstract void OnDisablePopupElements();
}
