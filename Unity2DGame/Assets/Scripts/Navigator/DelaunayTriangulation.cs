using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UIElements;


public class DelaunayTriangulation
{
    //��γ� �ﰢȭ
    public void GenerationDelaunayTriangulation()
    {
        
    }
    //�������� ���ϴ� �ڵ�

    //�Ϲ� �ﰢȭ
    public Dictionary<Vector2, List<Vertex>> GenerationNormalTriangulation(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();
        foreach (var vt in VertexDatas)
        {
            List<Vertex> triangle = null;
            //��� ������ ���� �Ÿ��� �����ϰ� ������� ���� �� �� �� 3���� ������ vertex�� ����� List�� ��ȯ
            var cellVertices = VertexDatas.Where(V => V != vt)
                                  .Select(V => new { vertex = V, Distance = Distance(vt, V) })
                                  .OrderBy(V => V.Distance)
                                  .Select(V => V.vertex)
                                  .ToList();

            bool searchEnd = true;
            Vector2 centroidVec = Vector2.zero;
            //���ǿ� �����ϴ� �ﰢ���� ã�� �� ����
            for (var i = 0; i < cellVertices.Count; i++)
            {
                for (var k = 0; k < cellVertices.Count; k++)
                {
                    if (cellVertices[i] == cellVertices[k])
                        continue;

                    triangle = new List<Vertex>() { vt, cellVertices[i], cellVertices[k] };
                    var centroid = Centroid(triangle);
                    //�����ؾ� �� ������????
                    //���� �����߽��� �ܺθ� ������ �ʴ´�
                    if (!CheakTriangleCentroid(mapData, distance, centroid))
                        continue;
                    //�ٸ� ���� �������� �ʴ´�(��� ���� ���)
                    if (!CheakCross(VertexDatas, vt, cellVertices[i], cellVertices[k]))
                        continue;

                    //���� ��, ���� ���� ����
                    //Unlink(ContourLines, vt, cellVertices[i], cellVertices[k]);

                    vt.Link(cellVertices[i]);
                    vt.Link(cellVertices[k]);
                    cellVertices[i].Link(cellVertices[k]);
                    //�����ϸ� �ٷ� ������ �ȴ�
                    
                    nearestPoints.Add(centroid, triangle);
                    return nearestPoints;
                }
            }
        }
        return null;
    }

    public void GenerationNormalTriangulationMK2(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        //���� �� ����� �����´�
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();
        //������ ���
        var otherVerties = ContourLines.Where(x => x != longestList).ToList().SelectMany(x => x).ToList();

        //otherVerties ����ȵ� ���� ���� ó��
        foreach (var vt in longestList)
        {
            var vtListOrderByDis = otherVerties.Where(x => x != vt)
                                                .Select(x => new { vertex = x, distance = Distance(vt, x) })
                                                .OrderBy(x => x.distance)
                                                .Select(x => x.vertex)
                                                .Take(1)
                                                .ToList();
            vt.Link(vtListOrderByDis[0]);
        }
    }
    public void GenerationNormalTriangulationMK3(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        //���� �� ����� �����´�
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();
        //������ ���
        var otherVerties = ContourLines.Where(x => x != longestList).ToList().SelectMany(x => x).ToList();

        Vertex startVt = VertexDatas.First();
        Vertex endVt = startVt.connectionVertex.First().Key;

        //otherVerties ����ȵ� ���� ���� ó��
        foreach (var vt in longestList)
        {
            var vtListOrderByDis = otherVerties.Where(x => x != vt)
                                                .Select(x => new { vertex = x, distance = Distance(vt, x) })
                                                .OrderBy(x => x.distance)
                                                .Select(x => x.vertex)
                                                .Take(1)
                                                .ToList();
            vt.Link(vtListOrderByDis[0]);
        }
    }

    //�� �� ������ �Ÿ� ����
    public float Distance(Vertex left, Vertex right)
    {
        return (float)Mathf.Sqrt((left.vertex.x - right.vertex.x) * (left.vertex.x - right.vertex.x) + (left.vertex.y - right.vertex.y) * (left.vertex.y - right.vertex.y));
    }
    //���� �����߽��� ���ϴ� �Լ�
    public Vector2 Centroid(List<Vertex> triangleVertices)
    {
        if (triangleVertices == null || triangleVertices.Count() != 3)
            return Vector2.zero;

        Vector2 sum = Vector2.zero;
        foreach (Vertex vertex in triangleVertices)
        {
            sum += vertex.vertex;
        }
        Vector2 centroid = new Vector2(sum.x * (1.0f / 3.0f), sum.y * (1.0f / 3.0f));

        return centroid;
    }

