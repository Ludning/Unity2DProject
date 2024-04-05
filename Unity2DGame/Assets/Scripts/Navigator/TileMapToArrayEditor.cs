using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapToArrayEditor : MonoBehaviour
{
    // ������ ���� ��ũ��Ʈ�̹Ƿ� ���� ���� �ƴϸ� Editor ���� ����� ����ϱ� ���� #if UNITY_EDITOR ���ù��� ����մϴ�.
#if UNITY_EDITOR
    [MenuItem("Custom Tools/Convert Tilemap to Array")]
    public static bool[,] ConvertTilemapToArray()
    {
        // Tilemap ��ü ��������
        GameObject tilemapObject = GameObject.Find("Tilemap"); // Ÿ�ϸ��� �̸��� ���� ������ �����ؾ� �մϴ�.
        if (tilemapObject == null)
        {
            Debug.LogError("Tilemap not found!");
            return null;
        }

        Tilemap tilemap = tilemapObject.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found!");
            return null;
        }

        // Ÿ�ϸ��� ũ�� ��������
        BoundsInt bounds = tilemap.cellBounds;

        // Ÿ�ϸ��� �� Ÿ���� �׷��� ���θ� ������ �迭
        bool[,] tileArray = new bool[bounds.size.x, bounds.size.y];

        // Ÿ�ϸ��� ��� Ÿ���� ��ȸ�ϸ鼭 �׷��� �ִ��� ���θ� �Ǵ��Ͽ� �迭�� ����
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, (int)tilemap.transform.position.z);
                TileBase tile = tilemap.GetTile(tilePosition);
                tileArray[x - bounds.xMin, y - bounds.yMin] = (tile == null); // Ÿ���� �����ϴ� ��� false, ��� �ִ� ��� true
            }
        }

        // �迭 ��� (����׿�)
        PrintTileArray(tileArray);
        return tileArray;
    }

    // �迭�� ����ϴ� �޼��� (����׿�)
    private static void PrintTileArray(bool[,] tileArray)
    {
        string arrayString = "Tile Array:\n";
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
            for (int x = 0; x < tileArray.GetLength(0); x++)
            {
                arrayString += (tileArray[x, y] ? "true" : "false") + "\t";
            }
            arrayString += "\n";
        }
        Debug.Log(arrayString);
    }
#endif
}