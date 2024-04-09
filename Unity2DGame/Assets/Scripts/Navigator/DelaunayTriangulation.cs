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
