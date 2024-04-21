using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
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

    GameObject hpUIPrefab;

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


    /*public void ShowPollingElement(ElementType elementType)
    {
        if(hpUIPrefab == null)
        {
            hpUIPrefab = Addressables.LoadAssetAsync<GameObject>(elementType.ToString()).WaitForCompletion();
        }
        ObjectPool.Instance.GetGameObject(hpUIPrefab);
        //GetElementData(elementType);
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        ElementData data = elementDic[elementType];
        data.gameObject.SetActive(true);

        GetCanvasData(data).AddCount();
    }*/
    public void HidePollingElement(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        ElementData data = elementDic[elementType];
        data.gameObject.SetActive(false);

        GetCanvasData(elementType).SubCount();
    }

    public void ShowSkillOverlayElement(int index)
    {
        if (!elementDic.ContainsKey(ElementType.InformationOverlay))
            InstantiateElement(ElementType.InformationOverlay);
        ElementData data = elementDic[ElementType.InformationOverlay];
        data.gameObject.SetActive(true);

        List<SkillData> skillDataList = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle").GetAllData();
        SkillData skillData = skillDataList.Find(x => x.skillId == index);
        data.controller.SetContext((skillData == null)? "" : skillData.skillDiscription);

        GetCanvasData(ElementType.InformationOverlay).AddCount();
    }
    public void HideSkillOverlayElement()
    {
        if (!elementDic.ContainsKey(ElementType.InformationOverlay))
            InstantiateElement(ElementType.InformationOverlay);
        ElementData data = elementDic[ElementType.InformationOverlay];
        data.gameObject.SetActive(false);

        GetCanvasData(data.controller.CanvasType).SubCount();
    }

    public void ShowItemOverlayElement(int index)
    {
        if (!elementDic.ContainsKey(ElementType.InformationOverlay))
            InstantiateElement(ElementType.InformationOverlay);
        ElementData data = elementDic[ElementType.InformationOverlay];
        data.gameObject.SetActive(true);

        data.controller.SetContext(ResourceManager.Instance.GetScriptableData<ItemData>("ItemData").items[index].description);

        GetCanvasData(ElementType.InformationOverlay).AddCount();
    }
    public void HideItemOverlayElement()
    {
        if (!elementDic.ContainsKey(ElementType.InformationOverlay))
            InstantiateElement(ElementType.InformationOverlay);
        ElementData data = elementDic[ElementType.InformationOverlay];
        data.gameObject.SetActive(false);

        GetCanvasData(data.controller.CanvasType).SubCount();
    }

    public void ShowElement(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        ElementData data = elementDic[elementType];
        data.gameObject.SetActive(true);

        GetCanvasData(elementType).AddCount();
    }
    public void ShowPopupElement(ElementType back, ElementType front)
    {
        var dataPair = GetPopupElementData(back, front);
        ElementData backData = dataPair.Item1;
        ElementData frontData = dataPair.Item2;

        GetCanvasData(backData.controller.CanvasType);
        GetCanvasData(frontData.controller.CanvasType);

        GetCanvasData(backData.controller.CanvasType).AddCount();
        GetCanvasData(frontData.controller.CanvasType).AddCount();
    }
    public void HideElement(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        ElementData data = elementDic[elementType];
        data.gameObject.SetActive(false);

        GetCanvasData(data.controller.CanvasType).SubCount();
    }
    public void HidePopupElement(ElementType back, ElementType front)
    {
        ElementData backData = elementDic[back];
        ElementData frontData = elementDic[front];
        backData.gameObject.SetActive(false);
        frontData.gameObject.SetActive(false);

        activatedPopupUI.Remove(back);
        activatedPopupUI.Remove(front);

        GetCanvasData(back).SubCount();
        GetCanvasData(front).SubCount();

        //메뉴로 가려진 UI 활성화
        if (front == ElementType.MenuFront)
        {
            foreach (var go in activatedPopupUI.Values)
            {
                go.SetActive(true);
            }
        }

        GameManager.Instance.SubPauseCount();
    }
    public void ShowUI_Status(GameObject go)
    {
        go.transform.parent = GetCanvasData(CanvasType.SceneInformationCanvas).gameObject.transform;
        go.GetComponent<RenderController>().SetUpHpUI();
        GetCanvasData(CanvasType.SceneInformationCanvas).AddCount();
    }
    public CanvasData GetCanvasData(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);
        //canvasDic[canvasType].gameObject.SetActive(true);
        return canvasDic[elementDic[elementType].controller.CanvasType];
    }
    public CanvasData GetCanvasData(CanvasType canvasType)
    {
        if (!canvasDic.ContainsKey(canvasType))
            InstantiateCanvas(canvasType);
        //canvasDic[canvasType].gameObject.SetActive(true);
        return canvasDic[canvasType];
    }
    public void Clear()
    {
        canvasDic.Clear();
        elementDic.Clear();
        activatedPopupUI.Clear();
    }

    #region Element, Canvas 생성, 해제
    private (ElementData, ElementData) GetPopupElementData(ElementType backElementType, ElementType frontElementType)
    {
        ElementData backData = GetElementData(backElementType);
        ElementData frontData = GetElementData(frontElementType);
        UI_PopupController frontPopup = (UI_PopupController)frontData.controller;
        frontPopup.SetBackElement(backData.gameObject);

        if (frontElementType == ElementType.MenuFront)
        {
            foreach (var go in activatedPopupUI.Values)
            {
                go.SetActive(false);
            }
        }
        else if (!activatedPopupUI.ContainsKey(backElementType) && !activatedPopupUI.ContainsKey(frontElementType))
        {
            activatedPopupUI.Add(backElementType, backData.gameObject);
            activatedPopupUI.Add(frontElementType, frontData.gameObject);
            GameManager.Instance.AddPauseCount();
        }

        
        return (backData, frontData);
    }
    private ElementData GetElementData(ElementType elementType)
    {
        if (!elementDic.ContainsKey(elementType))
            InstantiateElement(elementType);

        elementDic[elementType].gameObject.SetActive(true);
        elementDic[elementType].controller.OnEnableElements();
        return elementDic[elementType];
    }
    //캔버스 생성
    private void InstantiateElement(ElementType elementType)
    {
        GameObject prefab = Addressables.LoadAssetAsync<GameObject>(elementType.ToString()).WaitForCompletion();
        GameObject go = Instantiate(prefab);
        elementDic.Add(elementType, new ElementData(go, go.GetComponent<UI_Controller>()));

        CanvasType canvasType = elementDic[elementType].controller.CanvasType;
        CanvasData canvasData = GetCanvasData(canvasType);
        elementDic[elementType].gameObject.SetActive(false);

        go.transform.SetParent(canvasData.gameObject.transform, false);
    }
    private void HideCanvas(CanvasType canvasType)
    {
        if (!canvasDic.ContainsKey(canvasType))
            InstantiateCanvas(canvasType);
        canvasDic[canvasType].gameObject.SetActive(false);
    }

    //캔버스 생성
    private void InstantiateCanvas(CanvasType canvasType)
    {
        GameObject prefab = Addressables.LoadAssetAsync<GameObject>(canvasType.ToString()).WaitForCompletion();
        GameObject go = Instantiate(prefab);
        canvasDic.Add(canvasType, new CanvasData(go, go.GetComponent<Canvas>(), canvasType));
        //HideCanvas(canvasType);
    }
    #endregion
}
public class CanvasData
{
    public GameObject gameObject;
    public Canvas canvas;
    private int count;
    public CanvasData(GameObject gameObject, Canvas canvas, CanvasType canvasType)
    {
        count = 0;
        this.gameObject = gameObject;
        this.canvas = canvas;
        if (this.canvas.renderMode != RenderMode.ScreenSpaceCamera)
            this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        if (this.canvas.worldCamera == null)
            this.canvas.worldCamera = Camera.main;
        this.canvas.sortingLayerName = canvasType.ToString();
    }
    public void AddCount()
    {
        count++;
        canvas.enabled = true;
    }
    public void SubCount()
    {
        count--;
        if(count <= 0)
            canvas.enabled = false;
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


