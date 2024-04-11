using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Manager<SpawnManager>
{
    public GameObject SpawnPlayer(GameObject prefab)
    {
        GameObject player = Spawn(prefab);
        Camera.main.gameObject.GetComponent<CameraFollow>().target = player.transform;
        return player;
    }
    public GameObject SpawnMonster(GameObject prefab)
    {
        return Spawn(prefab);
    }
    public GameObject Spawn(GameObject prefab)
    {
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        go.GetComponent<Unit>().Init();
        return go;
    }
}
