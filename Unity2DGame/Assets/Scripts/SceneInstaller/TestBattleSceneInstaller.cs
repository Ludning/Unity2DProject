using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleSceneInstaller : SceneInstaller
{
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject bladePrefab;

    [SerializeField]
    GameObject monsterPrefab;
    private void Awake()
    {
        SpawnManager.Instance.SpawnPlayer(playerPrefab, bladePrefab, Vector3.zero);
        SpawnManager.Instance.SpawnMonster(MonsterType.Goblin, Vector3.zero);
    }
}
