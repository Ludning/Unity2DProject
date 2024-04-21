using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSceneInstaller : SceneInstaller
{
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject bladePrefab;
    [SerializeField]
    GameObject monsterPrefab;

    [SerializeField]
    CellularAutomata MapGenerater;

    [SerializeField]
    Camera camera;

    private void Awake()
    {
        MapGenerater.OnGeneraterMap();

        GameObject map = Instantiate(DataManager.Instance.LoadObject<GameObject>("BossMap"));
        map.transform.position = new Vector3(-100, 0, 0);

        int ran = Random.Range(0, 4);
        GameObject boss;
        switch (ran)
        {
            case 0:
                boss = Instantiate(DataManager.Instance.LoadObject<GameObject>("BigFlyingEye"));
                break;
            case 1:
                boss = Instantiate(DataManager.Instance.LoadObject<GameObject>("GoblinKing"));
                break;
            case 2:
                boss = Instantiate(DataManager.Instance.LoadObject<GameObject>("SuperMushroom"));
                break;
            default:
                boss = Instantiate(DataManager.Instance.LoadObject<GameObject>("UltimateSkeleton"));
                break;
        }
        boss.transform.position = new Vector3(-100, 5, 0);

        MapGenerater.Surface2D.BuildNavMesh();

        Vector2 pos = MapGenerater.RandomSpawnPos();
        SpawnManager.Instance.SpawnPlayer(playerPrefab, bladePrefab, pos + new Vector2(0.5f, 0.5f));

        UIManager.Instance.ShowElement(ElementType.GameStatic);
        UIManager.Instance.ShowElement(ElementType.SkillPanel);

        MonsterDisposition(MapGenerater.blockDic.Values.ToList());

        PortalDisposition(MapGenerater.blockDic.Values.ToList());

        CullingSystem cs = camera.gameObject.AddComponent<CullingSystem>();
        cs.OnStartCullingMap(MapGenerater.blockDic);
    }



    //��Ż ���� ����
    private void PortalDisposition(List<Block> blockList)
    {
        Vector2 pos = blockList[Random.Range(0, blockList.Count)].RandomPlainPos();
        SpawnManager.Instance.SpawnPortal("Portal").transform.position = pos;
    }
    //���� ���� ����
    private void TraderDisposition(List<Block> blockList)
    {
        int count = Random.Range(0, 3);
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = blockList[Random.Range(0, blockList.Count)].RandomPlainPos();
            SpawnManager.Instance.SpawnTrader("Trader").transform.position = pos;
        }
    }
    //�渶�� ��ġ�� �󸶳� �Ұ��ΰ�?
    private void MonsterDisposition(List<Block> blockList)
    {
        foreach (Block block in blockList)
        {
            if(block == blockList.First())
                continue;
            int monsterCount = Random.Range(5, 10);
            RandomSpawn(block, monsterCount);
        }
    }
    
    private void RandomSpawn(Block block, int count = 1)
    {
        int currentCount = count;
        while (currentCount > 0)
        {
            MonsterType monsterType = (MonsterType)Random.Range(0, (int)MonsterType.MaxCount);

            SpawnManager.Instance.SpawnMonster(monsterType, block.RandomPlainPos());
            currentCount--;
        }
    }
}
