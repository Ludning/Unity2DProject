using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.UI.Image;


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
    public Dictionary<Vector2, List<Vertex>> GenerationNormalTriangulationMK4(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        //����� ������ �ﰢ���� ������
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();
        foreach (var currentVertex in VertexDatas)
        {
            int count = 0;
            while(true)
            {
                var vtListOrderByDis = VertexDatas.Where(x => x != currentVertex)
                                                .Select(x => new { vertex = x, distance = Distance(currentVertex, x) })
                                                .OrderBy(x => x.distance)
                                                .Select(x => x.vertex)
                                                .ToList();


                List<Vertex> tri = new List<Vertex>() { currentVertex, vtListOrderByDis[count], vtListOrderByDis[count + 1] };
                if(!nearestPoints.ContainsKey(Centroid(tri)))
                {
                    nearestPoints.Add(Centroid(tri), tri);
                    currentVertex.UnlinkSelf();
                    break;
                }
                count++;
                if (count + 1 >= vtListOrderByDis.Count)
                    break;
            }
            
        }
        return nearestPoints;
    }

    public Dictionary<Vector2, List<Vertex>> GenerationContourLines(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();
        //���� �� ����� �����´�
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();

        bool research = false;
        bool searchEnd = false;
        while (true)
        {
            research = false;
            searchEnd = false;
            //������ ���鼭 �ﰢȭ�� �Ѵ�
            foreach (var vt in longestList)
            {
                var vecList = vt.connectionVertex.ToList();
                if(longestList.Count <= 2)
                {
                    searchEnd = true;
                    break;
                }
                //���� ���ͷκ��� �¿�� 90���� �Ѿ�� ������
                if (DotProduct(vecList[0].Value, vecList[1].Value))
                {
                    //�ﰢ�� ����Ʈ�� �����
                    List<Vertex> vTri = new List<Vertex>();
                    vTri.Add(vt);
                    vTri.AddRange(vt.connectionVertex.Keys.ToList());
                    nearestPoints.Add(Centroid(vTri), vTri);
                    vt.Unlink(vecList[0].Key);
                    vt.Unlink(vecList[1].Key);
                    vecList[0].Key.Link(vecList[1].Key);
                    longestList.Remove(vt);
                    research = true;
                }
                if (research)
                    break;
            }
            if (searchEnd)
                break;
        }

        
        return nearestPoints;

    }
    public Dictionary<Vector2, List<Vertex>> GenerationContourLinesMK2(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();
        //���� �� ����� �����´�
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();

        int count = 0;
        while (count < longestList.Count)
        {

            var vecList = longestList[count].connectionVertex.ToList();
            if (longestList.Count <= 2)
            {
                count++;
                continue; 
            }
            //���� ���ͷκ��� �¿�� 90���� �Ѿ��
            if (!DotProduct(vecList[0].Value, vecList[1].Value))
            {
                count++;
                continue; 
            }

            //�ﰢ�� ����Ʈ�� �����
            List<Vertex> vTri = new List<Vertex>();
            vTri.Add(longestList[count]);
            vTri.AddRange(longestList[count].connectionVertex.Keys.ToList());

            nearestPoints.Add(Centroid(vTri), vTri);

            longestList[count].UnlinkSelf();
            longestList.Remove(longestList[count]);
        }
        return nearestPoints;
    }

    public Dictionary<Vector2, List<Vertex>> GenerationContourLinesMK3(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();
        //���� �� ����� �����´�
        var ContourList = OrderByContourLineLength(ContourLines);

        int count = 0;
        //��� ����
        var longestList = ContourList.Last();
        ContourList.Remove(longestList);

        List<Vertex> newContourLine = new List<Vertex>();
        foreach (var ContourLine in ContourList)
        {
            (Vertex, Vertex, float)? minLine = null;
            foreach(var otherVertex in ContourLine)
            {
                foreach(var vertex in longestList)
                {
                    if (minLine == null)
                    { 
                        minLine = (vertex, otherVertex, Distance(vertex, otherVertex));
                        continue;
                    }
                    float dis = Distance(vertex, otherVertex);
                    if (minLine.Value.Item3 > dis)
                    {
                        minLine = (vertex, otherVertex, Distance(vertex, otherVertex));
                        continue;
                    }
                }
            }
            List<Vertex> vTri1 = new List<Vertex>() { minLine.Value.Item1, minLine.Value.Item2, minLine.Value.Item1.connectionVertex.First().Key };
            List<Vertex> vTri2 = new List<Vertex>() { minLine.Value.Item2.connectionVertex.OrderBy(x => Distance(minLine.Value.Item1.connectionVertex.First().Key, x.Key)).ToList().First().Key, minLine.Value.Item2, minLine.Value.Item1.connectionVertex.First().Key };
            nearestPoints.Add(Centroid(vTri1), vTri1);
            nearestPoints.Add(Centroid(vTri2), vTri2);
        }


        return nearestPoints;
    }
    public List<List<Vertex>> OrderByContourLineLength(List<List<Vertex>> ContourLines)
    {
        Dictionary<List<Vertex>, float> Contours = new Dictionary<List<Vertex>, float>();
        foreach (var ContourLine in ContourLines)
        {
            Contours.Add(ContourLine, ContourLineLength(ContourLine));
        }
        return Contours.OrderBy(x => x.Value).Select(x => x.Key).ToList();
    }
    public float ContourLineLength(List<Vertex> ContourLine)
    {
        Vertex first = ContourLine.First();
        return VerticsLength(first, first, null);
    }
    public float VerticsLength(Vertex vt, Vertex origin, Vertex prev)
    {
        float dis = 0f;

        foreach (var conVt in vt.connectionVertex)
        {
            if(prev != null && conVt.Key == prev)
                continue;
            if (origin == conVt.Key)
                break;
            dis += Distance(vt, conVt.Key) + VerticsLength(conVt.Key, origin, vt);
        }
        return dis;
    }
    //���� ���� �ﰢ���� �ɼ� �ִ� ����
    /*public bool IsCanTriangle(Vertex vt1, Vertex vt2, Vertex other)
    {
        vt1.
    }*/

    //���� �˻�
    public bool DotProduct(Vector2 v1, Vector2 v2)
    {
        float dotProduct = v1.x * v2.x + v1.y * v2.y;
        if (dotProduct < 0)
        {
            //���� ���ͷκ��� �¿�� 90���� �Ѿ
            return false;
        }
        return true;
    }

    //�ﰢ���
    /*public void GenTri(HashSet<Vertex> VertexDatas, Vertex currentVertex)
    {
        List<Vertex> conVtList = currentVertex.connectionVertex.Keys.ToList();

        var vtListOrderByDis = VertexDatas.Where(x => x != currentVertex)
                                                .Select(x => new { vertex = x, distance = Distance(currentVertex, x) })
                                                .OrderBy(x => x.distance)
                                                .Select(x => x.vertex)
                                                .ToList();

        foreach (var conVt in conVtList)
        {
            
        }
        VertexDatas.Remove();
    }*/

    #region ��γ� ���� �Լ�
    // �ﰢ���� ������ �߽��� ���
    public Vector2 CalculateCircumcenter(Vertex vt1, Vertex vt2, Vertex vt3)
    {
        float d = 2 * (vt1.vertex.x * (vt2.vertex.y - vt3.vertex.y) + vt2.vertex.x * (vt3.vertex.y - vt1.vertex.y) + vt3.vertex.x * (vt1.vertex.y - vt2.vertex.y));
        float ux = ((vt1.vertex.x * vt1.vertex.x + vt1.vertex.y * vt1.vertex.y) * (vt2.vertex.y - vt3.vertex.y) + (vt2.vertex.x * vt2.vertex.x + vt2.vertex.y * vt2.vertex.y) * (vt3.vertex.y - vt1.vertex.y) + (vt3.vertex.x * vt3.vertex.x + vt3.vertex.y * vt3.vertex.y) * (vt1.vertex.y - vt2.vertex.y)) / d;
        float uy = ((vt1.vertex.x * vt1.vertex.x + vt1.vertex.y * vt1.vertex.y) * (vt3.vertex.x - vt2.vertex.x) + (vt2.vertex.x * vt2.vertex.x + vt2.vertex.y * vt2.vertex.y) * (vt1.vertex.x - vt3.vertex.x) + (vt3.vertex.x * vt3.vertex.x + vt3.vertex.y * vt3.vertex.y) * (vt2.vertex.x - vt1.vertex.x)) / d;
        return new Vector2(ux, uy);
    }

    // ������ ������ ���
    public double CalculateCircumradius(Vector2 circumcenter, Vertex vt)
    {
        return Math.Sqrt((circumcenter.x - vt.vertex.x) * (circumcenter.x - vt.vertex.x) + (circumcenter.y - vt.vertex.y) * (circumcenter.y - vt.vertex.y));
    }

    // �־��� ���� �ﰢ���� ������ ���� �ִ��� �˻�
    public bool IsPointInsideCircumcircle(Vertex vt1, Vertex vt2, Vertex vt3, Vector2 point)
    {
        Vector2 circumcenter = CalculateCircumcenter(vt1, vt2, vt3);
        double circumradius = CalculateCircumradius(circumcenter, vt1);
        double distance = Math.Sqrt((point.x - circumcenter.x) * (point.x - circumcenter.x) + (point.y - circumcenter.y) * (point.y - circumcenter.y));
        return distance < circumradius;
    }
    #endregion

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
