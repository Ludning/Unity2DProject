using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CellularAutomata : MonoBehaviour
{
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    [SerializeField] private int smoothNum;

    private const int PLAIN = 0;
    private const int EMPTY = 1;

    private const int ROAD = 2;

    //private const int WALL = 1;
    //private const int WALL = 1;
    //private const int WALL = 1;

    [SerializeField] private Tile tile;
    [SerializeField] private Tile routeTile;

    [Space]
    [SerializeField] private Tile upperLeftCorner;  //�»�
    [SerializeField] private Tile upperRightCorner; //���
    [SerializeField] private Tile bottomLeftCorner; //����
    [SerializeField] private Tile bottomRightCorner; //����
    [SerializeField] private Tile topCorner;        //��
    [SerializeField] private Tile bottomCorner;     //�Ʒ�
    [SerializeField] private Tile leftCorner;       //��
    [SerializeField] private Tile rightCorner;      //��

    [Space]
    [SerializeField] private Tile upperLeftEdge;  //�»�
    [SerializeField] private Tile upperRightEdge; //���
    [SerializeField] private Tile bottomLeftEdge; //����
    [SerializeField] private Tile bottomRightEdge; //����


    BinarySpacePartition BSP;
    Dictionary<BSPNode, Block> blockDic = new Dictionary<BSPNode, Block>();

    public void OnClickButton()
    {
        Clear();
        if (BSP == null)
            BSP = new BinarySpacePartition(new Vector2Int(70, 70));

        List<BSPNode> bspNodes = BSP.GetLastNode();

        GameObject frontParent = new GameObject("FrontGrid");
        GameObject backParent = new GameObject("BackGrid");
        GameObject wallParent = new GameObject("WallGrid");
        frontParent.AddComponent<Grid>();
        backParent.AddComponent<Grid>();
        wallParent.AddComponent<Grid>();

        foreach (BSPNode node in bspNodes)
        {
            //Ÿ�ϸ� ������Ʈ ����
            GameObject front = new GameObject("PlainTilemap");
            front.isStatic = true;
            front.AddComponent<TilemapRenderer>();
            front.transform.SetParent(frontParent.transform, false);
            front.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);

            GameObject back = new GameObject("BackTilemap");
            back.isStatic = true;
            back.AddComponent<TilemapRenderer>();
            back.transform.SetParent(backParent.transform, false);
            back.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);

            GameObject wall = new GameObject("WallTilemap");
            wall.isStatic = true;
            Rigidbody2D wallRigid = wall.AddComponent<Rigidbody2D>();
            wallRigid.isKinematic = true;
            wall.AddComponent<TilemapRenderer>();
            wall.transform.SetParent(wallParent.transform, false);
            wall.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);
            TilemapCollider2D tilemapCollider = wall.AddComponent<TilemapCollider2D>();
            wall.AddComponent<CompositeCollider2D>();
            tilemapCollider.usedByComposite = true;

            Tilemap frontTilemap = front.GetComponent<Tilemap>();
            Tilemap backTilemap = back.GetComponent<Tilemap>();
            Tilemap wallTilemap = wall.GetComponent<Tilemap>();

            blockDic.Add(node, new Block(frontTilemap, backTilemap, wallTilemap));

            //��� ���� ����

        }
        foreach (var block in blockDic)
        {
            GenerateByMapNode(block.Key, block.Value);
        }
        //GenerationRoute(bspNodes);
    }
    public void Clear()
    {
        blockDic = new Dictionary<BSPNode, Block>();
    }

    private void GenerateByMapNode(BSPNode node, Block block)
    {
        //�õ�� ���� �ǻ� ���� ����
        if (useRandomSeed) seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        block.map = new int[node.mapRect.width, node.mapRect.height];
        NodeRandomFill(node, block, pseudoRandom);

        for (int i = 0; i < smoothNum; i++) //�ݺ��� �������� ������ ������ �Ų���������.
            SmoothMap(node, block);

        //����ٰ� �� ������ش�
        GenerationRoute(node, block);

        OnDrawNode(node, block);

        FillWall(node, block);
    }

    //���� ������ ���� �� Ȥ�� �� �������� �����ϰ� ä��
    private void NodeRandomFill(BSPNode node, Block block, System.Random pseudoRandom)
    {
        //���� �� �����͸� ��������
        //���� �׸� �� ������� Route�� �׸���
        //�׸� �� Route�� ������ ������ �ڽų���� ������ �׸�
        //var data = BSP.map.GetNearNodes();

        for (int x = 0; x < node.mapRect.width; x++)
        {
            for (int y = 0; y < node.mapRect.height; y++)
            {
                //�����ڸ��� �� ���� ä��
                if (x == 0 || x == node.mapRect.width - 1 || y == 0 || y == node.mapRect.height - 1)
                    block.map[x, y] = EMPTY;
                //������ ���� �� ���� Ȥ�� �ٴ� ����
                else
                    block.map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? EMPTY : PLAIN;
            }
        }
    }
    //��� ä���
    private void OnDrawNode(BSPNode node, Block block)
    {
        for (int x = 0; x < node.mapRect.width; x++)
        {
            for (int y = 0; y < node.mapRect.height; y++)
            {
                OnDrawTile(node, block, x, y); //Ÿ�� ����
            }
        }
    }
    //Ÿ�� �׸���
    private void OnDrawTile(BSPNode node, Block block, int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);
        if (block.map[x, y] == EMPTY)
        {
            block.frontTilemap.SetTile(pos, null);
        }
        else if (block.map[x, y] == PLAIN)
        {
            block.frontTilemap.SetTile(pos, tile);
        }

        //Vector3Int pos = new Vector3Int(-node.mapRect.width / 2 + x, -node.mapRect.height / 2 + y, 0);
        /*if (map[pos.x, pos.y] == WALL)
        {
            tilemapDic[node].SetTile(pos, null);
        }
        else if (map[pos.x, pos.z] == ROAD)
        {
            tilemapDic[node].SetTile(pos, tile);
        }*/
    }
    //�׵θ� �ε巴��
    private void SmoothMap(BSPNode node, Block block)
    {
        for (int x = 0; x < node.mapRect.width; x++)
        {
            for (int y = 0; y < node.mapRect.height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(node, block, x, y);
                if (neighbourWallTiles > 4) block.map[x, y] = EMPTY; //�ֺ� ĭ �� ���� 4ĭ�� �ʰ��� ��� ���� Ÿ���� �� �������� �ٲ�
                else if (neighbourWallTiles < 4) block.map[x, y] = PLAIN; //�ֺ� ĭ �� ���� 4ĭ �̸��� ��� ���� Ÿ���� �ٴ����� �ٲ�
                //SetTileColor(node, x, y); //Ÿ�� ���� ����
            }
        }
    }
    //�� ����
    private void FillWall(BSPNode node, Block block)
    {
        for (int x = 0; x < node.mapRect.width; x++)
        {
            for (int y = 0; y < node.mapRect.height; y++)
            {
                //if (x >= 0 && x < node.mapRect.width && neighbourY >= 0 && neighbourY < node.mapRect.height);
                Vector3Int pos = new Vector3Int(x, y);
                int neighbourWallTiles = GetSurroundingWallCount(node, block, x, y);

                int wallPattern = GetWallPattern(node, block, x, y);
                int edgePattern = GetEdgePattern(node, block, x, y);
                //�ֺ� ĭ �� ���� 1ĭ�� �ʰ��� ��� ��Ÿ�� �׸���
                if (neighbourWallTiles >= 1 && block.map[x, y] == PLAIN)
                {
                    block.wallTilemap.SetTile(pos, ProcessWallPattern(wallPattern));
                }
                if(edgePattern != 0)
                    block.wallTilemap.SetTile(pos, ProcessEdgePattern(edgePattern));
            }
        }
    }

    #region ���� ���
    //�� �׵θ�
    private int GetWallPattern(BSPNode node, Block block, int gridX, int gridY)
    {
        int pattern = 0;

        if (gridY + 1 >= node.mapRect.height) pattern |= 1 << 3; //��
        else if (gridY + 1 < node.mapRect.height && block.map[gridX, gridY + 1] == EMPTY) pattern |= 1 << 3; //��

        if (gridX + 1 >= node.mapRect.width) pattern |= 1 << 2; //��
        else if (gridX + 1 < node.mapRect.width && block.map[gridX + 1, gridY] == EMPTY) pattern |= 1 << 2; //��

        if (gridY - 1 < 0) pattern |= 1 << 1; //�Ʒ�
        else if (gridY - 1 >= 0 && block.map[gridX, gridY - 1] == EMPTY) pattern |= 1 << 1; //�Ʒ�

        if (gridX - 1 < 0) pattern |= 1 << 0; //��
        else if (gridX - 1 >= 0 && block.map[gridX - 1, gridY] == EMPTY) pattern |= 1 << 0; //��

        return pattern;
    }
    //�� �𼭸�
    private int GetEdgePattern(BSPNode node, Block block, int gridX, int gridY)
    {
        int pattern = 0;

        if(GetWallPattern(node, block, gridX, gridY) == 0b0000)
        {
            if ((gridX + 1 < 0 && gridY + 1 >= node.mapRect.height)) return 1 << 3;//�»�
            else if ((gridX - 1 >= 0 && gridY + 1 < node.mapRect.height) && block.map[gridX - 1, gridY + 1] == EMPTY) return 1 << 3; //�»�

            if ((gridX + 1 >= node.mapRect.width && gridY + 1 >= node.mapRect.height)) return 1 << 2; //���
            else if ((gridX + 1 < node.mapRect.width && gridY + 1 < node.mapRect.height) && block.map[gridX + 1, gridY + 1] == EMPTY) return 1 << 2; //���

            if ((gridX + 1 >= node.mapRect.width && gridY - 1 < 0)) return 1 << 0; //����
            else if ((gridX + 1 < node.mapRect.width && gridY - 1 >= 0) && block.map[gridX + 1, gridY - 1] == EMPTY) return 1 << 1; //����

            if ((gridX + 1 < 0 && gridY + 1 < 0)) return 1 << 1; //����
            else if ((gridX - 1 >= 0 && gridY - 1 >= 0) && block.map[gridX - 1, gridY - 1] == EMPTY) return 1 << 0; //����
        }

        return pattern;
    }
    //�� ����ȭ
    private int GetSurroundingWallCount(BSPNode node, Block block, int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //���� ��ǥ�� �������� �ֺ� 8ĭ �˻�
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < node.mapRect.width && neighbourY >= 0 && neighbourY < node.mapRect.height)
                { 
                    //�� ������ �ʰ����� �ʰ� ���ǹ����� �˻�
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += block.map[neighbourX, neighbourY]; //���� 1�̰� �� ������ 0�̹Ƿ� ���� ��� wallCount ����
                }
                else wallCount++; //�ֺ� Ÿ���� �� ������ ��� ��� wallCount ����
            }
        }
        return wallCount;
    }
    #endregion

    #region ���� ó��
    private Tile ProcessWallPattern(int pattern)
    {
        //���� ���
        /*
        
            1
          4 �� 2
            3
        �»�� �𼭸�
        ���� �𼭸�
        
        ���ϴ� �𼭸�
        ���ϴ� �𼭸�
        
        ��� �𼭸�
        �ϴ� �𼭸�
        �´� �𼭸�
        ��� �𼭸�
        */
        if (pattern == 0b1001) { return upperLeftCorner; }
        else if (pattern == 0b1100) { return upperRightCorner; }
        else if (pattern == 0b0011) { return bottomLeftCorner; }
        else if (pattern == 0b0110) { return bottomRightCorner; }
        else if (pattern == 0b1000) { return topCorner; }
        else if (pattern == 0b0010) { return bottomCorner; }
        else if (pattern == 0b0001) { return leftCorner; }
        else if (pattern == 0b0100) { return rightCorner; }
        else return null; 
    }
    private Tile ProcessEdgePattern(int pattern)
    {
        //���� ���
        /*
        
          1   2
            �� 
          3   4

        �»�� �𼭸�
        ���� �𼭸�
        ���ϴ� �𼭸�
        ���ϴ� �𼭸�
        */
        if (pattern == 0b1000) { return upperLeftEdge; }
        else if (pattern == 0b0100) { return upperRightEdge; }
        else if (pattern == 0b0010) { return bottomRightEdge; }
        else if (pattern == 0b0001) { return bottomLeftEdge; }
        else return null;
    }
    #endregion

    public void GenerationRoute(BSPNode node, Block block)
    {
        var data = BSP.map.GetNearNodes();

        var nearDatas = data.Where(nodePair => nodePair.Contains(node)).ToList();

        foreach (List<BSPNode> nearData in nearDatas)
        {
            if(nearData.Contains(node))
            {
                BSPNode other = nearData.Find(x => x != node);
                //node to other
                Vector2Int start = new Vector2Int((int)(node.mapRect.center.x), (int)(node.mapRect.center.y));
                Vector2Int end = new Vector2Int((int)(other.mapRect.center.x), (int)(other.mapRect.center.y));

                
                DrawRoute(node, block, start, end);
            }
        }
    }
    void DrawRoute(BSPNode node, Block block, Vector2Int start, Vector2Int end)
    {
        //start�� node�� center
        //end�� ��Ÿ
        //start���� ��� �������� ������ �� ���ϱ�

        Vector2Int vec = start - end;


        if(vec.x > vec.y)
        {

        }




        /*//��
        if ()
        {

        }
        //�Ʒ�
        else if (start.y > end.y)
        {

        }
        //������
        if (start.x < end.x)
        {

        }
        //����
        else if (start.x > end.x)
        {

        }*/
        


        /*//����
        if(start.x == end.x)
        {
            int minY = Mathf.Min(start.y, end.y);
            int maxY = Mathf.Max(start.y, end.y);
            for (int y = minY; y <= maxY; y++)
            {
                if (node.mapRect.height <= y)
                    break;
                block.map[start.x, y] = ROAD;
                //tilemap.SetTile(new Vector3Int(x1, y, 0), routeTile);
            }
        }
        //����
        else if(start.y == end.y)
        {
            int minX = Mathf.Min(start.x, end.x);
            int maxX = Mathf.Max(start.x, end.x);
            for (int x = minX; x <= maxX; x++)
            {
                if (node.mapRect.width <= x)
                    break;
                block.map[x, start.y] = ROAD;
                //tilemap.SetTile(new Vector3Int(x, y1, 0), routeTile);
            }
        }*/



        /*int xStart = start.x;
        int yStart = start.y;
        int xEnd = end.x;
        int yEnd = end.y;

        int xStartPos = startPos.x;
        int yStartPos = startPos.y;
        int xEndPos = endPos.x;
        int yEndPos = endPos.y;

        if (xStart == xEnd) // Vertical line
        {
            int minY = Mathf.Min(yStart, yEnd) - Mathf.Min(yStartPos, yEndPos);
            int maxY = Mathf.Max(yStart, yEnd) - Mathf.Min(yStartPos, yEndPos);
            for (int y = minY; y <= maxY; y++)
            {
                if (node.mapRect.height <= y)
                    break;
                block.map[xStart, y] = ROAD;
                //tilemap.SetTile(new Vector3Int(x1, y, 0), routeTile);
            }
        }
        else if (yStart == yEnd) // Horizontal line
        {
            int minX = Mathf.Min(xStart, xEnd) - Mathf.Min(xStartPos, xEndPos);
            int maxX = Mathf.Max(xStart, xEnd) - Mathf.Min(xStartPos, xEndPos);
            for (int x = minX; x <= maxX; x++)
            {
                if (node.mapRect.width <= x)
                    break;
                block.map[x, yStart] = ROAD;
                //tilemap.SetTile(new Vector3Int(x, y1, 0), routeTile);
            }
        }
        else // Right angle
        {
            int minX = Mathf.Min(xStart, xEnd);
            int maxX = Mathf.Max(xStart, xEnd);
            int minY = Mathf.Min(yStart, yEnd);
            int maxY = Mathf.Max(yStart, yEnd);
            for (int x = minX; x <= maxX; x++)
            {
                if (node.mapRect.width <= x)
                    break;
                block.map[x, yStart] = ROAD;
                //tilemap.SetTile(new Vector3Int(x, y1, 0), tileBase1);
            }
            for (int y = minY; y <= maxY; y++)
            {
                if (node.mapRect.height <= y)
                    break;
                block.map[xEnd, y] = ROAD;
                //tilemap.SetTile(new Vector3Int(x2, y, 0), tileBase1);
            }
        }*/
    }
}

//���, ������ ���� Block���� ����
public class Block
{
    public Tilemap frontTilemap;
    public Tilemap backTilemap;
    public Tilemap wallTilemap;
    public int[,] map;

    public Block(Tilemap frontTilemap, Tilemap backTilemap, Tilemap wallTilemap)
    {
        this.frontTilemap = frontTilemap;
        this.backTilemap = backTilemap;
        this.wallTilemap = wallTilemap;
    }
}