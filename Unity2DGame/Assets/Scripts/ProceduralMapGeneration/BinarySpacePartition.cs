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

    //�ּҸ��� ������!
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
        //������ nodeMinimumArea���� ������ Ȯ���Ѵ�
        if (mapRect.size.x * mapRect.size.y <= nodeMinimumArea)
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
        leftNode.Partition();
        rightNode.Partition();
    }

    //���ȿ� �� �����
    public void GenerateRoom()
    {

    }
    //��峢�� ����� �� �����
    public void GenerateRoad()
    {

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
