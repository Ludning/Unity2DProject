using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class UIManager : Manager<UIManager>
{
    private Dictionary<CanvasType, CanvasData> canvasDic = new Dictionary<CanvasType, CanvasData>();

    public GameObject eventSystem = null;

    public void Init()
    {
        if (eventSystem == null)
        {
            eventSystem = Instantiate(ResourceManager.Instance.GetPrefab("EventSystem"));
            DontDestroyOnLoad(eventSystem);
        }
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

    public void InstantiateCanvas(CanvasType canvasType)
    {
        GameObject prefab = Addressables.LoadAssetAsync<GameObject>(canvasType.ToString()).WaitForCompletion();
        GameObject go = Instantiate(prefab);
        canvasDic.Add(canvasType, new CanvasData(go, go.GetComponent<Canvas>(), go.GetComponent<CanvasController>()));
        HideCanvas(canvasType);
    }

    public void Clear()
    {
        canvasDic.Clear();
    }
}
public class CanvasData
{
    public GameObject gameObject;
    public Canvas canvas;
    public CanvasController controller;
    public CanvasData(GameObject gameObject, Canvas canvas, CanvasController controller)
    {
        this.gameObject = gameObject;
        this.canvas = canvas;
        this.controller = controller;
        if (this.canvas.renderMode != RenderMode.ScreenSpaceCamera)
            this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        if (this.canvas.worldCamera == null)
            this.canvas.worldCamera = Camera.main;
    }
}