    //���� ��, ���� ���� ����
    public List<Vertex> Unlink(List<List<Vertex>> ContourLines, Vertex vt1, Vertex vt2, Vertex vt3)
    {
        List<Vertex> vertices = new List<Vertex>();
        bool endSearch = false;

        foreach (List<Vertex> contourLine in ContourLines)
        {
            vertices.Concat(contourLine.Where(x => x == vt1 || x == vt2 || x == vt3).ToList()).ToList();
        }
        for (int i = 0; i < vertices.Count; i++)
        {
            for (int k = 0; k < vertices.Count; k++)
            {
                if (i == k) continue;
                if(vertices[i].connectionVertex.ContainsKey(vertices[k]))
                {
                    vertices[i].Unlink(vertices[k]);
                    endSearch = true;
                    break;
                }
            }
            if (endSearch) break;
        }
        return vertices;
    }

    //�ﰢ���� �����߽��� �ܺ���ġ�� �ִ��� Ȯ��(�����߽ɰ� �������̿��� �ܺΰ����� ���� ��� Ž���Ұ�)
    public bool CheakTriangleCentroid(bool[,] mapData, float distance, Vector2 Centroid)
    {
        Vector2 temp = Centroid / distance;
        return mapData[(int)temp.y, (int)temp.x];
    }
    //�ٸ� ���� �����Ǵ��� üũ�ϴ� �Լ�(���� Ž���Ұ�)
    public bool CheakCross(HashSet<Vertex> vertices, Vertex vt1, Vertex vt2, Vertex vt3)
    {
        List<Vertex> triVertices = new List<Vertex>() { vt1, vt2, vt3 };
        foreach (var vertex in vertices)
        {
            foreach (var conVt in vertex.connectionVertex.Keys)
            {
                if (IsIntersect(triVertices[0], triVertices[1], vertex, conVt))
                    return false;
                if (IsIntersect(triVertices[1], triVertices[2], vertex, conVt))
                    return false;
                if (IsIntersect(triVertices[2], triVertices[0], vertex, conVt))
                    return false;
            }
        }
        return true;
    }
    // ���� ���� Ȯ�� �Լ�
    public bool IsIntersect(Vertex originLeft, Vertex originRight, Vertex otherLeft, Vertex otherRight)
    {
        Vector2 d1 = originRight.vertex - originLeft.vertex;
        Vector2 d2 = otherRight.vertex - otherLeft.vertex;
        Vector2 d3 = originLeft.vertex - otherLeft.vertex;

        float cross1 = Cross(d1, d2);
        float cross2 = Cross(d3, d2);
        float cross3 = Cross(d3, d1);

        if (cross1 == 0) return false; // �����ϰų� �������� ����

        float t1 = cross2 / cross1;
        float t2 = cross3 / cross1;

        return (t1 > 0 && t1 < 1) && (t2 > 0 && t2 < 1);
    }
    // ���� ��� �Լ�
    private float Cross(Vector2 left, Vector2 right)
    {
        return left.x * right.y - left.y * right.x;
    }

