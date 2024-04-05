using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapToArrayEditor : MonoBehaviour
{
    // 에디터 전용 스크립트이므로 실행 중이 아니면 Editor 전용 기능을 사용하기 위해 #if UNITY_EDITOR 지시문을 사용합니다.
#if UNITY_EDITOR
    [MenuItem("Custom Tools/Convert Tilemap to Array")]
    public static bool[,] ConvertTilemapToArray()
    {
        // Tilemap 객체 가져오기
        GameObject tilemapObject = GameObject.Find("Tilemap"); // 타일맵의 이름에 따라 적절히 수정해야 합니다.
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

        // 타일맵의 크기 가져오기
        BoundsInt bounds = tilemap.cellBounds;

        // 타일맵의 각 타일의 그려진 여부를 저장할 배열
        bool[,] tileArray = new bool[bounds.size.x, bounds.size.y];

        // 타일맵의 모든 타일을 순회하면서 그려져 있는지 여부를 판단하여 배열에 저장
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, (int)tilemap.transform.position.z);
                TileBase tile = tilemap.GetTile(tilePosition);
                tileArray[x - bounds.xMin, y - bounds.yMin] = (tile == null); // 타일이 존재하는 경우 false, 비어 있는 경우 true
            }
        }

        // 배열 출력 (디버그용)
        PrintTileArray(tileArray);
        return tileArray;
    }

    // 배열을 출력하는 메서드 (디버그용)
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