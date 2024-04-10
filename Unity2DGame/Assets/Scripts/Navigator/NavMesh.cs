using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NavMesh : MonoBehaviour
{
    List<NavCell> cells = new List<NavCell>();

    MarchingSquares marchingSquares = new MarchingSquares();
    DelaunayTriangulation delaunayTriangulation = new DelaunayTriangulation();

    [SerializeField]
    GameObject textUIPrefab;

    private void Start()
    {
        marchingSquares.Init();

        //delaunayTriangulation.GenerationNormalTriangulationMK3(marchingSquares.VertexDatas, marchingSquares.ContourLines, marchingSquares.mapData, marchingSquares.distance);

        //Dictionary<Vector2, List<Vertex>> triangulationData = delaunayTriangulation.GenerationNormalTriangulationMK4(marchingSquares.VertexDatas, marchingSquares.ContourLines, marchingSquares.mapData, marchingSquares.distance);

        //Dictionary<Vector2, List<Vertex>> triangulationData = delaunayTriangulation.GenerationContourLines(marchingSquares.VertexDatas, marchingSquares.ContourLines, marchingSquares.mapData, marchingSquares.distance);
        Dictionary<Vector2, List<Vertex>> triangulationData = delaunayTriangulation.GenerationContourLinesMK3(marchingSquares.VertexDatas, marchingSquares.ContourLines, marchingSquares.mapData, marchingSquares.distance);

        //Dictionary<Vector2, List<Vertex>> triangulationData = delaunayTriangulation.Triangulation(marchingSquares.VertexDatas, marchingSquares.ContourLines, marchingSquares.mapData, marchingSquares.distance);
        /*
        Dictionary<Vector2, List<Vertex>> triangulationData = new Dictionary<Vector2, List<Vertex>>();
        triangulationData.Add(new Vector2(0, 0), new List<Vertex>() { new Vertex(new Vector2(8.5f, 2f)), new Vertex(new Vector2(6.5f, 2f)), new Vertex(new Vector2(9.5f, 1f)) });
        triangulationData.Add(new Vector2(1, 0), new List<Vertex>() { new Vertex(new Vector2(14f, 1.5f)), new Vertex(new Vector2(9.5f, 1f)), new Vertex(new Vector2(13.5f, 1f)) });
        */


        /*
            new Vertex(new Vector2(8.5f, 2f)), new Vertex(new Vector2(6.5f, 2f)), new Vertex(new Vector2(9.5f, 1f)
            new Vertex(new Vector2(14f, 1.5f)), new Vertex(new Vector2(9.5f, 1f)), new Vertex(new Vector2(13.5f, 1f)
            new Vertex(new Vector2(13.5f, 1f)), new Vertex(new Vector2(14f, 5.5f)), new Vertex(new Vector2(14f, 1.5f)
            new Vertex(new Vector2(5.5f, 3f)), new Vertex(new Vector2(8.5f, 2f)), new Vertex(new Vector2(6.5f, 2f)
            new Vertex(new Vector2(9.5f, 1f)), new Vertex(new Vector2(6.5f, 2f)), new Vertex(new Vector2(8.5f, 2f)
            new Vertex(new Vector2(1.5f, 3f)), new Vertex(new Vector2(2f, 4.5f)), new Vertex(new Vector2(1f, 3.5f)
            new Vertex(new Vector2(1f, 3.5f)), new Vertex(new Vector2(2.5f, 3f)), new Vertex(new Vector2(1.5f, 3f)
            new Vertex(new Vector2(1.5f, 3f)), new Vertex(new Vector2(3.5f, 4f)), new Vertex(new Vector2(2.5f, 3f)
            new Vertex(new Vector2(5.5f, 3f)), new Vertex(new Vector2(3.5f, 4f)), new Vertex(new Vector2(4.5f, 3f)
            new Vertex(new Vector2(4.5f, 3f)), new Vertex(new Vector2(6.5f, 2f)), new Vertex(new Vector2(5.5f, 3f)
            new Vertex(new Vector2(2f, 5.5f)), new Vertex(new Vector2(1f, 3.5f)), new Vertex(new Vector2(2f, 4.5f)
            new Vertex(new Vector2(2.5f, 3f)), new Vertex(new Vector2(4.5f, 3f)), new Vertex(new Vector2(3.5f, 4f)
            new Vertex(new Vector2(2f, 4.5f)), new Vertex(new Vector2(1f, 6.5f)), new Vertex(new Vector2(2f, 5.5f)
            new Vertex(new Vector2(12f, 7.5f)), new Vertex(new Vector2(13f, 8.5f)), new Vertex(new Vector2(14f, 5.5f)
            new Vertex(new Vector2(1f, 7.5f)), new Vertex(new Vector2(2f, 5.5f)), new Vertex(new Vector2(1f, 6.5f)
            new Vertex(new Vector2(6f, 6.5f)), new Vertex(new Vector2(7.5f, 6f)), new Vertex(new Vector2(6.5f, 6f)
            new Vertex(new Vector2(6.5f, 6f)), new Vertex(new Vector2(7.5f, 6f)), new Vertex(new Vector2(6f, 6.5f)
            new Vertex(new Vector2(8f, 6.5f)), new Vertex(new Vector2(6.5f, 6f)), new Vertex(new Vector2(7.5f, 6f)
            new Vertex(new Vector2(7.5f, 6f)), new Vertex(new Vector2(6.5f, 6f)), new Vertex(new Vector2(8f, 6.5f)
            new Vertex(new Vector2(1f, 6.5f)), new Vertex(new Vector2(2f, 8.5f)), new Vertex(new Vector2(1f, 7.5f)
            new Vertex(new Vector2(13f, 8.5f)), new Vertex(new Vector2(11.5f, 10f)), new Vertex(new Vector2(12f, 7.5f)
            new Vertex(new Vector2(1f, 7.5f)), new Vertex(new Vector2(1f, 6.5f)), new Vertex(new Vector2(2f, 8.5f)
            new Vertex(new Vector2(6.5f, 9f)), new Vertex(new Vector2(7.5f, 9f)), new Vertex(new Vector2(6f, 8.5f)
            new Vertex(new Vector2(7.5f, 9f)), new Vertex(new Vector2(6.5f, 9f)), new Vertex(new Vector2(8f, 8.5f)
            new Vertex(new Vector2(12f, 7.5f)), new Vertex(new Vector2(11.5f, 10f)), new Vertex(new Vector2(13f, 8.5f)
            new Vertex(new Vector2(6f, 8.5f)), new Vertex(new Vector2(7.5f, 9f)), new Vertex(new Vector2(6.5f, 9f)
            new Vertex(new Vector2(8f, 8.5f)), new Vertex(new Vector2(6.5f, 9f)), new Vertex(new Vector2(7.5f, 9f)
            new Vertex(new Vector2(10f, 10.5f)), new Vertex(new Vector2(11.5f, 10f)), new Vertex(new Vector2(10.5f, 10f)
            new Vertex(new Vector2(10.5f, 10f)), new Vertex(new Vector2(10f, 11.5f)), new Vertex(new Vector2(10f, 10.5f)
            new Vertex(new Vector2(10.5f, 10f)), new Vertex(new Vector2(10f, 10.5f)), new Vertex(new Vector2(11.5f, 10f)
            new Vertex(new Vector2(1f, 12.5f)), new Vertex(new Vector2(1f, 13.5f)), new Vertex(new Vector2(2f, 11.5f)
            new Vertex(new Vector2(10f, 10.5f)), new Vertex(new Vector2(10.5f, 10f)), new Vertex(new Vector2(10f, 11.5f)
            new Vertex(new Vector2(1f, 13.5f)), new Vertex(new Vector2(2f, 11.5f)), new Vertex(new Vector2(1f, 12.5f)
            new Vertex(new Vector2(1.5f, 14f)), new Vertex(new Vector2(1f, 12.5f)), new Vertex(new Vector2(1f, 13.5f)
            new Vertex(new Vector2(3.5f, 14f)), new Vertex(new Vector2(2f, 11.5f)), new Vertex(new Vector2(4.5f, 13f)
            new Vertex(new Vector2(10f, 11.5f)), new Vertex(new Vector2(10f, 10.5f)), new Vertex(new Vector2(8.5f, 13f)
            new Vertex(new Vector2(1f, 13.5f)), new Vertex(new Vector2(1f, 12.5f)), new Vertex(new Vector2(1.5f, 14f)
            new Vertex(new Vector2(4.5f, 13f)), new Vertex(new Vector2(1.5f, 14f)), new Vertex(new Vector2(3.5f, 14f)
         */


        ReadCellData(triangulationData);


        //StartCoroutine(DrawMarchingSquares(marchingSquares.VertexDatas));
        //StartCoroutine(marchingSquares.DrawContourLines());
        /*AddCell(new Vector2(0, 0), new Vector2(5, 0), new Vector2(0, 5));
        AddCell(new Vector2(0, 0), new Vector2(5, 0), new Vector2(0, -5));
        AddCell(new Vector2(0, 0), new Vector2(-5, 0), new Vector2(0, -5));*/

        //삼각형 계산 함수 실행
        //Triangulation();

        StartCoroutine(DrawNavCells());
    }
    /*public void DrawTextUI(HashSet<Vertex> VertexDatas)
    {
        foreach (var cell in VertexDatas)
        {

        }

        GameObject go = Instantiate(textUIPrefab);
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        go.transform.position = 
        tmp.text = ""
    }*/
    public IEnumerator DrawMarchingSquares(HashSet<Vertex> VertexDatas)
    {
        while (true)
        {
            //연결선
            foreach (var data in VertexDatas)
            {
                foreach (var con in data.connectionVertex.Keys)
                {
                    Debug.DrawLine(data.vertex, con.vertex);
                }
            }
            //수직선
            foreach (var data in VertexDatas)
            {
                foreach (var con in data.connectionVertex)
                {
                    Debug.DrawLine(data.vertex, data.vertex + con.Value, Color.blue);
                }
            }
            //정점
            foreach (var data in VertexDatas)
            {
                Debug.DrawLine(data.vertex, data.vertex + Vector2.one * 0.1f, Color.red);
            }
            Debug.Log(VertexDatas.Count);
            yield return null;
        }
    }
    /*//삼각화
    public void Triangulation()
    {
        //삼각화
        //삼각화를 해서 Cell이 되었으며 Cell에 포함된 이웃정점외에 다른 정점이 없으면 제거


        Vertex vertex = marchingSquares.VertexDatas.First();
        //vertex.connectionVertex
    }*/

    public void ReadCellData(Dictionary<Vector2, List<Vertex>> triangulationData)
    {
        foreach(var data in triangulationData)
        {
            AddCell(data.Value, data.Key);
        }
    }
    //셀 추가 함수
    public void AddCell(List<Vertex> vertices, Vector2 centroid)
    {
        if (vertices == null || vertices.Count != 3)
            return;
        //if (!IsIncludedCell(vertices))
            cells.Add(new NavCell(vertices[0].vertex, vertices[1].vertex, vertices[2].vertex, centroid));
    }
    //일단 세 점을 가지고 있는 cell이 있는지 확인한다
    public bool IsIncludedCell(List<Vertex> vertices)
    {
        //일단 세 점을 가지고 있는 cell이 있는지 확인한다
        //있으면 true
        //아니면 false
        Vector2[] vecs = new Vector2[3] { vertices[0].vertex, vertices[1].vertex, vertices[2].vertex };
        foreach (var cell in cells)
        {
            if (cell.datas.All(x => vecs.Contains(x.vertex)))
            {
                return true;
            }
        }
        return false;
    }
    //셀 그리기 함수
    IEnumerator DrawNavCells()
    {
        while(true)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for (int c = 0; c < cells[i].datas.Count(); c++)
                {
                    Debug.DrawLine(cells[i].datas[c].vertex, cells[i].NextVertex(c), Color.red);
                }
            }
            yield return null;
        }
    }
    
    //길찾기 함수
    public List<Vector2> PathFinding(Vector2 start, Vector2 end)
    {
        NavCell temp = null;
        List<Vector2> route = new List<Vector2>();

        foreach (var cell in cells)
        {
            if (cell.IsInArea(start))
            {
                temp = cell;
                break;
            }
        }
        if (temp == null)
            return null;

        //길찾기 알고리즘

        return route;
    }
}

