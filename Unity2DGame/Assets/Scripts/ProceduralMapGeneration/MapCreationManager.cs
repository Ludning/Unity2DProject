using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreationManager : MonoBehaviour
{
    public Tilemap tilemapLayer1;//������ �ܵ� ��
    public Tilemap tilemapLayer2;//����
    public Tilemap tilemapLayer3;//���� ����
    public Tilemap tilemapLayer4;//�׸���
    public Tilemap tilemapLayer5;//�� ��ǰ, ������Ʈ
    public TileBase newTile;


    Vector3Int mapSize;
    Vector3Int MapSize { get { return mapSize; } }

    public GameObject grassPrefab;
    public GameObject stoneGroundPrefab;
    public GameObject wallPrefab;

    public List<TileBase> grassTiles;
    public List<TileBase> stoneGroundTiles;
    public List<TileBase> wallTiles;

    public void Init()
    {
        Tilemap grassTilemap = grassPrefab.GetComponentInChildren<Tilemap>();
        Tilemap stoneGroundTilemap = stoneGroundPrefab.GetComponentInChildren<Tilemap>();
        Tilemap wallTilemap = wallPrefab.GetComponentInChildren<Tilemap>();

        grassTiles = grassTilemap.GetTilesBlock(grassTilemap.cellBounds).ToList();
        stoneGroundTiles = stoneGroundTilemap.GetTilesBlock(stoneGroundTilemap.cellBounds).ToList();
        wallTiles = wallTilemap.GetTilesBlock(wallTilemap.cellBounds).ToList();

        grassTiles.RemoveAll(tile => tile == null);
        stoneGroundTiles.RemoveAll(tile => tile == null);
        wallTiles.RemoveAll(tile => tile == null);
    }
    public void SetTile(Vector3Int pos, int layer = 1)
    {
        Tilemap selectTilemap;
        switch (layer)
        {
            case 1:
                selectTilemap = tilemapLayer1;
                break;
            case 2:
                selectTilemap = tilemapLayer2;
                break;
            case 3:
                selectTilemap = tilemapLayer3;
                break;
            default:
                selectTilemap = tilemapLayer1;
                break;
        }
        selectTilemap.SetTile(pos, newTile);
    }

    private void Start()
    {
        Init();
        Debug.Log($"grassTiles : {grassTiles.Count} // 64");
        Debug.Log($"stoneGroundTiles : {stoneGroundTiles.Count} // 50");
        Debug.Log($"wallTiles : {wallTiles.Count} // 65");
    }

    /*
    Binary Space Partitioning�� ���� ����
    herringbone wang tiles�� ���� ����
    �׵θ��� wall�� �ѷ��α�
    */
}