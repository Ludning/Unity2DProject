using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMesh : MonoBehaviour
{
    List<NavCell> cells = new List<NavCell>();

    [SerializeField]
    Material material;

    Vector3[] vertices;

    [SerializeField]
    Transform start;
    [SerializeField]
    Transform end;

    List<Vector2> routes;


    private void Awake()
    {
        /*NavCell c1 = CreateCell(new Vector2(0, 10), new Vector2(10, 0), new Vector2(10, 10), null);
        NavCell c2 = CreateCell(new Vector2(10, 0), new Vector2(10, 10), new Vector2(20, 10), c1);
        NavCell c3 = CreateCell(new Vector2(10, 10), new Vector2(10, 20), new Vector2(20, 10), c2);*/

        //routes = PathFinding(start, end);
    }
    private void Start()
    {

    }
    private void Update()
    {


        //DrawNaviCells();
        //DrawRoot(routes);
    }
    public NavCell CreateCell(Vector2 vec1, Vector2 vec2, Vector2 vec3, NavCell cell)
    {
        NavCell temp = new NavCell(vec1, vec2, vec3);
        if(cell != null)
        {
            //NavCell t1 = temp.EmptyCell();
            //NavCell t2 = cell.EmptyCell();

            temp.EmptyCell = cell;
            cell.EmptyCell = temp;
        }
        cells.Add(temp);
        return temp;
    }
    /*public void CreateTriangle(Vector2 vec1, Vector2 vec2, Vector2 vec3)
    {
        *//*Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { vec1, vec2, vec3 };
        mesh.triangles = new int[] { 0, 1, 2 };

        // Mesh를 렌더링하는 GameObject 생성
        GameObject triangleObject = new GameObject("FilledTriangle");
        triangleObject.AddComponent<MeshFilter>().mesh = mesh;
        MeshRenderer mr = triangleObject.AddComponent<MeshRenderer>();
        mr.material = material;
        mr.material.color = RandColor();*//*
    }*/
    
    public void DrawNaviCells()
    {
        foreach (var cell in cells)
        {
            Debug.DrawLine(cell.vec1, cell.vec2, Color.red);
            Debug.DrawLine(cell.vec2, cell.vec3, Color.red);
            Debug.DrawLine(cell.vec3, cell.vec1, Color.red);
        }
    }

    public void AddCell(Vector2 vec1, Vector2 vec2, Vector2 vec3)
    {

    }
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

        route.Add(start);
        NavCell next = temp.ExistingCell;
        route.Add(next.Center);
        NavCell nNext = next.ExistingCell;
        route.Add(end);


        return route;
    }

    public void DrawRoot(List<Vector2> routes)
    {
        Vector2 vec1 = Vector2.zero;
        Vector2 vec2 = Vector2.zero;


        Debug.DrawLine(routes[0], routes[1], Color.red);
        Debug.DrawLine(routes[1], routes[2], Color.red);

        foreach (var route in routes)
        {
            Debug.Log(route);
        }
    }

    public Color RandColor()
    {
        // 랜덤한 RGB 값 생성
        float randomRed = UnityEngine.Random.Range(0f, 1f);
        float randomGreen = UnityEngine.Random.Range(0f, 1f);
        float randomBlue = UnityEngine.Random.Range(0f, 1f);
        // 알파 값은 보통 1로 설정하여 완전 불투명한 색을 얻습니다.
        float alpha = 1f;
        // Color 생성
        return new Color(randomRed, randomGreen, randomBlue, alpha);
    }
}
public class NavCell
{
    //세 점을 가지고 이웃Cell을 가진다
    public Vector2 vec1;
    public Vector2 vec2;
    public Vector2 vec3;

    public NavCell cell1;
    public NavCell cell2;
    public NavCell cell3;

    public NavCell(Vector2 vec1, Vector2 vec2, Vector2 vec3)
    {
        this.vec1 = vec1;
        this.vec2 = vec2;
        this.vec3 = vec3;
    }

    public bool IsInArea(Vector2 testP)
    {
        float signAB = Mathf.Sign((testP.x - vec1.x) * (vec2.y - vec1.y) - (testP.y - vec1.y) * (vec2.x - vec1.x));
        float signBC = Mathf.Sign((testP.x - vec2.x) * (vec3.y - vec2.y) - (testP.y - vec2.y) * (vec3.x - vec2.x));
        float signCA = Mathf.Sign((testP.x - vec3.x) * (vec1.y - vec3.y) - (testP.y - vec3.y) * (vec1.x - vec3.x));

        return (signAB == signBC) && (signBC == signCA);
    }
    public Vector2 Center
    {
        get
        {
            return (vec1 + vec2 + vec3) / 3;
        }
    }

    public NavCell EmptyCell 
    {  
        get 
        {
            if (cell1 == null)
                return cell1;
            else if (cell2 == null)
                return cell2;
            else if (cell3 == null)
                return cell3;
            else
                return null;
        } 
        set 
        {
            if (cell1 == null)
                cell1 = value;
            else if (cell2 == null)
                cell2 = value;
            else if (cell3 == null)
                cell3 = value;
        }
    }

    public NavCell ExistingCell
    {
        get 
        {
            if (cell1 != null)
                return cell1;
            else if (cell2 != null)
                return cell2;
            else if (cell3 != null)
                return cell3;
            else
                return null;
        }
        set
        {
            if (cell1 != null)
                cell1 = value;
            else if (cell2 != null)
                cell2 = value;
            else if (cell3 != null)
                cell3 = value;
        }
    }


    /*public NavCell EmptyCell()
    {
        if (cell1 == null)
            return cell1;
        else if (cell2 == null)
            return cell2;
        else if (cell3 == null)
            return cell3;
        else
            return null;
    }*/
}
