using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BinarySpacePartition
{
    public Vector2Int mapSize;

    BSPNode map;
    public BinarySpacePartition(Vector2Int size)
    {
        mapSize = size;
        map = new BSPNode(new RectInt(new Vector2Int(0, 0), mapSize));
        map.Partition();
    }
    public List<BSPNode> GetLastNode()
    {
        return map.GetLastNode();
    }
}

public class BSPNode
{
    public RectInt mapRect;

    BSPNode leftNode;
    BSPNode rightNode;

    //최소면적 사이즈!
    public const int nodeMinimumArea = 500;

    public BSPNode(RectInt area)
    {
        mapRect = area;
    }

    public List<BSPNode> GetLastNode()
    {
        List<BSPNode> nodes = new List<BSPNode>();
        if (leftNode != null)
            nodes.AddRange(leftNode.GetLastNode());
        if(rightNode != null)
            nodes.AddRange(rightNode.GetLastNode());
        if (leftNode == null && rightNode == null)
            nodes.Add(this);
        return nodes;
    }

    public void Partition()
    {
        //면적이 nodeMinimumArea보다 작은지 확인한다
        if (mapRect.size.x * mapRect.size.y <= nodeMinimumArea)
            return;
        //왼쪽과 오른쪽 노드로 분할후 할당한다
        RectInt leftRect;
        RectInt rightRect;
        //가로가 세로보다 클 경우
        if (mapRect.width >= mapRect.height)
        {
            //분할한다
            int length = mapRect.width / 3;
            //3분의 1부터 3분의 2지점까지 랜덤한 위치를 선택한다
            int width = Random.Range(length, length * 2);
            //선택한 값을 기준으로 자른다
            leftRect = new RectInt(mapRect.position, new Vector2Int(width, mapRect.height));
            rightRect = new RectInt(new Vector2Int(mapRect.position.x + width, mapRect.position.y), new Vector2Int(mapRect.width - width, mapRect.height));
        }
        //세로가 가로보다 클 경우
        else
        {
            //분할한다
            int length = mapRect.height / 3;
            //3분의 1부터 3분의 2지점까지 랜덤한 위치를 선택한다
            int height = Random.Range(length, length * 2);
            //선택한 값을 기준으로 자른다, 여기서 leftRect는 아래, rightRect는 위
            leftRect = new RectInt(mapRect.position, new Vector2Int(mapRect.width, height));
            rightRect = new RectInt(new Vector2Int(mapRect.position.x, mapRect.position.y + height), new Vector2Int(mapRect.width, mapRect.height - height));
        }
        //왼쪽 노드와 오른쪽 노드도 나누러 간다
        leftNode = new BSPNode(leftRect);
        rightNode = new BSPNode(rightRect);
        leftNode.Partition();
        rightNode.Partition();
    }

    //노드안에 방 만들기
    public void GenerateRoom()
    {

    }
    //노드끼리 연결된 길 만들기
    public void GenerateRoad()
    {

    }
}

public enum NodeType
{
    //잔디
    Grass,
    //길
    Road,
    //돌제단
    StoneGround,
    //벽
    Wall,
}
public enum PropType
{
    //게이트, 관문
    Gate,
    //제단
    Altar,
    //한 맵에는 한개~두개의 상자
    Chest,
    //표지판
    RoadSign,
    //오크통
    Barrel,
    //묘비
    Gravestone,
    //나무
    Tree,
    //덤불
    Bush,
    //무충돌 오브젝트
    NonCollider,
}
