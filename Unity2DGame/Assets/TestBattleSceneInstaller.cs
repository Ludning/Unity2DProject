using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleSceneInstaller : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject monsterPrefab;
    private void Awake()
    {
        //ObjectPool.Instance.GetGameObject(playerPrefab);
        ObjectPool.Instance.GetGameObject(monsterPrefab);
    }
    private void OnDestroy()
    {
        ObjectPool.Instance.SceneRig = null;
    }
}
