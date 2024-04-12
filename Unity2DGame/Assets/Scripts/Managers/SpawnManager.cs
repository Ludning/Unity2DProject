using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Manager<SpawnManager>
{
    string playerStatusBar = "UI_HPBar";
    string monsterStatusBar = "UI_MonsterStatusBar";
    //string bossStatusBar = "UI_HPBar";

    public GameObject SpawnPlayer(GameObject playerPrefab, GameObject bladePrefab)
    {
        GameObject player = Instantiate(playerPrefab);
        GameObject blade = Instantiate(bladePrefab);
        player.name = playerPrefab.name;
        blade.name = bladePrefab.name;
        Camera.main.gameObject.GetComponent<CameraFollow>().target = player.transform;
        Player playerComponent = player.GetComponent<Player>();
        playerComponent.Init(playerStatusBar);
        blade.GetComponent<WeaponController>().Player = playerComponent;
        GameManager.Instance.player = playerComponent;
        return player;
    }
    public GameObject SpawnMonster(GameObject prefab)
    {
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        go.GetComponent<Unit>().Init(monsterStatusBar);
        return go;
    }
    public GameObject Spawn(GameObject prefab)
    {
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        go.GetComponent<Unit>().Init(monsterStatusBar);
        return go;
    }
}
