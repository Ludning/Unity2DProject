using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnManager : Manager<SpawnManager>
{
    string playerStatusBar = "UI_PlayerStatusBar";
    string monsterStatusBar = "UI_MonsterStatusBar";
    //string bossStatusBar = "UI_BossStatusBar";

    public GameObject SpawnPlayer(GameObject playerPrefab, GameObject bladePrefab, Vector3 pos)
    {
        //플레이어, 무기 생성
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
        GameObject blade = Instantiate(bladePrefab);
        player.name = playerPrefab.name;
        blade.name = bladePrefab.name;

        //카메라 타겟 설정
        Camera camera = Camera.main;
        camera.transform.position = player.transform.position;
        camera.gameObject.GetComponent<CameraFollow>().Target = player.transform;

        //플레이어 초기화
        Player playerComponent = player.GetComponent<Player>();
        playerComponent.Init(playerStatusBar);
        WeaponController weaponController = blade.GetComponent<WeaponController>();
        playerComponent.SetWeaponController(weaponController);
        weaponController.Player = playerComponent;
        GameManager.Instance.player = playerComponent;
        GameManager.Instance.playerController = playerComponent.GetPlayerController();
        return player;
    }
    public GameObject SpawnMonster(MonsterType type, Vector3 pos)
    {
        var prefab = ResourceManager.Instance.GetPrefab(type.ToString());
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        go.transform.position = pos;
        Monster monster = go.GetComponent<Monster>();
        monster.Init(monsterStatusBar);
        GameManager.Instance.AddMonster(monster);
        return go;
    }
    public GameObject SpawnUnit(GameObject prefab)
    {
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        go.GetComponent<Unit>().Init(monsterStatusBar);
        return go;
    }
    public GameObject SpawnPortal(string addressableAssetKey)
    {
        GameObject prefab = ResourceManager.Instance.GetPrefab(addressableAssetKey);
        GameObject go = Instantiate(prefab);
        //go.GetComponent<Portal>().Init();
        return go;
    }
    public GameObject SpawnTrader(string addressableAssetKey)
    {
        GameObject prefab = ResourceManager.Instance.GetPrefab(addressableAssetKey);
        GameObject go = Instantiate(prefab);
        //go.GetComponent<Portal>().Init();
        return go;
    }
}