public class NavCell
{
    //세 데이터 각각 점과 이웃Cell을 가진다
    public NavData[] datas = new NavData[3];

    Vector2 centroid;

    public Dictionary<NavEdge, NavCell> adjacencyCell = new Dictionary<NavEdge, NavCell>();

    public NavCell(Vector2 vec1, Vector2 vec2, Vector2 vec3, Vector2 Centroid)
    {
        datas[0] = new NavData(vec1);
        datas[1] = new NavData(vec2);
        datas[2] = new NavData(vec3);
        centroid = Centroid;
    }
    public Vector2 NextVertex(int index)
    {
        switch(index)
        {
            case 0:
                return datas[1].vertex;
            case 1:
                return datas[2].vertex;
            case 2:
                return datas[0].vertex;
        }
        return Vector2.zero;
    }
    public bool IsInArea(Vector2 position)
    {
        float signAB = Mathf.Sign((position.x - datas[0].vertex.x) * (datas[1].vertex.y - datas[0].vertex.y) - (position.y - datas[0].vertex.y) * (datas[1].vertex.x - datas[0].vertex.x));
        float signBC = Mathf.Sign((position.x - datas[1].vertex.x) * (datas[2].vertex.y - datas[1].vertex.y) - (position.y - datas[1].vertex.y) * (datas[2].vertex.x - datas[1].vertex.x));
        float signCA = Mathf.Sign((position.x - datas[2].vertex.x) * (datas[0].vertex.y - datas[2].vertex.y) - (position.y - datas[2].vertex.y) * (datas[0].vertex.x - datas[2].vertex.x));
        return (signAB == signBC) && (signBC == signCA);
    }
    public Vector2 Center { get { return (datas[0].vertex + datas[1].vertex + datas[2].vertex) / 3; } }

    //이웃Cell이면 노드참조 추가
    public bool CheakStuckAddCell(NavCell navCell)
    {
        List<int> equalNum = new List<int>();
        for (int i = 0; i < datas.Count(); i++)
        {
            for (int k = 0; k < navCell.datas.Count(); k++)
            {
                if (datas[i].vertex == navCell.datas[k].vertex)
                {
                    equalNum.Add(i);
                    continue;
                }
            }
        }
        // 조건을 만족하는 경우 해당 인덱스의 datas에 navCell을 추가하고 true를 반환합니다.
        if (equalNum[0] == (equalNum[1] + 1) % 3)
        {
            datas[equalNum[0]].AddCell(navCell);
            return true;
        }
        // 만족하는 조건이 없는 경우 false를 반환합니다.
        return false;
    }
}
public class NavData
{
    public Vector2 vertex;
    public NavCell stuckCell = null;

    public NavData(Vector2 vec)
    {
        this.vertex = vec;
    }
    public bool IsCellNull { get { return stuckCell == null; } }
    public bool AddCell(NavCell cell)
    {
        if (stuckCell != null)
        {
            stuckCell = cell;
            return false;
        }
        return false;
    }
}
public class NavEdge
{
    public Vector2 left;
    public Vector2 right;
}