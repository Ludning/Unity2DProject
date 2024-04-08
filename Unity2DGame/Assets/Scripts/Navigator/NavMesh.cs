using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class NavMesh : MonoBehaviour
{
    List<NavCell> cells = new List<NavCell>();

    MarchingSquares marchingSquares = new MarchingSquares();

    private void Start()
    {
        marchingSquares.Init();

        StartCoroutine(marchingSquares.DrawContourLines());
        /*AddCell(new Vector2(0, 0), new Vector2(5, 0), new Vector2(0, 5));
        AddCell(new Vector2(0, 0), new Vector2(5, 0), new Vector2(0, -5));
        AddCell(new Vector2(0, 0), new Vector2(-5, 0), new Vector2(0, -5));*/

        //�ﰢ�� ��� �Լ� ����
        //Triangulation();

        StartCoroutine(DrawNavCells());
    }
   
    public bool IsIncludedCell(Vector2 vec1, Vector2 vec2, Vector2 vec3)
    {
        //�ϴ� �� ���� ������ �ִ� cell�� �ִ��� Ȯ���Ѵ�
        //������ true
        //�ƴϸ� false
        Vector2[] vecs = new Vector2[3] { vec1, vec2, vec3 };
        foreach (var cell in cells)
        {
            if (cell.datas.All(x => vecs.Contains(x.vertex)))
            {
                return true;
            }
        }
        return false;
    }
    //�� �׸��� �Լ�
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
    //�� �߰� �Լ�
    public void AddCell(Vector2 vec1, Vector2 vec2, Vector2 vec3)
    {
        cells.Add(new NavCell(vec1, vec2, vec3));
    }
    //��ã�� �Լ�
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

        //��ã�� �˰���

        return route;
    }
}

public class NavCell
{
    //�� ������ ���� ���� �̿�Cell�� ������
    public NavData[] datas = new NavData[3];

    public NavCell(Vector2 vec1, Vector2 vec2, Vector2 vec3)
    {
        datas[0] = new NavData(vec1);
        datas[1] = new NavData(vec2);
        datas[2] = new NavData(vec3);
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

    //�̿�Cell�̸� ������� �߰�
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
        // ������ �����ϴ� ��� �ش� �ε����� datas�� navCell�� �߰��ϰ� true�� ��ȯ�մϴ�.
        if (equalNum[0] == (equalNum[1] + 1) % 3)
        {
            datas[equalNum[0]].AddCell(navCell);
            return true;
        }
        // �����ϴ� ������ ���� ��� false�� ��ȯ�մϴ�.
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