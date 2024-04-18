using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class MapTest : MonoBehaviour
{
    BinarySpacePartition BSP;

    public TileBase tileBase1;//최하위 잔디 등
    public TileBase tileBase2;//도로
    public TileBase tileBase3;//중위 스톤

    public GameObject prefab;

    [SerializeField]
    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        BSP = new BinarySpacePartition(new Vector2Int(70, 70));

        List<BSPNode> bspNodes = BSP.GetLastNode();

        #region 타일생성
        foreach (BSPNode node in bspNodes)
        {
            int rand = Random.Range(0, 3);
            RectInt mapdata = node.mapRect;
            TileBase tileBase;
            switch (rand)
            {
                case 0:
                    tileBase = tileBase1;
                    break;
                case 1:
                    tileBase = tileBase2;
                    break;
                default:
                    tileBase = tileBase3;
                    break;
            }
            for (int y = mapdata.position.y + 1; y < mapdata.height + mapdata.position.y - 1; y++)
            {
                for (int x = mapdata.position.x + 1; x < mapdata.width + mapdata.position.x - 1; x++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);
                }
            }
        }
        #endregion

        GenerationRoute(bspNodes);

    }

    public void GenerationRoute(List<BSPNode> bspNodes)
    {
        var data = BSP.map.GetNearNodes();
        foreach(List<BSPNode> node in data)
        {
            //LinkRoute(node[0], node[1]);
            DrawLine(new Vector2Int((int)node[0].mapRect.center.x, (int)node[0].mapRect.center.y), new Vector2Int((int)node[1].mapRect.center.x, (int)node[1].mapRect.center.y));
        }
    }
    void DrawLine(Vector2Int start, Vector2Int end)
    {
        int x1 = start.x;
        int y1 = start.y;
        int x2 = end.x;
        int y2 = end.y;

        if (x1 == x2) // Vertical line
        {
            int minY = Mathf.Min(y1, y2);
            int maxY = Mathf.Max(y1, y2);
            for (int y = minY; y <= maxY; y++)
            {
                tilemap.SetTile(new Vector3Int(x1, y, 0), tileBase1);
            }
        }
        else if (y1 == y2) // Horizontal line
        {
            int minX = Mathf.Min(x1, x2);
            int maxX = Mathf.Max(x1, x2);
            for (int x = minX; x <= maxX; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y1, 0), tileBase1);
            }
        }
        else // Right angle
        {
            int minX = Mathf.Min(x1, x2);
            int maxX = Mathf.Max(x1, x2);
            int minY = Mathf.Min(y1, y2);
            int maxY = Mathf.Max(y1, y2);
            for (int x = minX; x <= maxX; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y1, 0), tileBase1);
            }
            for (int y = minY; y <= maxY; y++)
            {
                tilemap.SetTile(new Vector3Int(x2, y, 0), tileBase1);
            }
        }
    }
    public void LinkRoute(BSPNode bspNode1, BSPNode bspNode2)
    {
        /*var position1 = bspNode1.RandomPosition();
        var position2 = bspNode2.RandomPosition();*/
        var position1 = bspNode1.mapRect.center;
        var position2 = bspNode2.mapRect.center;
        Debug.DrawLine(new Vector3(position1.x, position1.y, 0), new Vector3(position2.x, position2.y, 0), Color.red, float.MaxValue);
    }

    /*public Color RandColor()
    {
        // 랜덤한 RGB 값 생성
        float randomRed = Random.Range(0f, 1f);
        float randomGreen = Random.Range(0f, 1f);
        float randomBlue = Random.Range(0f, 1f);
        // 알파 값은 보통 1로 설정하여 완전 불투명한 색을 얻습니다.
        float alpha = 1f;
        // Color 생성
        return new Color(randomRed, randomGreen, randomBlue, alpha);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
