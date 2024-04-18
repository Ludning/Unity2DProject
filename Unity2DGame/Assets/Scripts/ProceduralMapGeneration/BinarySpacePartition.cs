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

    //�ּҸ��� ������!
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

        // ���� ���� ���
        if (rectA.xMax < rectB.xMin)
        {
            horizontalGap = rectB.xMin - rectA.xMax; // A�� B�� ���ʿ� ���� ��
        }
        else if (rectB.xMax < rectA.xMin)
        {
            horizontalGap = rectA.xMin - rectB.xMax; // B�� A�� ���ʿ� ���� ��
        }

        // ���� ���� ���
        if (rectA.yMax < rectB.yMin)
        {
            verticalGap = rectB.yMin - rectA.yMax; // A�� B�� �Ʒ��� ���� ��
        }
        else if (rectB.yMax < rectA.yMin)
        {
            verticalGap = rectA.yMin - rectB.yMax; // B�� A�� �Ʒ��� ���� ��
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
        //������ nodeMinimumArea���� ������ Ȯ���Ѵ�
        if (mapRect.size.x <=30 && mapRect.size.y<= 30)// <= nodeMinimumArea)
            return;
        //���ʰ� ������ ���� ������ �Ҵ��Ѵ�
        RectInt leftRect;
        RectInt rightRect;
        //���ΰ� ���κ��� Ŭ ���
        if (mapRect.width >= mapRect.height)
        {
            //�����Ѵ�
            int length = mapRect.width / 3;
            //3���� 1���� 3���� 2�������� ������ ��ġ�� �����Ѵ�
            int width = Random.Range(length, length * 2);
            //������ ���� �������� �ڸ���
            leftRect = new RectInt(mapRect.position, new Vector2Int(width, mapRect.height));
            rightRect = new RectInt(new Vector2Int(mapRect.position.x + width, mapRect.position.y), new Vector2Int(mapRect.width - width, mapRect.height));
        }
        //���ΰ� ���κ��� Ŭ ���
        else
        {
            //�����Ѵ�
            int length = mapRect.height / 3;
            //3���� 1���� 3���� 2�������� ������ ��ġ�� �����Ѵ�
            int height = Random.Range(length, length * 2);
            //������ ���� �������� �ڸ���, ���⼭ leftRect�� �Ʒ�, rightRect�� ��
            leftRect = new RectInt(mapRect.position, new Vector2Int(mapRect.width, height));
            rightRect = new RectInt(new Vector2Int(mapRect.position.x, mapRect.position.y + height), new Vector2Int(mapRect.width, mapRect.height - height));
        }
        //���� ���� ������ ��嵵 ������ ����
        leftNode = new BSPNode(leftRect);
        rightNode = new BSPNode(rightRect);
        //����� �θ� ����
        leftNode.parentNode = this;
        rightNode.parentNode = this;
        //����� ���� ����
        leftNode.Depth += 1;
        rightNode.Depth += 1;
        //������
        leftNode.Partition();
        rightNode.Partition();
    }

}

public enum NodeType
{
    //�ܵ�
    Grass,
    //��
    Road,
    //������
    StoneGround,
    //��
    Wall,
}
public enum PropType
{
    //����Ʈ, ����
    Gate,
    //����
    Altar,
    //�� �ʿ��� �Ѱ�~�ΰ��� ����
    Chest,
    //ǥ����
    RoadSign,
    //��ũ��
    Barrel,
    //����
    Gravestone,
    //����
    Tree,
    //����
    Bush,
    //���浹 ������Ʈ
    NonCollider,
}
