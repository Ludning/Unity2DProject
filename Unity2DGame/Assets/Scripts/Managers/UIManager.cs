using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    private Dictionary<CanvasType, CanvasData> canvasDic = new Dictionary<CanvasType, CanvasData>();

    private Dictionary<ElementType, ElementData> elementDic = new Dictionary<ElementType, ElementData>();

    private GameObject eventSystem = null;

    private Dictionary<ElementType, GameObject> activatedPopupUI = new Dictionary<ElementType, GameObject>();

    public GameObject EventSystem 
    {
        get
        {
            if (eventSystem == null)
            {
                eventSystem = Instantiate(ResourceManager.Instance.GetPrefab("EventSystem"));
                DontDestroyOnLoad(eventSystem);
            }
            return eventSystem;
        }
    }

    public void Init()
    {
        if (eventSystem == null)
        {
            eventSystem = Instantiate(ResourceManager.Instance.GetPrefab("EventSystem"));
            DontDestroyOnLoad(eventSystem);
        }
    }
    public ElementData GetPopupElementData(ElementType backElementType, ElementType frontElementType)
    {
        ElementData backData = GetElementData(backElementType);
        ElementData frontData = GetElementData(frontElementType);
        UI_PopupController frontPopup = (UI_PopupController)frontData.controller;
        frontPopup.SetBackElement(backData.gameObject);

        if(frontElementType == ElementType.MenuFront)
        {
            foreach (var go in activatedPopupUI.Values)
            {
                go.SetActive(false);
            }
        }
        else
        {
            activatedPopupUI.Add(backElementType, backData.gameObject);
            activatedPopupUI.Add(frontElementType, frontData.gameObject);
        }

        GameManager.Instance.AddPauseCount();
        return frontData;
    }
    public ElementData GetElementData(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        elementDic[elementType].gameObject.SetActive(true);
        elementDic[elementType].controller.OnEnableElements();
        return elementDic[elementType];
    }
    //牡滚胶 积己
    public void InstantiateElement(ElementType elementType)
    {
        GameObject prefab = Addressables.LoadAssetAsync<GameObject>(elementType.ToString()).WaitForCompletion();
        GameObject go = Instantiate(prefab);
        elementDic.Add(elementType, new ElementData(go, go.GetComponent<UI_Controller>()));

        CanvasType canvasType = elementDic[elementType].controller.CanvasType;
        CanvasData canvasData = GetCanvasData(canvasType);

        go.transform.SetParent(canvasData.gameObject.transform, false);
        
        HideElement(elementType);
    }
    public void HidePopupElement(ElementType back, ElementType front)
    {
        HideElement(back);
        HideElement(front);

        activatedPopupUI.Remove(back);
        activatedPopupUI.Remove(front);

        if (front == ElementType.MenuFront)
        {
            foreach (var go in activatedPopupUI.Values)
            {
                go.SetActive(true);
            }
        }

        GameManager.Instance.SubPauseCount();
    }
    public void HideElement(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        elementDic[elementType].gameObject.SetActive(false);
    }


    public CanvasData GetCanvasData(CanvasType canvasType)
    {
        if (!canvasDic.ContainsKey(canvasType))
            InstantiateCanvas(canvasType);
        canvasDic[canvasType].gameObject.SetActive(true);
        return canvasDic[canvasType];
    }

    public void HideCanvas(CanvasType canvasType)
    {
        if (!canvasDic.ContainsKey(canvasType))
            InstantiateCanvas(canvasType);
        canvasDic[canvasType].gameObject.SetActive(false);
    }

    //牡滚胶 积己
    public void InstantiateCanvas(CanvasType canvasType)
    {
        GameObject prefab = Addressables.LoadAssetAsync<GameObject>(canvasType.ToString()).WaitForCompletion();
        GameObject go = Instantiate(prefab);
        canvasDic.Add(canvasType, new CanvasData(go, go.GetComponent<Canvas>(), canvasType));
        HideCanvas(canvasType);
    }

    public void Clear()
    {
        canvasDic.Clear();
        elementDic.Clear();
        activatedPopupUI.Clear();
    }
}
public class CanvasData
{
    public GameObject gameObject;
    public Canvas canvas;
    public CanvasData(GameObject gameObject, Canvas canvas, CanvasType canvasType)
    {
        this.gameObject = gameObject;
        this.canvas = canvas;
        if (this.canvas.renderMode != RenderMode.ScreenSpaceCamera)
            this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        if (this.canvas.worldCamera == null)
            this.canvas.worldCamera = Camera.main;
        this.canvas.sortingLayerName = canvasType.ToString();
    }
}
public class ElementData
{
    public GameObject gameObject;
    public UI_Controller controller;
    public ElementData(GameObject gameObject, UI_Controller controller)
    {
        this.gameObject = gameObject;
        this.controller = controller;
    }
}


