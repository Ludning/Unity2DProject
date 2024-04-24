using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPTestSceneInstaller : MonoBehaviour
{
    [SerializeField]
    CellularAutomata MapGenerater;

    [SerializeField]
    Camera camera;

    void Awake()
    {
        MapGenerater.OnGeneraterMap();

        CullingSystem cs = camera.gameObject.AddComponent<CullingSystem>();
        cs.OnStartCullingMap(MapGenerater.blockDic);
    }
}