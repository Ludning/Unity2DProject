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
    //들로네 삼각화
    public void GenerationDelaunayTriangulation()
    {
        
    }
    //외접원을 구하는 코드

    //일반 삼각화
    public Dictionary<Vector2, List<Vertex>> GenerationNormalTriangulation(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();
        foreach (var vt in VertexDatas)
        {
            List<Vertex> triangle = null;
            //모든 정점에 대해 거리를 측정하고 순서대로 정렬 한 후 앞 3개를 꺼내서 vertex만 남기고 List로 반환
            var cellVertices = VertexDatas.Where(V => V != vt)
                                  .Select(V => new { vertex = V, Distance = Distance(vt, V) })
                                  .OrderBy(V => V.Distance)
                                  .Select(V => V.vertex)
                                  .ToList();

            bool searchEnd = true;
            Vector2 centroidVec = Vector2.zero;
            //조건에 만족하는 삼각형을 찾을 때 까지
            for (var i = 0; i < cellVertices.Count; i++)
            {
                for (var k = 0; k < cellVertices.Count; k++)
                {
                    if (cellVertices[i] == cellVertices[k])
                        continue;

                    triangle = new List<Vertex>() { vt, cellVertices[i], cellVertices[k] };
                    var centroid = Centroid(triangle);
                    //만족해야 할 조건은????
                    //셀의 무게중심이 외부를 지나지 않는다
                    if (!CheakTriangleCentroid(mapData, distance, centroid))
                        continue;
                    //다른 선과 교차되지 않는다(모든 연결 계산)
                    if (!CheakCross(VertexDatas, vt, cellVertices[i], cellVertices[k]))
                        continue;

                    //닫힌 벽, 닫힌 점은 삭제
                    //Unlink(ContourLines, vt, cellVertices[i], cellVertices[k]);

                    vt.Link(cellVertices[i]);
                    vt.Link(cellVertices[k]);
                    cellVertices[i].Link(cellVertices[k]);
                    //만족하면 바로 나가게 된다
                    
                    nearestPoints.Add(centroid, triangle);
                    return nearestPoints;
                }
            }
        }
        return null;
    }

    public void GenerationNormalTriangulationMK2(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        //제일 긴 등고선을 가져온다
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();
        //나머지 등고선
        var otherVerties = ContourLines.Where(x => x != longestList).ToList().SelectMany(x => x).ToList();

        //otherVerties 연결안된 점은 따로 처리
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
        //제일 긴 등고선을 가져온다
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();
        //나머지 등고선
        var otherVerties = ContourLines.Where(x => x != longestList).ToList().SelectMany(x => x).ToList();

        Vertex startVt = VertexDatas.First();
        Vertex endVt = startVt.connectionVertex.First().Key;

        //otherVerties 연결안된 점은 따로 처리
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
        //가까운 선끼리 삼각형을 만들어보자
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
        //제일 긴 등고선을 가져온다
        var longestList = ContourLines.OrderByDescending(list => list.Count).FirstOrDefault();

        bool research = false;
        bool searchEnd = false;
        while (true)
        {
            research = false;
            searchEnd = false;
            //주위를 돌면서 삼각화를 한다
            foreach (var vt in longestList)
            {
                var vecList = vt.connectionVertex.ToList();
                if(longestList.Count <= 2)
                {
                    searchEnd = true;
                    break;
                }
                //수직 벡터로부터 좌우로 90도를 넘어가지 않으면
                if (DotProduct(vecList[0].Value, vecList[1].Value))
                {
                    //삼각형 리스트를 만들고
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
        //제일 긴 등고선을 가져온다
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
            //수직 벡터로부터 좌우로 90도를 넘어가면
            if (!DotProduct(vecList[0].Value, vecList[1].Value))
            {
                count++;
                continue; 
            }

            //삼각형 리스트를 만들고
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
        //제일 긴 등고선을 가져온다
        var ContourList = OrderByContourLineLength(ContourLines);

        int count = 0;
        //등고선 연결
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
    //변과 점이 삼각형이 될수 있는 조건
    /*public bool IsCanTriangle(Vertex vt1, Vertex vt2, Vertex other)
    {
        vt1.
    }*/

    //내적 검사
    public bool DotProduct(Vector2 v1, Vector2 v2)
    {
        float dotProduct = v1.x * v2.x + v1.y * v2.y;
        if (dotProduct < 0)
        {
            //수직 벡터로부터 좌우로 90도를 넘어감
            return false;
        }
        return true;
    }

    //삼각재귀
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

    #region 들로네 수학 함수
    // 삼각형의 외접원 중심을 계산
    public Vector2 CalculateCircumcenter(Vertex vt1, Vertex vt2, Vertex vt3)
    {
        float d = 2 * (vt1.vertex.x * (vt2.vertex.y - vt3.vertex.y) + vt2.vertex.x * (vt3.vertex.y - vt1.vertex.y) + vt3.vertex.x * (vt1.vertex.y - vt2.vertex.y));
        float ux = ((vt1.vertex.x * vt1.vertex.x + vt1.vertex.y * vt1.vertex.y) * (vt2.vertex.y - vt3.vertex.y) + (vt2.vertex.x * vt2.vertex.x + vt2.vertex.y * vt2.vertex.y) * (vt3.vertex.y - vt1.vertex.y) + (vt3.vertex.x * vt3.vertex.x + vt3.vertex.y * vt3.vertex.y) * (vt1.vertex.y - vt2.vertex.y)) / d;
        float uy = ((vt1.vertex.x * vt1.vertex.x + vt1.vertex.y * vt1.vertex.y) * (vt3.vertex.x - vt2.vertex.x) + (vt2.vertex.x * vt2.vertex.x + vt2.vertex.y * vt2.vertex.y) * (vt1.vertex.x - vt3.vertex.x) + (vt3.vertex.x * vt3.vertex.x + vt3.vertex.y * vt3.vertex.y) * (vt2.vertex.x - vt1.vertex.x)) / d;
        return new Vector2(ux, uy);
    }

    // 외접원 반지름 계산
    public double CalculateCircumradius(Vector2 circumcenter, Vertex vt)
    {
        return Math.Sqrt((circumcenter.x - vt.vertex.x) * (circumcenter.x - vt.vertex.x) + (circumcenter.y - vt.vertex.y) * (circumcenter.y - vt.vertex.y));
    }

    // 주어진 점이 삼각형의 외접원 내에 있는지 검사
    public bool IsPointInsideCircumcircle(Vertex vt1, Vertex vt2, Vertex vt3, Vector2 point)
    {
        Vector2 circumcenter = CalculateCircumcenter(vt1, vt2, vt3);
        double circumradius = CalculateCircumradius(circumcenter, vt1);
        double distance = Math.Sqrt((point.x - circumcenter.x) * (point.x - circumcenter.x) + (point.y - circumcenter.y) * (point.y - circumcenter.y));
        return distance < circumradius;
    }
    #endregion

    //두 점 사이의 거리 측정
    public float Distance(Vertex left, Vertex right)
    {
        return (float)Mathf.Sqrt((left.vertex.x - right.vertex.x) * (left.vertex.x - right.vertex.x) + (left.vertex.y - right.vertex.y) * (left.vertex.y - right.vertex.y));
    }

    //셀의 무게중심을 구하는 함수
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

    //닫힌 벽, 닫힌 점은 삭제
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

    //삼각형의 무게중심이 외부위치에 있는지 확인(무게중심과 정점사이에만 외부공간이 있을 경우 탐지불가)
    public bool CheakTriangleCentroid(bool[,] mapData, float distance, Vector2 Centroid)
    {
        Vector2 temp = Centroid / distance;
        return mapData[(int)temp.y, (int)temp.x];
    }
    //다른 선과 교차되는지 체크하는 함수(역뿔 탐지불가)
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
    // 교차 여부 확인 함수
    public bool IsIntersect(Vertex originLeft, Vertex originRight, Vertex otherLeft, Vertex otherRight)
    {
        Vector2 d1 = originRight.vertex - originLeft.vertex;
        Vector2 d2 = otherRight.vertex - otherLeft.vertex;
        Vector2 d3 = originLeft.vertex - otherLeft.vertex;

        float cross1 = Cross(d1, d2);
        float cross2 = Cross(d3, d2);
        float cross3 = Cross(d3, d1);

        if (cross1 == 0) return false; // 평행하거나 일직선상에 있음

        float t1 = cross2 / cross1;
        float t2 = cross3 / cross1;

        return (t1 > 0 && t1 < 1) && (t2 > 0 && t2 < 1);
    }
    // 외적 계산 함수
    private float Cross(Vector2 left, Vector2 right)
    {
        return left.x * right.y - left.y * right.x;
    }

    /*public Dictionary<Vector2, List<Vertex>> Triangulation(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        // 일반 삼각화
        // 삼각화중 무세중심, 등교선 교차 체크
        // 삼각화가 완료된 vertex는 리스트에서 삭제
        //남은 vertex를 삼각화
        //리스트가 0일 때 종료
        //얻은 Cell을 이웃 Cell과 계산해 들로네 삼각화 최적화
        //종료

        //자료구조
        //Cell은 딕셔너리<Vecter2, Cell>중심점, 셀

        //일반 삼각화
        return GenerationNormalTriangulation(VertexDatas, ContourLines, mapData, distance);
        //들로네 삼각화
        //GenerationDelaunayTriangulation();
    }
    public Dictionary<Vector2, List<Vertex>> GenerationNormalTriangulation(HashSet<Vertex> VertexDatas, List<List<Vertex>> ContourLines, bool[,] mapData, float distance)
    {
        //모두 순회후 가까운 점 3개를 찾는다
        //점 3개가 삼각화 조건에 맞는지 확인한다
        //정점이 포함된 Cell을 지나가는지 확인한다
        //Cell을 생성한다
        Dictionary<Vector2, List<Vertex>> nearestPoints = new Dictionary<Vector2, List<Vertex>>();

        foreach (var vt in VertexDatas)
        {
            //모든 정점에 대해 거리를 측정하고 순서대로 정렬 한 후 앞 3개를 꺼내서 vertex만 남기고 List로 반환
            var cellVertices = VertexDatas.Where(V => V != vt)
                                  .Select(V => new { vertex = V, Distance = Distance(vt, V) })
                                  .OrderBy(V => V.Distance)
                                  .Select(V => V.vertex)
                                  .ToList();

            //반복, 선을 찾을 때까지
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
    //두 점 사이의 거리 측정
    public float Distance(Vertex left, Vertex right)
    {
        return (float)Mathf.Sqrt((left.vertex.x - right.vertex.x) * (left.vertex.x - right.vertex.x) + (left.vertex.y - right.vertex.y) * (left.vertex.y - right.vertex.y));
    }

    public void GenerationDelaunayTriangulation()
    {

    }

    //각 계산
    public void CheakAngle()
    {
        //첫 점과 두점의 각도 계산
        //첫 점의 허용 각도(등고선 이용한 이웃 탐색후 두 이웃과의 각도)
    }
    //원의 중심점이 외부위치에 있는지 확인
    public void CheakCircleCenter(Vertex vertex)
    {

    }

    //삼각형의 무게중심이 외부위치에 있는지 확인(무게중심과 정점사이에만 외부공간이 있을 경우 탐지불가)
    public bool CheakTriangleCentroid(bool[,] mapData, float distance, Vector2 Centroid)
    {
        //TODO
        Vector2 temp = Centroid / distance;

        return mapData[(int)temp.y, (int)temp.x];
    }

    //등고선과 교차되는지 체크하는 함수(역뿔 탐지불가)
    //다른 선과 교차되는지 체크하는 함수(역뿔 탐지불가)
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
    // 교차 여부 확인 함수
    public bool IsIntersect(Vertex originLeft, Vertex originRight, Vertex otherLeft, Vertex otherRight)
    {
        Vector2 d1 = originRight.vertex - originLeft.vertex;
        Vector2 d2 = otherRight.vertex - otherLeft.vertex;
        Vector2 d3 = originLeft.vertex - otherLeft.vertex;

        float cross1 = Cross(d1, d2);
        float cross2 = Cross(d3, d2);
        float cross3 = Cross(d3, d1);

        if (cross1 == 0) return false; // 평행하거나 일직선상에 있음

        float t1 = cross2 / cross1;
        float t2 = cross3 / cross1;

        return (t1 >= 0 && t1 <= 1) && (t2 >= 0 && t2 <= 1);
    }
    // 외적 계산 함수
    private float Cross(Vector2 left, Vector2 right)
    {
        return left.x * right.y - left.y * right.x;
    }

    //셀의 무게중심을 구하는 함수
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
