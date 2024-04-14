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

    public GameObject SpawnPlayer(GameObject playerPrefab, GameObject bladePrefab)
    {
        //�÷��̾�, ���� ����
        GameObject player = Instantiate(playerPrefab);
        GameObject blade = Instantiate(bladePrefab);
        player.name = playerPrefab.name;
        blade.name = bladePrefab.name;

        //ī�޶� Ÿ�� ����
        Camera.main.gameObject.GetComponent<CameraFollow>().target = player.transform;

        //�÷��̾� �ʱ�ȭ
        Player playerComponent = player.GetComponent<Player>();
        playerComponent.Init(playerStatusBar);
        WeaponController weaponController = blade.GetComponent<WeaponController>();
        playerComponent.SetWeaponController(weaponController);
        weaponController.Player = playerComponent;
        GameManager.Instance.player = playerComponent;
        return player;
    }
    public GameObject SpawnMonster(GameObject prefab)
    {
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        Monster monster = go.GetComponent<Monster>();
        monster.Init(monsterStatusBar);
        GameManager.Instance.AddMonster(monster);
        return go;
    }
    public GameObject Spawn(GameObject prefab)
    {
        GameObject go = ObjectPool.Instance.GetGameObject(prefab);
        go.GetComponent<Unit>().Init(monsterStatusBar);
        return go;
    }
}
