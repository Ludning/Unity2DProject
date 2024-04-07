using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MarchingSquares// : MonoBehaviour
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

    bool[,] mapData = null;
    Dictionary<Vector2, MarchingType> MachingData = new Dictionary<Vector2, MarchingType>();
    public List<Edge> EdgeDatas = new List<Edge>();

    float distance = 1.0f;

    // Start is called before the first frame update
    public void Init()
    {
        mapData = TileMapToArrayEditor.ConvertTilemapToArray();
        //MarchingSquares������ ����
        GenerateMSData();
        //Patternó��, Edge����
        ProcessPattern();
        //Edge�ܼ�ȭ
        EdgeAbsorption();
        //Debug.Log(EdgeData.Count);
        //StartCoroutine(DrawMS());
    }
    public IEnumerator DrawMS()
    {
        while(true)
        {
            foreach (var data in EdgeDatas)
            {
                data.Draw();
            }
            yield return null;
        }
    }
    void EdgeAbsorption()
    {
        int cursor = 0;
        List<Edge> edges = new List<Edge>();
        while (true)
        {
            foreach(var data in EdgeDatas)
            {
                if(EdgeDatas[cursor].end == data.start)
                {
                    Debug.Log(Mathf.Abs(Vector2.Dot(EdgeDatas[cursor].direction, data.direction)));
                }
                if(EdgeDatas[cursor].end == data.start && Mathf.Abs(Vector2.Dot(EdgeDatas[cursor].direction, data.direction)) > Mathf.Cos(Mathf.Deg2Rad * 30))//30�� �̳��϶���
                {
                    edges.Add(new Edge(EdgeDatas[cursor].start, data.end));
                    EdgeDatas.Remove(EdgeDatas[cursor]);
                    EdgeDatas.Remove(data);
                    cursor--;
                    cursor--;
                    if (cursor < 0)
                        cursor = 0;
                    break;
                }
            }
            if(edges.Count > 0)
            {
                //Debug.Log($"{EdgeData.Remove(edges[0])}, {edges[0].start},{edges[0].end}");
                EdgeDatas.AddRange(edges);
                edges.Clear();
            }
            cursor++;
            if(cursor >= EdgeDatas.Count)
            {
                break;
            }
        }
    }

    void ProcessPattern()
    {
        foreach (var data in MachingData)
        {
            int pattern = (int)data.Value;
            Vector2 position = data.Key;
            //if (pattern == 0b0000) { HandlePattern0b0000(position); } else 
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

    #region HandlePattern
    // �� ���Ͽ� ���� ó���� �޼����
    //void HandlePattern0b0000(Vector2 position){ return; }
    void HandlePattern0b0001(Vector2 position) { EdgeDatas.Add(new Edge(Down(position), Left(position))); }
    void HandlePattern0b0010(Vector2 position){ EdgeDatas.Add(new Edge(Right(position), Down(position))); }
    void HandlePattern0b0011(Vector2 position){ EdgeDatas.Add(new Edge(Right(position), Left(position))); }
    void HandlePattern0b0100(Vector2 position){ EdgeDatas.Add(new Edge(Top(position), Right(position))); }
    void HandlePattern0b0101(Vector2 position){ EdgeDatas.Add(new Edge(Top(position), Right(position))); EdgeDatas.Add(new Edge(Down(position), Left(position))); }
    void HandlePattern0b0110(Vector2 position){ EdgeDatas.Add(new Edge(Top(position), Down(position))); }
    void HandlePattern0b0111(Vector2 position){ EdgeDatas.Add(new Edge(Top(position), Left(position))); }
    void HandlePattern0b1000(Vector2 position){ EdgeDatas.Add(new Edge(Left(position), Top(position))); }
    void HandlePattern0b1001(Vector2 position){ EdgeDatas.Add(new Edge(Down(position), Top(position))); }
    void HandlePattern0b1010(Vector2 position){ EdgeDatas.Add(new Edge(Left(position), Top(position))); EdgeDatas.Add(new Edge(Right(position), Down(position))); }
    void HandlePattern0b1011(Vector2 position){ EdgeDatas.Add(new Edge(Right(position), Top(position))); }
    void HandlePattern0b1100(Vector2 position){ EdgeDatas.Add(new Edge(Left(position), Right(position))); }
    void HandlePattern0b1101(Vector2 position){ EdgeDatas.Add(new Edge(Down(position), Right(position))); }
    void HandlePattern0b1110(Vector2 position) { EdgeDatas.Add(new Edge(Left(position), Down(position))); }
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

    /*// Down�� Left �Լ��� ������ ����ϵ��� ����
    Vector2 Top(Vector2 vec)
    {
        Vector2 start = new Vector2(vec.x, vec.y + distance); // ���� ��ġ
        Vector2 end = new Vector2(vec.x + distance, vec.y + distance); // ���� �������� �̵�

        // start�� end ���̿��� �����Ͽ� ���� ��� ��ġ�� ���
        return Interpolate(start, end);
    }
    Vector2 Down(Vector2 vec)
    {
        Vector2 start = new Vector2(vec.x, vec.y);
        Vector2 end = new Vector2(vec.x + distance, vec.y);

        // start�� end ���̿� �����Ͽ� ���� ��� ��ġ�� ���
        return Interpolate(start, end);
    }

    Vector2 Left(Vector2 vec)
    {
        Vector2 start = new Vector2(vec.x, vec.y);
        Vector2 end = new Vector2(vec.x, vec.y + distance);

        // start�� end ���̿� �����Ͽ� ���� ��� ��ġ�� ���
        return Interpolate(start, end);
    }
    Vector2 Right(Vector2 vec)
    {
        Vector2 start = new Vector2(vec.x + distance, vec.y);
        Vector2 end = new Vector2(vec.x + distance, vec.y + distance); // ������ �������� �̵�

        // start�� end ���̿��� �����Ͽ� ���� ��� ��ġ�� ���
        return Interpolate(start, end);
    }
    

    #region ���� �׽�Ʈ
    Vector2 Interpolate(Vector2 point1, Vector2 point2, float value1, float value2, float isoValue)
    {
        // �� ���� ���� isoValue�� ���ų�, ���� ���̰� ������,
        // �����ϰ� ù ��° ���� ��ȯ�մϴ�.
        if (Math.Abs(value1 - value2) < 0.0001 || value1 == isoValue)
        {
            return point1;
        }
        else if (value2 == isoValue)
        {
            return point2;
        }

        // ���� ������ ����Ͽ� ��� ��ġ ���
        float t = (isoValue - value1) / (value2 - value1);
        Vector2 interpolatedPoint = point1 + t * (point2 - point1);
        return interpolatedPoint;
    }
    #endregion*/

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

                if(temp != 0 || temp != 15)
                    MachingData.Add(new Vector2(w, h), (MarchingType)temp);
                temp = 0;
            }
        }
    }

    public bool GetLeftTop(int x, int y)
    {
        if(IsOutOfRange(x, y + 1))
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
        if(x < 0 || x > mapData.GetLength(1) || y < 0 || y > mapData.GetLength(0))
            return true;
        return false;
    }

    public class Edge
    {
        public Vector2 start;
        public Vector2 end;
        public Vector2 direction;
        public Edge(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            direction = (this.start - this.end).normalized;
        }
        public void Draw()
        {
            Debug.DrawLine(start, end);
            Debug.DrawLine(start, start + Vector2.one * 0.1f, Color.red);
        }
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
}
