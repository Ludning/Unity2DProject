using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class MapTest : MonoBehaviour
{
    BinarySpacePartition BSP;

    public TileBase tileBase1;//������ �ܵ� ��
    public TileBase tileBase2;//����
    public TileBase tileBase3;//���� ����

    public GameObject prefab;

    [SerializeField]
    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        #region TEST
        //GameObject go = Instantiate(prefab);
        //go.GetComponentInChildren<SpriteRenderer>().color;
        #endregion

        BSP = new BinarySpacePartition(new Vector2Int(100, 100));

        List<BSPNode> bspNode = BSP.GetLastNode();

        foreach (BSPNode node in bspNode)
        {
            #region Ÿ�ϻ���
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
            for (int y = mapdata.position.y; y < mapdata.height + mapdata.position.y; y++)
            {
                for (int x = mapdata.position.x; x < mapdata.width + mapdata.position.x; x++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);
                }
            }
            #endregion

            /*Color color = RandColor();
            RectInt mapdata = node.mapRect;

            GameObject go = Instantiate(prefab, new Vector3(mapdata.position.x, mapdata.position.y, 0), new Quaternion());
            go.GetComponentInChildren<SpriteRenderer>().color = color;
            go.transform.localScale = new Vector3(mapdata.width, mapdata.height, 1);

            for (int y = mapdata.position.y; y < mapdata.height + mapdata.position.y; y++)
            {
                for (int x = mapdata.position.x; x < mapdata.width + mapdata.position.x; x++)
                {
                    //tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);
                    
                }
            }*/
        }
    }
    /*public Color RandColor()
    {
        // ������ RGB �� ����
        float randomRed = Random.Range(0f, 1f);
        float randomGreen = Random.Range(0f, 1f);
        float randomBlue = Random.Range(0f, 1f);
        // ���� ���� ���� 1�� �����Ͽ� ���� �������� ���� ����ϴ�.
        float alpha = 1f;
        // Color ����
        return new Color(randomRed, randomGreen, randomBlue, alpha);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