    /*public Dictionary<Vector2, List<Vertex>> Triangulation(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        // �Ϲ� �ﰢȭ
        // �ﰢȭ�� �����߽�, ��� ���� üũ
        // �ﰢȭ�� �Ϸ�� vertex�� ����Ʈ���� ����
        //���� vertex�� �ﰢȭ
        //����Ʈ�� 0�� �� ����
        //���� Cell�� �̿� Cell�� ����� ��γ� �ﰢȭ ����ȭ
        //����

        //�ڷᱸ��
        //Cell�� ��ųʸ�<Vecter2, Cell>�߽���, ��

        //�Ϲ� �ﰢȭ
        return GenerationNormalTriangulation(VertexDatas, ContourLines, mapData, distance);
        //��γ� �ﰢȭ
        //GenerationDelaunayTriangulation();
    }
    public Dictionary<Vector2, List<Vertex>> GenerationNormalTriangulation(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        //��� ��ȸ�� ����� �� 3���� ã�´�
        //�� 3���� �ﰢȭ ���ǿ� �´��� Ȯ���Ѵ�
        //������ ���Ե� Cell�� ���������� Ȯ���Ѵ�
        //Cell�� �����Ѵ�
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();

        foreach (var vt in VertexDatas)
        {
            //��� ������ ���� �Ÿ��� �����ϰ� ������� ���� �� �� �� 3���� ������ vertex�� ����� List�� ��ȯ
            var cellVertices = VertexDatas.Where(V => V != vt)
                                  .Select(V => new { vertex = V, Distance = Distance(vt, V) })
                                  .OrderBy(V => V.Distance)
                                  .Select(V => V.vertex)
                                  .ToList();

            //�ݺ�, ���� ã�� ������
            for (int i = 0; i < cellVertices.Count; i++)
            {
                for (int k = 0; k < cellVertices.Count; k++)
                {

                }
            }

                //.Add(vt);
            Vector2 centroidVec = Centroid(cellVertices);

            //new Vertex(new Vector2(8.5f, 2.0f)), new Vertex(new Vector2(6.5f, 2.0f)), new Vertex(new Vector2(9.5f, 1.0f))

            if (!CheakTriangleCentroid(mapData, distance, centroidVec))
                continue;
            if(!CheakCross(cellVertices, ContourLines))
                continue;
            nearestPoints.Add(centroidVec, cellVertices);
        }
        return nearestPoints;
    }
    //�� �� ������ �Ÿ� ����
    public float Distance(Vertex left, Vertex right)
    {
        return (float)Mathf.Sqrt((left.vertex.x - right.vertex.x) * (left.vertex.x - right.vertex.x) + (left.vertex.y - right.vertex.y) * (left.vertex.y - right.vertex.y));
    }

    public void GenerationDelaunayTriangulation()
    {

    }

    //�� ���
    public void CheakAngle()
    {
        //ù ���� ������ ���� ���
        //ù ���� ��� ����(��� �̿��� �̿� Ž���� �� �̿����� ����)
    }
    //���� �߽����� �ܺ���ġ�� �ִ��� Ȯ��
    public void CheakCircleCenter(Vertex vertex)
    {

    }

    //�ﰢ���� �����߽��� �ܺ���ġ�� �ִ��� Ȯ��(�����߽ɰ� �������̿��� �ܺΰ����� ���� ��� Ž���Ұ�)
    public bool CheakTriangleCentroid(bool[,] mapData, float distance, Vector2 Centroid)
    {
        //TODO
        Vector2 temp = Centroid / distance;

        return mapData[(int)temp.y, (int)temp.x];
    }

    //����� �����Ǵ��� üũ�ϴ� �Լ�(���� Ž���Ұ�)
    //�ٸ� ���� �����Ǵ��� üũ�ϴ� �Լ�(���� Ž���Ұ�)
    public bool CheakCross(List<Vertex> triangleVertices, List<List<Vertex>> contourLines)
    {
        if (triangleVertices == null || triangleVertices.Count() != 3)
            return false;

        foreach (List<Vertex> contourLine in contourLines)
        {
            for (int i = 0; i < contourLine.Count; i++)
            {
                foreach(var conVertex in contourLine[i].connectionVertex)
                {
                    if (IsIntersect(triangleVertices[0], triangleVertices[1], contourLine[i], conVertex.Key))
                        return false;
                    if (IsIntersect(triangleVertices[1], triangleVertices[2], contourLine[i], conVertex.Key))
                        return false;
                    if (IsIntersect(triangleVertices[2], triangleVertices[0], contourLine[i], conVertex.Key))
                        return false;
                }
            }
        }
        return true;
    }
    // ���� ���� Ȯ�� �Լ�
    public bool IsIntersect(Vertex originLeft, Vertex originRight, Vertex otherLeft, Vertex otherRight)
    {
        Vector2 d1 = originRight.vertex - originLeft.vertex;
        Vector2 d2 = otherRight.vertex - otherLeft.vertex;
        Vector2 d3 = originLeft.vertex - otherLeft.vertex;

        float cross1 = Cross(d1, d2);
        float cross2 = Cross(d3, d2);
        float cross3 = Cross(d3, d1);

        if (cross1 == 0) return false; // �����ϰų� �������� ����

        float t1 = cross2 / cross1;
        float t2 = cross3 / cross1;

        return (t1 >= 0 && t1 <= 1) && (t2 >= 0 && t2 <= 1);
    }
    // ���� ��� �Լ�
    private float Cross(Vector2 left, Vector2 right)
    {
        return left.x * right.y - left.y * right.x;
    }

    //���� �����߽��� ���ϴ� �Լ�
    public Vector2 Centroid(List<Vertex> triangleVertices)
    {
        if (triangleVertices == null || triangleVertices.Count() != 3)
            return Vector2.zero;

        Vector2 sum = Vector2.zero;
        foreach (Vertex vertex in triangleVertices)
        {
            sum += vertex.vertex;
        }
        Vector2 centroid = new Vector2(sum.x * (1.0f / 3.0f), sum.y * (1.0f / 3.0f));

        return centroid;
    }*/
}
