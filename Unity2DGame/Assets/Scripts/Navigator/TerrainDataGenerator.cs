using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDataGenerator : MonoBehaviour
{
    public int width; // 배열의 가로 길이
    public int height; // 배열의 세로 길이
    public float distance;

    bool[,] terrainData;

    private bool coroutineComplete = false;

    private Coroutine dataProcessingCoroutine;

    LinkedList<Vector2> edges = new LinkedList<Vector2>();

    private void Start()
    {
        terrainData = new bool[width, height];
        //GenerateTerrainData();

        dataProcessingCoroutine = StartCoroutine(ProcessData());
    }

    private void Update()
    {
        if (coroutineComplete == true)
        {
            /*for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = (terrainData[y, x]) ? Color.blue : Color.red;
                    Gizmos.color = color;

                    Gizmos.DrawCube(new Vector2(x, y) * distance, new Vector2(distance, distance));
                }
            }*/

            /*LinkedListNode<Vector2> node = edges.First;
            while (node.Next != null)
            {
                Gizmos.DrawLine(node.Value, node.Next.Value);
            }*/

            coroutineComplete = false;
            StartCoroutine(DrawLine());
        }

        Debug.DrawLine(new Vector2(0, 0), new Vector2(0, height) * distance, Color.red);
        Debug.DrawLine(new Vector2(0, 0), new Vector2(width, 0) * distance, Color.red);
        Debug.DrawLine(new Vector2(width, 0) * distance, new Vector2(width, height) * distance, Color.red);
        Debug.DrawLine(new Vector2(0, height) * distance, new Vector2(width, height) * distance, Color.red);
    }
    public IEnumerator DrawLine()
    {
        LinkedListNode<Vector2> node = edges.First;
        LinkedListNode<Vector2> left;
        LinkedListNode<Vector2> right;
        while (true)
        {
            Debug.Log("DrawingLine");
            if (node.Next != null )
            {
                left = node;
                right = node.Next;
                node = node.Next;
            }
            else
            {
                left = node;
                right = edges.First;
                node = edges.First;
            }
            Debug.DrawLine(left.Value, right.Value, Color.red);
            Debug.Log($"{left.Value}, {right.Value}");
            yield return null;
        }
    }

    public IEnumerator ProcessData()
    {
        //각 점을 리스트에 추가하기, 한쪽에 같지않은 bool이 있어도 넣기
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Collider2D collider = Physics2D.OverlapBox(new Vector2(x, y) * distance, new Vector2(distance, distance), 0);
                terrainData[y, x] = (collider == null) ? false : true;

                if(terrainData[y, x] == false)
                {
                    if (y + 1 < height && terrainData[y, x] != terrainData[y + 1, x])
                    {
                        edges.AddLast(new Vector2(x, y) * distance);
                    }
                    else if (y - 1 >= 0 && terrainData[y, x] != terrainData[y - 1, x])
                    {
                        edges.AddLast(new Vector2(x, y) * distance);
                    }
                    else if (x + 1 < width && terrainData[y, x] != terrainData[y, x + 1])
                    {
                        edges.AddLast(new Vector2(x, y) * distance);
                    }
                    else if (x - 1 >= 0 && terrainData[y, x] != terrainData[y, x - 1])
                    {
                        edges.AddLast(new Vector2(x, y) * distance);
                    }
                }
                yield return null;
            }
        }
        Debug.Log("ProcessData is Succese");
        coroutineComplete = true;
        yield break;
    }


    // Collider가 있는 지점은 false, 빈 곳은 true로 표시하는 2차원 배열 생성
    public bool[,] GenerateTerrainData()
    {
        bool[,] terrainData = new bool[width, height];

        // 모든 좌표에 대해 Collider 체크
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 해당 좌표에 Collider가 있는지 확인
                Vector2 worldPosition = new Vector2(x, y); // 좌표를 world 좌표로 변환
                Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition); // 좌표 위치의 Collider 검사

                // Collider가 없으면 빈 곳(true), 있으면 지형이 있는 곳(false)으로 표시
                terrainData[x, y] = colliders.Length == 0;
            }
        }
        return terrainData;
    }

    /*private void OnDrawGizmos()
    {
        *//*int width = 100;
        int height = 100;
        float distance = 0.3f;
        bool[,] terrainData = new bool[width, height];

        LinkedList<Vector2> edges = new LinkedList<Vector2>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Collider2D collider = Physics2D.OverlapBox(new Vector2(x, y) * distance, new Vector2(distance, distance), 0);
                terrainData[y, x] = (collider == null) ? false : true;


                if (y + 1 < height && terrainData[y, x] != terrainData[y + 1, x])
                {
                    edges.AddLast(new Vector2(x, y) * distance);
                }
                else if (y - 1 >= 0 && terrainData[y, x] != terrainData[y - 1, x])
                {
                    edges.AddLast(new Vector2(x, y) * distance);
                }
                else if (x + 1 < width && terrainData[y, x] != terrainData[y, x + 1])
                {
                    edges.AddLast(new Vector2(x, y) * distance);
                }
                else if (x - 1 >= 0 && terrainData[y, x] != terrainData[y, x - 1])
                {
                    edges.AddLast(new Vector2(x, y) * distance);
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = (terrainData[y, x]) ? Color.blue : Color.red;
                Gizmos.color = color;

                Gizmos.DrawCube(new Vector2(x, y) * distance, new Vector2(distance, distance));
            }
        }

        LinkedListNode<Vector2> node = edges.First;
        while (node.Next != null)
        {
            Gizmos.DrawLine(node.Value, node.Next.Value);
        }*//*

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(0, 0), new Vector2(0, height) * distance);
        Gizmos.DrawLine(new Vector2(0, 0), new Vector2(width, 0) * distance);
        Gizmos.DrawLine(new Vector2(width, 0) * distance, new Vector2(width, height) * distance);
        Gizmos.DrawLine(new Vector2(0, height) * distance, new Vector2(width, height) * distance);
    }*/
}
