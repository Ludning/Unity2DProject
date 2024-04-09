using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class MarchingSquares
{
    /*  {0, 0, 0, 0}���ʺ��� 0��° ��Ʈ
    0��° ��Ʈ      1��° ��Ʈ
           ������������
           ��        ��
           ��        ��
           ��        ��
           ��        ��
           ������������
    3��° ��Ʈ      2��° ��Ʈ
    */

    //2��ȭ �� ������
    public bool[,] mapData = null;
    //��Ī ������
    Dictionary<Vector2, MarchingType> MachingData = new Dictionary<Vector2, MarchingType>();
    //���� ������
    public HashSet<Vertex> VertexDatas = new HashSet<Vertex>();
    //���
    public List<List<Vertex>> ContourLines = new List<List<Vertex>>();
    //���� �Ÿ�
    public float distance = 1.0f;
    // ���� ��� ����
    public const double tiltTolerance = 0.1f;

    // Start is called before the first frame update
    public void Init()
    {
        //�� ���� ������ �ҷ�����
        mapData = TileMapToArrayEditor.ConvertTilemapToArray();
        //MarchingSquares������ ����
        GenerateMSData();
        //Patternó��, Edge����
        ProcessPattern();
        //Edge�ܼ�ȭ
        SimplifyGraph(tiltTolerance);
        //��� �и�
        GenerationContourLine();
        //�ﰢȭ
        Triangulation();

        //Debug.Log(EdgeData.Count);
        //StartCoroutine(DrawMS());
    }

    #region DebugDraw
    public IEnumerator DrawMarchingSquares()
    {
        while (true)
        {
            foreach (var data in VertexDatas)
            {
                foreach (var con in data.connectionVertex.Keys)
                {
                    Debug.DrawLine(data.vertex, con.vertex);
                    Debug.DrawLine(data.vertex, data.vertex + Vector2.one * 0.1f, Color.red);
                }
            }
            yield return null;
        }
    }
    public IEnumerator DrawContourLines()
    {
        while (true)
        {
            foreach (var list in ContourLines)
            {
                foreach (var vt in list)
                {
                    foreach(var con in vt.connectionVertex)
                    {
                        Debug.DrawLine(vt.vertex, con.Key.vertex);
                    }
                    Debug.DrawLine(vt.vertex, vt.vertex + Vector2.one * 0.1f, Color.red);
                }
            }
            yield return null;
        }
    }
    #endregion

    #region DelaunayTriangulation
    //�ﰢȭ �Լ�
    public void Triangulation()
    {
        //��� ����� Vertex�� Ž���Ѵ�.
        //MarchingSquares�� ������ ����� ���� �����̴�
        //�׷��Ƿ� ù Vertex�� ��ȸ�ϸ� �ڽ����� ���ƿ� �� ���� �ݺ��Ѵ�
        //�ڽ����� ���ƿ��� ��� �׷��� �ϼ��ǰ� �׷��� �������� ������ Vertex�� Ž���Ѵ�
        //�̷��� �ݺ��Ͽ� ���� ����� �и��ϰ� ������� �����Ѵ�
        //������� ���� ����� Vertex�� �����Ͽ� �ﰢ�� Cell�� �����, Cell�� �� ������ �߽����� ������ �ִ�.

        //ContourLink(ContourLines[0], ContourLines[1]);
        //ContourLink(ContourLines[1], ContourLines[0]);
    }

    //Cell ����
    /*public void ContourLink(List<Vertex> left, List<Vertex> right)
    {
        foreach(var origin in left)
        {
            Vertex temp = null;
            float distance = 0.0f;
            foreach (var search in right)
            {
                float currentDistance = Vector2.Distance(origin.vertex, search.vertex);
                if (temp == null || currentDistance < distance)
                {
                    temp = search;
                    distance = currentDistance;
                }
            }
            origin.Link(temp);
        }
    }*/


    //��� �и�
    public void GenerationContourLine()
    {
        List<Vertex> VertexList = VertexDatas.ToList();
        List<Vertex> tempList;

        while (true)
        {
            tempList = new List<Vertex>();
            AddToContourLine(tempList, VertexList.First());
            ContourLines.Add(tempList);
            foreach (var v in tempList)
                VertexList.Remove(v);
            if (VertexList.Count == 0)
                break;
        }
    }

    public void AddToContourLine(List<Vertex> tempList , Vertex vertex)
    {
        if (tempList.Contains(vertex))
            return;
        tempList.Add(vertex);
        foreach (var vt in vertex.connectionVertex.Keys)
        {
            AddToContourLine(tempList, vt);
        }
    }


    #endregion

    #region ���� �ܼ�ȭ
    // ���Ⱑ ������ ������ �ܼ�ȭ�մϴ�.
    public void SimplifyGraph(double slopeTolerance)
    {
        //vertex ���縮��Ʈ
        bool searchEnd = false;
        bool research = false;
        List<Vertex> vtList = VertexDatas.ToList();

        while(true)
        {
            research = false;
            //��� vertex��ȸ
            foreach (var vertex in vtList)
            {
                // ����� ������ ��ȸ�ϸ鼭 ���� ���
                var slopes = new Dictionary<Vertex, double>();
                foreach (var conVertex in vertex.connectionVertex)
                {
                    slopes[conVertex.Key] = conVertex.Value;
                }
                // ���Ⱑ ����� ���� ã�� ���� ����Ʈ�� �߰�
                foreach (var pair in slopes)
                {
                    foreach (var innerPair in slopes)
                    {
                        if (pair.Key == innerPair.Key)
                            continue;
                        //���� ���Ⱑ ����ġ�� ����� �ʾҴٸ� ó��
                        if (Math.Abs(pair.Value - innerPair.Value) <= slopeTolerance)
                        {
                            pair.Key.Link(innerPair.Key);
                            vertex.UnlinkSelf();
                            vtList.Remove(vertex);
                            research = true;
                            break;
                        }
                    }
                    if (research == true)
                        break;
                }
                if (research == true)
                    break;
                if(vtList.Last() == vertex)
                    searchEnd = true;
            }
            if (searchEnd == true)
                break;
        }
        // ������ ���� �� ����
        ClearGraph();
        /*foreach (var removeVertex in toRemove)
        {
            if (!VertexDatas.TryGetValue(removeVertex, out var vertexData))
                continue;

            foreach (var conVertex in vertexData.connectionVertex.Keys)
            {
                //conVertex.Unlink();
            }
            VertexDatas.Remove(removeVertex);
        }*/
    }
    public void ClearGraph()
    {
        bool searchEnd = false;
        bool research = false;
        while (searchEnd == false)
        {
            research = false;
            foreach (var vertex in VertexDatas)
            {
                if (vertex.connectionVertex.Count == 0)
                {
                    VertexDatas.Remove(vertex);
                    research = true;
                }
                if (research == true)
                    break;
                if (VertexDatas.Last() == vertex)
                    searchEnd = true;
            }
        }
    }
    #endregion

    //�� ������ �߰�
    void ProcessPattern()
    {
        foreach (var data in MachingData)
        {
            int pattern = (int)data.Value;
            Vector2 position = data.Key;

            if (pattern == 0b0001) { HandlePattern0b0001(position); }
            else if (pattern == 0b0010) { HandlePattern0b0010(position); }
            else if (pattern == 0b0011) { HandlePattern0b0011(position); }
            else if (pattern == 0b0100) { HandlePattern0b0100(position); }
            else if (pattern == 0b0101) { HandlePattern0b0101(position); }
            else if (pattern == 0b0110) { HandlePattern0b0110(position); }
            else if (pattern == 0b0111) { HandlePattern0b0111(position); }
            else if (pattern == 0b1000) { HandlePattern0b1000(position); }
            else if (pattern == 0b1001) { HandlePattern0b1001(position); }
            else if (pattern == 0b1010) { HandlePattern0b1010(position); }
            else if (pattern == 0b1011) { HandlePattern0b1011(position); }
            else if (pattern == 0b1100) { HandlePattern0b1100(position); }
            else if (pattern == 0b1101) { HandlePattern0b1101(position); }
            else if (pattern == 0b1110) { HandlePattern0b1110(position); }
            else { HandleDefaultPattern(position); }
        }
    }

    #region HandlePatternMK2
    // �� ���Ͽ� ���� ó���� �޼����
    void HandlePattern0b0001(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Down(position));
        Vertex vertex2 = FindVertex(Left(position));

        vertex1.Link(vertex2);

        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b0010(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Right(position));
        Vertex vertex2 = FindVertex(Down(position));

        vertex1.Link(vertex2);

        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b0011(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Right(position));
        Vertex vertex2 = FindVertex(Left(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b0100(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Top(position));
        Vertex vertex2 = FindVertex(Right(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b0101(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Top(position));
        Vertex vertex2 = FindVertex(Right(position));

        vertex1.Link(vertex2);

        Vertex vertex3 = FindVertex(Down(position));
        Vertex vertex4 = FindVertex(Left(position));

        vertex3.Link(vertex4);

        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
        VertexDatas.Add(vertex3);
        VertexDatas.Add(vertex4);
    }
    void HandlePattern0b0110(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Top(position));
        Vertex vertex2 = FindVertex(Down(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b0111(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Top(position));
        Vertex vertex2 = FindVertex(Left(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b1000(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Left(position));
        Vertex vertex2 = FindVertex(Top(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b1001(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Down(position));
        Vertex vertex2 = FindVertex(Top(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b1010(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Left(position));
        Vertex vertex2 = FindVertex(Top(position));
        vertex1.Link(vertex2);
        Vertex vertex3 = FindVertex(Right(position));
        Vertex vertex4 = FindVertex(Down(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
        VertexDatas.Add(vertex3);
        VertexDatas.Add(vertex4);
    }
    void HandlePattern0b1011(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Right(position));
        Vertex vertex2 = FindVertex(Top(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b1100(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Left(position));
        Vertex vertex2 = FindVertex(Right(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b1101(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Down(position));
        Vertex vertex2 = FindVertex(Right(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandlePattern0b1110(Vector2 position)
    {
        Vertex vertex1 = FindVertex(Left(position));
        Vertex vertex2 = FindVertex(Down(position));
        vertex1.Link(vertex2);
        VertexDatas.Add(vertex1);
        VertexDatas.Add(vertex2);
    }
    void HandleDefaultPattern(Vector2 position) { return; }
    #endregion

    #region DirPosition
    Vector2 Top(Vector2 vec)
    {
        return new Vector2(vec.x + distance, vec.y + distance * 1.5f);
    }
    Vector2 Down(Vector2 vec)
    {
        return new Vector2(vec.x + distance, vec.y + distance * 0.5f);
    }
    Vector2 Left(Vector2 vec)
    {
        return new Vector2(vec.x + distance * 0.5f, vec.y + distance);
    }
    Vector2 Right(Vector2 vec)
    {
        return new Vector2(vec.x + distance * 1.5f, vec.y + distance);
    }
    #endregion

    public Vertex? FindVertex(Vector2 target)
    {
        foreach (var vector in VertexDatas)
            if (vector.vertex.Equals(target))
                return vector;
        // ��ġ�ϴ� Vector2 ��ü�� ���� ���
        return new Vertex(target);
    }

    public void GenerateMSData()
    {
        int temp = 0;
        for (int h = 0; h < mapData.GetLength(0) - 1; h++)
        {
            for (int w = 0; w < mapData.GetLength(1) - 1; w++)
            {
                temp |= (GetLeftTop(w, h) == true) ? 1 << 3 : 0;
                temp |= (GetRightTop(w, h) == true) ? 1 << 2 : 0;
                temp |= (GetRightDown(w, h) == true) ? 1 << 1 : 0;
                temp |= (GetLeftDown(w, h) == true) ? 1 << 0 : 0;

                if (temp != 0 || temp != 15)
                    MachingData.Add(new Vector2(w, h), (MarchingType)temp);
                temp = 0;
            }
        }
    }

    public bool GetLeftTop(int x, int y)
    {
        if (IsOutOfRange(x, y + 1))
            return false;
        //return mapData[y + 1, x];
        return mapData[x, y + 1];
    }
    public bool GetRightTop(int x, int y)
    {
        if (IsOutOfRange(x + 1, y + 1))
            return false;
        //return mapData[y + 1, x + 1];
        return mapData[x + 1, y + 1];
    }
    public bool GetRightDown(int x, int y)
    {
        if (IsOutOfRange(x + 1, y))
            return false;
        //return mapData[y, x + 1];
        return mapData[x + 1, y];
    }
    public bool GetLeftDown(int x, int y)
    {
        if (IsOutOfRange(x, y))
            return false;
        //return mapData[y, x];
        return mapData[x, y];
    }
    public bool IsOutOfRange(int x, int y)
    {
        if (x < 0 || x > mapData.GetLength(1) || y < 0 || y > mapData.GetLength(0))
            return true;
        return false;
    }
}
public class Vertex
{
    public Vector2 vertex;
    public Dictionary<Vertex, double> connectionVertex;

    public bool isClose = false;

    public Vertex(Vector2 vector)
    {
        this.vertex = vector;
        connectionVertex = new Dictionary<Vertex, double>();
    }
    public void Link(Vertex other)
    {
        if (!connectionVertex.ContainsKey(other))
            connectionVertex.Add(other, CalculateSlope(vertex, other.vertex));
        if (!other.connectionVertex.ContainsKey(this))
            other.connectionVertex.Add(this, CalculateSlope(other.vertex, vertex));
    }
    public void Unlink(Vertex other)
    {
        if (connectionVertex.ContainsKey(other))
            connectionVertex.Remove(other);
        if (other.connectionVertex.ContainsKey(this))
            other.connectionVertex.Remove(this);
    }
    public void UnlinkSelf()
    {
        Vertex[] conVer = connectionVertex.Keys.ToArray();
        foreach (Vertex vt in conVer)
        {
            this.Unlink(vt);
        }
    }
    // �� �� ������ ���⸦ ����մϴ�.
    private double CalculateSlope(Vector2 a, Vector2 b)
    {
        if (b.x == a.x) return double.MaxValue; // ������ ó��
        return (b.y - a.y) / (b.x - a.x);
    }
    /*public Vertex NextRandomVertex(Vertex prevVertex)
    {
        foreach (Vertex vt in connectionVertex.Keys)
        {
            if(vt == prevVertex)
                continue;
            Vertex temp = vt;
            prevVertex = vt;
            return temp;
        }
        return null;
    }*/
}
    
    public enum MarchingType
    {
        _0,          //0,    // 0000   ����
        _1,          //1,    // 0001   ����
        _2,          //2,    // 0010   ����
        _3,          //3,    // 0011   ����, ����
        _4,          //4,    // 0100   ���
        _5,          //5,    // 0101   ���, ����
        _6,          //6,    // 0110   ���, ����
        _7,          //7,    // 0111   ���, ����, ����
        _8,          //8,    // 1000   �»�
        _9,          //9,    // 1001   �»�, ����
        _10,          //10,    // 1010  �»�, ����
        _11,          //11,    // 1011  �»�, ����, ����
        _12,          //12,    // 1100  �»�, ���
        _13,          //13,    // 1101  �»�, ���, ����
        _14,          //14,    // 1110  �»�, ���, ����
        _15,          //15,    // 1111  �»�, ���, ����, ����
    }

