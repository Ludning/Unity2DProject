using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BinarySpacePartition
{
    public Vector2Int mapSize;

    public BSPNode map;
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

    public BSPNode parentNode = null;

    public BSPNode leftNode;
    public BSPNode rightNode;

    public int Depth = 0;

    //최소면적 사이즈!
    public const int nodeMinimumArea = 400;

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
    public List<List<BSPNode>> GetNearNodes()
    {
        if (leftNode == null && rightNode == null)
            return null;
        var leftList = leftNode.GetLastNode();
        var rightList = rightNode.GetLastNode();

        BSPNode node1 = null;
        BSPNode node2 = null;

        List<BSPNode> nearPair = new List<BSPNode>();

        float gap = float.MaxValue;

        foreach (var lNode in leftList)
        {
            foreach (var rNode in rightList)
            {
                //float tempGap = CalculateGap(lNode.mapRect, rNode.mapRect);
                float tempGap = CalculateDistance(lNode.mapRect, rNode.mapRect);
                if (gap > tempGap)
                {
                    node1 = lNode;
                    node2 = rNode;
                    gap = tempGap;
                }
            }
        }
        nearPair.Add(node1);
        nearPair.Add(node2);

        
        
        var PairList = new List<List<BSPNode>>() { nearPair };

        var leftTemp = leftNode.GetNearNodes();
        var rightTemp = rightNode.GetNearNodes();
        if(leftTemp != null)
            PairList.AddRange(leftTemp);
        if(rightTemp != null)
            PairList.AddRange(rightTemp);

        return PairList;
    }
    private float CalculateDistance(RectInt rectA, RectInt rectB)
    {
        return new Vector2(Mathf.Abs(rectA.center.x - rectB.center.x), Mathf.Abs(rectA.center.y - rectB.center.y)).magnitude;
    }
    private float CalculateGap(RectInt rectA, RectInt rectB)
    {
        int horizontalGap = 0;
        int verticalGap = 0;

        // 수평 간격 계산
        if (rectA.xMax < rectB.xMin)
        {
            horizontalGap = rectB.xMin - rectA.xMax; // A가 B의 왼쪽에 있을 때
        }
        else if (rectB.xMax < rectA.xMin)
        {
            horizontalGap = rectA.xMin - rectB.xMax; // B가 A의 왼쪽에 있을 때
        }

        // 수직 간격 계산
        if (rectA.yMax < rectB.yMin)
        {
            verticalGap = rectB.yMin - rectA.yMax; // A가 B의 아래에 있을 때
        }
        else if (rectB.yMax < rectA.yMin)
        {
            verticalGap = rectA.yMin - rectB.yMax; // B가 A의 아래에 있을 때
        }

        return new Vector2Int(horizontalGap, verticalGap).magnitude;
    }
    public Vector2Int RandomPosition()
    {
        int x = Random.Range(mapRect.position.x, mapRect.position.x + mapRect.size.x + 1);
        int y = Random.Range(mapRect.position.y, mapRect.position.y + mapRect.size.y + 1);
        return new Vector2Int(x, y);
    }

    public void Partition()
    {
        //면적이 nodeMinimumArea보다 작은지 확인한다
        if (mapRect.size.x <=30 && mapRect.size.y<= 30)// <= nodeMinimumArea)
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
        //노드의 부모 설정
        leftNode.parentNode = this;
        rightNode.parentNode = this;
        //노드의 깊이 설정
        leftNode.Depth += 1;
        rightNode.Depth += 1;
        //노드분할
        leftNode.Partition();
        rightNode.Partition();
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
