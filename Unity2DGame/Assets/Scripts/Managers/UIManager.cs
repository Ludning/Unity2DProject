using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Manager<UIManager>
{

    Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                GameObject go = GameObject.Find("Canvas");
                if (go == null)
                {
                    go = new GameObject() { name = "Canvas" };
                }
                if(!go.TryGetComponent(out canvas))
                {
                    canvas = go.AddComponent<Canvas>();
                }

                if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                if (canvas.worldCamera == null)
                    canvas.worldCamera = Camera.main;
            }
            return canvas;
        }
    }
}


