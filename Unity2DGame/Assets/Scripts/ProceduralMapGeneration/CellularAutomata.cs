using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

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


    [SerializeField] private List<Tile> planeTiles;

    [SerializeField] private Tile routeTile;

    [SerializeField] private Tile backgroungTile;

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
    public Dictionary<BSPNode, Block> blockDic = new Dictionary<BSPNode, Block>();

    public NavMeshSurface Surface2D;

    Dictionary<BSPNode, List<BSPNode>> adjacencyNodeDic = null;
    public Dictionary<BSPNode, List<BSPNode>> AdjacencyNodeDic
    {
        get
        {
            if (adjacencyNodeDic == null)
                adjacencyNodeDic = BSP.GetAdjacencyList();
            return adjacencyNodeDic;
        }
    }

    public Vector2 RandomSpawnPos(Block block = null)
    {
        if (block == null)
            block = blockDic.First().Value;
        int width = block.map.GetLength(0);
        int height = block.map.GetLength(1);

        int xPos;
        int yPos;
        while (true)
        {
            xPos = UnityEngine.Random.Range(0, width);
            yPos = UnityEngine.Random.Range(0, height);
            if (block.map[xPos, yPos] == PLAIN)
                break;
        }
        Vector3Int tilePos = new Vector3Int(xPos, yPos);
        return block.frontTilemap.CellToWorld(tilePos);
    }
    public Vector2 StartBlockPosition()
    {
        Block block = blockDic.First().Value;
        int xCenter = (int)(block.map.GetLength(0) * 0.5f);
        int yCenter = (int)(block.map.GetLength(1) * 0.5f);

        Vector3Int tilePos = Vector3Int.zero;
        if (block.map[xCenter, yCenter] != PLAIN)
            tilePos = FindNearestPlainTile(block.map, xCenter, yCenter);
        tilePos = new Vector3Int(xCenter, yCenter);
        return block.frontTilemap.CellToWorld(tilePos);
    }
    Vector3Int FindNearestPlainTile(int[,] map, int startX, int startY)
    {
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(new Vector3Int(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nx = (int)current.x + dx[i];
                int ny = (int)current.y + dy[i];

                if (nx >= 0 && nx < map.GetLength(0) && ny >= 0 && ny < map.GetLength(1) && !visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    if (map[nx, ny] == PLAIN)
                    {
                        return new Vector3Int(nx, ny);
                    }
                    queue.Enqueue(new Vector3Int(nx, ny));
                }
            }
        }

        return Vector3Int.zero;
    }

    public void OnGeneraterMap()
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
            //front.isStatic = true;
            TilemapRenderer frontRenderer = front.AddComponent<TilemapRenderer>();
            frontRenderer.sortingLayerName = "Background";
            front.transform.SetParent(frontParent.transform, false);
            front.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);
            //�׺�޽�
            NavMeshModifier nmF = front.AddComponent<NavMeshModifier>();
            nmF.overrideArea = true;
            nmF.area = 0;


            GameObject back = new GameObject("BackTilemap");
            //back.isStatic = true;
            Rigidbody2D backRigid = back.AddComponent<Rigidbody2D>();
            backRigid.isKinematic = true;
            TilemapRenderer backRenderer = back.AddComponent<TilemapRenderer>();
            backRenderer.sortingLayerName = "Frontground";
            back.transform.SetParent(backParent.transform, false);
            back.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);
            TilemapCollider2D tilemapCollider = back.AddComponent<TilemapCollider2D>();
            back.AddComponent<CompositeCollider2D>();
            tilemapCollider.usedByComposite = true;
            //�׺�޽�
            NavMeshModifier nmB = back.AddComponent<NavMeshModifier>();
            nmB.overrideArea = true;
            nmB.area = 1;


            GameObject wall = new GameObject("WallTilemap");
            //wall.isStatic = true;
            TilemapRenderer wallRenderer = wall.AddComponent<TilemapRenderer>();
            wallRenderer.sortingLayerName = "Wall";
            wall.transform.SetParent(wallParent.transform, false);
            wall.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);
            

            Tilemap frontTilemap = front.GetComponent<Tilemap>();
            Tilemap backTilemap = back.GetComponent<Tilemap>();
            Tilemap wallTilemap = wall.GetComponent<Tilemap>();

            blockDic.Add(node, new Block(node, frontTilemap, backTilemap, wallTilemap));
        }

        GenerateByMapNode(blockDic);

        GeneratorTilemapCurrlingData();
    }

    private void GeneratorTilemapCurrlingData()
    {
        var adjacencyDatas = BSP.GetAdjacencyList();
        foreach (var data in adjacencyDatas)
        {
            foreach (var adjNode in data.Value)
            {
                if (blockDic.ContainsKey(data.Key) && blockDic.ContainsKey(adjNode))
                {
                    blockDic[data.Key].adjacencyBlock.Add(blockDic[adjNode]);
                }
            }
        }
    }

    public void Clear()
    {
        blockDic = new Dictionary<BSPNode, Block>();
    }
    private void Start()
    {
        Surface2D.BuildNavMeshAsync();
    }

    private void GenerateByMapNode(Dictionary<BSPNode, Block> blockDic)
    {
        foreach (var block in blockDic.Values)
        {
            //�õ�� ���� �ǻ� ���� ����
            if (useRandomSeed) seed = Time.time.ToString();
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            block.map = new int[block.node.mapRect.width, block.node.mapRect.height];
            NodeRandomFill(block, pseudoRandom);

            for (int i = 0; i < smoothNum; i++) //�ݺ��� �������� ������ ������ �Ų���������.
                SmoothMap(block);
        }
        
        GenerationRoute(blockDic.Values.ToList());

        foreach (var block in blockDic.Values)
        {
            OnDrawNode(block);

            FillWall(block);
        }

        foreach(var block in blockDic.Values)
        {
            FillBackground(block);
        }
    }
    private void FillBackground(Block block)
    {
        // ù ��° Ÿ�ϸ��� ��� ���� ��ȸ
        BoundsInt bounds = block.frontTilemap.cellBounds;
        
        for (int x = 0; x < block.map.GetLength(0); x++)
        {
            for (int y = 0; y < block.map.GetLength(1); y++)
            {
                Tile tile = (block.map[x,y] == EMPTY) ? backgroungTile : null;

                // ù ��° Ÿ�ϸʿ� Ÿ���� ���� ��� �� ��° Ÿ�ϸʿ� Ÿ���� ä��ϴ�.
                if (tile == backgroungTile)
                {
                    Vector3Int pos = new Vector3Int(x, y);
                    block.backTilemap.SetTile(pos, tile);
                }
            }
        }
    }

    //���� ������ ���� �� Ȥ�� �� �������� �����ϰ� ä��
    private void NodeRandomFill(Block block, System.Random pseudoRandom)
    {
        //���� �� �����͸� ��������
        //���� �׸� �� ������� Route�� �׸���
        //�׸� �� Route�� ������ ������ �ڽų���� ������ �׸�
        //var data = BSP.map.GetNearNodes();

        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                //�����ڸ��� �� ���� ä��
                if (x == 0 || x == block.node.mapRect.width - 1 || y == 0 || y == block.node.mapRect.height - 1)
                    block.map[x, y] = EMPTY;
                //������ ���� �� ���� Ȥ�� �ٴ� ����
                else
                    block.map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? EMPTY : PLAIN;
            }
        }
    }
    //��� ä���
    private void OnDrawNode(Block block)
    {
        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                OnDrawTile(block, x, y); //Ÿ�� ����
            }
        }
    }
    //Ÿ�� �׸���
    private void OnDrawTile(Block block, int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);
        if (block.map[x, y] == EMPTY)
        {
            block.frontTilemap.SetTile(pos, null);
        }
        else if (block.map[x, y] == PLAIN)
        {
            int index = UnityEngine.Random.Range(0, planeTiles.Count);
            block.frontTilemap.SetTile(pos, planeTiles[index]);
        }
        else if (block.map[x, y] == ROAD)
        {
            block.frontTilemap.SetTile(pos, routeTile);
        }
    }
    //�׵θ� �ε巴��
    private void SmoothMap(Block block)
    {
        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(block, x, y);
                if (neighbourWallTiles > 4) block.map[x, y] = EMPTY; //�ֺ� ĭ �� ���� 4ĭ�� �ʰ��� ��� ���� Ÿ���� �� �������� �ٲ�
                else if (neighbourWallTiles < 4) block.map[x, y] = PLAIN; //�ֺ� ĭ �� ���� 4ĭ �̸��� ��� ���� Ÿ���� �ٴ����� �ٲ�
                //SetTileColor(node, x, y); //Ÿ�� ���� ����
            }
        }
    }
    //�� ����
    private void FillWall(Block block)
    {
        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                //if (x >= 0 && x < node.mapRect.width && neighbourY >= 0 && neighbourY < node.mapRect.height);
                Vector3Int pos = new Vector3Int(x, y);
                int neighbourWallTiles = GetSurroundingWallCount(block, x, y);

                int wallPattern = GetWallPattern(block, x, y);
                int edgePattern = GetEdgePattern(block, x, y);
                //�ֺ� ĭ �� ���� 1ĭ�� �ʰ��� ��� ��Ÿ�� �׸���
                if (neighbourWallTiles >= 1 && (block.map[x, y] == PLAIN || block.map[x, y] == ROAD))
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
    private int GetWallPattern(Block block, int gridX, int gridY)
    {
        int pattern = 0;

        //��輱�� �����鼭 �ڽ��� ROAD�� ��
        //��
        if (gridY == block.node.mapRect.height - 1 && block.map[gridX, gridY] == ROAD)
        {
            //��
            if (gridX - 1 < 0 || block.map[gridX - 1, gridY] == EMPTY)
                return pattern |= 1 << 0;
            //��
            if (gridX + 1 >= block.node.mapRect.width || block.map[gridX + 1, gridY] == EMPTY)
                return pattern |= 1 << 2;
            
        }
        //��
        if (gridX == block.node.mapRect.width - 1 && block.map[gridX, gridY] == ROAD)
        {
            //��
            if (gridY + 1 >= block.node.mapRect.height || block.map[gridX, gridY + 1] == EMPTY)
                return pattern |= 1 << 3;
            //�Ʒ�
            if (gridY - 1 < 0 || block.map[gridX, gridY - 1] == EMPTY)
                return pattern |= 1 << 1;
        }
        //�Ʒ�
        if (gridY == 0 && block.map[gridX, gridY] == ROAD)
        {
            //��
            if (gridX - 1 < 0 || block.map[gridX - 1, gridY] == EMPTY)
                return pattern |= 1 << 0;
            //��
            if (gridX + 1 >= block.node.mapRect.width || block.map[gridX + 1, gridY] == EMPTY)
                return pattern |= 1 << 2;
        }
        //��
        if (gridX == 0 && block.map[gridX, gridY] == ROAD)
        {
            //��
            if (gridY + 1 >= block.node.mapRect.height || block.map[gridX, gridY + 1] == EMPTY)
                return pattern |= 1 << 3;
            //�Ʒ�
            if (gridY - 1 < 0 || block.map[gridX, gridY - 1] == EMPTY)
                return pattern |= 1 << 1;
        }



        if (gridY + 1 >= block.node.mapRect.height) pattern |= 1 << 3; //��
        else if (gridY + 1 < block.node.mapRect.height && block.map[gridX, gridY + 1] == EMPTY) pattern |= 1 << 3; //��

        if (gridX + 1 >= block.node.mapRect.width) pattern |= 1 << 2; //��
        else if (gridX + 1 < block.node.mapRect.width && block.map[gridX + 1, gridY] == EMPTY) pattern |= 1 << 2; //��

        if (gridY - 1 < 0) pattern |= 1 << 1; //�Ʒ�
        else if (gridY - 1 >= 0 && block.map[gridX, gridY - 1] == EMPTY) pattern |= 1 << 1; //�Ʒ�

        if (gridX - 1 < 0) pattern |= 1 << 0; //��
        else if (gridX - 1 >= 0 && block.map[gridX - 1, gridY] == EMPTY) pattern |= 1 << 0; //��

        return pattern;
    }
    //�� �𼭸�
    private int GetEdgePattern(Block block, int gridX, int gridY)
    {
        int pattern = 0;

        if(GetWallPattern(block, gridX, gridY) == 0b0000)
        {
            if ((gridX + 1 < 0 && gridY + 1 >= block.node.mapRect.height)) return 1 << 3;//�»�
            else if ((gridX - 1 >= 0 && gridY + 1 < block.node.mapRect.height) && block.map[gridX - 1, gridY + 1] == EMPTY) return 1 << 3; //�»�

            if ((gridX + 1 >= block.node.mapRect.width && gridY + 1 >= block.node.mapRect.height)) return 1 << 2; //���
            else if ((gridX + 1 < block.node.mapRect.width && gridY + 1 < block.node.mapRect.height) && block.map[gridX + 1, gridY + 1] == EMPTY) return 1 << 2; //���

            if ((gridX + 1 >= block.node.mapRect.width && gridY - 1 < 0)) return 1 << 0; //����
            else if ((gridX + 1 < block.node.mapRect.width && gridY - 1 >= 0) && block.map[gridX + 1, gridY - 1] == EMPTY) return 1 << 1; //����

            if ((gridX + 1 < 0 && gridY + 1 < 0)) return 1 << 1; //����
            else if ((gridX - 1 >= 0 && gridY - 1 >= 0) && block.map[gridX - 1, gridY - 1] == EMPTY) return 1 << 0; //����
        }

        return pattern;
    }
    //�� ����ȭ
    private int GetSurroundingWallCount(Block block, int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //���� ��ǥ�� �������� �ֺ� 8ĭ �˻�
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < block.node.mapRect.width && neighbourY >= 0 && neighbourY < block.node.mapRect.height)
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

    public void GenerationRoute(List<Block> blockList)
    {
        var data = BSP.map.GetNearNodes();
        foreach (List<BSPNode> node in data)
        {
            Block node0 = blockList.Find(x => x.node == node[0]);
            Block node1 = blockList.Find(x => x.node == node[1]);
            //LinkRoute(node[0], node[1]);
            DrawLine(node0, node1);
        }
    }
    void DrawLine(Block left, Block right)
    {
        Vector2Int start = new Vector2Int((int)left.node.mapRect.center.x, (int)left.node.mapRect.center.y);
        Vector2Int end = new Vector2Int((int)right.node.mapRect.center.x, (int)right.node.mapRect.center.y);

        int x1 = start.x;
        int y1 = start.y;
        int x2 = end.x;
        int y2 = end.y;

        //Vector2Int leftPos = start.magnitude > end.magnitude ? left.node.mapRect.position : right.node.mapRect.position;
        Vector2Int leftPos = left.node.mapRect.position;
        Vector2Int rightPos = right.node.mapRect.position;

        if (x1 == x2) // Vertical line
        {
            int minY = Mathf.Min(y1, y2);
            int maxY = Mathf.Max(y1, y2);
            for (int y = minY; y <= maxY; y++)
            {
                if (left.node.mapRect.Contains(new Vector2Int(x1, y)))
                    left.map[x1 - leftPos.x, y - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x1 + 1, y)))
                    left.map[x1 - leftPos.x + 1, y - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x1, y + 1)))
                    left.map[x1 - leftPos.x, y - leftPos.y + 1] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x1 + 1, y + 1)))
                    left.map[x1 - leftPos.x + 1, y - leftPos.y + 1] = ROAD;

                if (right.node.mapRect.Contains(new Vector2Int(x1, y)))
                    right.map[x1 - rightPos.x, y - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x1 + 1, y)))
                    right.map[x1 - rightPos.x + 1, y - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x1, y + 1)))
                    right.map[x1 - rightPos.x, y - rightPos.y + 1] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x1 + 1, y + 1)))
                    right.map[x1 - rightPos.x + 1, y - rightPos.y + 1] = ROAD;
            }
        }
        else if (y1 == y2) // Horizontal line
        {
            int minX = Mathf.Min(x1, x2);
            int maxX = Mathf.Max(x1, x2);
            for (int x = minX; x <= maxX; x++)
            {
                if (left.node.mapRect.Contains(new Vector2Int(x, y1)))
                    left.map[x - leftPos.x, y1 - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x + 1, y1)))
                    left.map[x - leftPos.x + 1, y1 - leftPos.y] = ROAD; 
                if (left.node.mapRect.Contains(new Vector2Int(x, y1 + 1)))
                    left.map[x - leftPos.x, y1 - leftPos.y + 1] = ROAD; 
                if (left.node.mapRect.Contains(new Vector2Int(x + 1, y1 + 1)))
                    left.map[x - leftPos.x + 1, y1 - leftPos.y + 1] = ROAD;

                if (right.node.mapRect.Contains(new Vector2Int(x, y1)))
                    right.map[x - rightPos.x, y1 - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x + 1, y1)))
                    right.map[x - rightPos.x + 1, y1 - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x, y1 + 1)))
                    right.map[x - rightPos.x, y1 - rightPos.y + 1] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x + 1, y1 + 1)))
                    right.map[x - rightPos.x + 1, y1 - rightPos.y + 1] = ROAD;
            }
        }
        else // Right angle
        {
            int minX = Mathf.Min(x1, x2);
            int maxX = Mathf.Max(x1, x2);
            int minY = Mathf.Min(y1, y2);
            int maxY = Mathf.Max(y1, y2);
            for (int x = minX; x <= maxX; x++)
            {
                if (left.node.mapRect.Contains(new Vector2Int(x, y1)))
                    left.map[x - leftPos.x, y1 - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x + 1, y1)))
                    left.map[x - leftPos.x + 1, y1 - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x, y1 + 1)))
                    left.map[x - leftPos.x, y1 - leftPos.y + 1] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x + 1, y1 + 1)))
                    left.map[x - leftPos.x + 1, y1 - leftPos.y + 1] = ROAD;

                if (right.node.mapRect.Contains(new Vector2Int(x, y1)))
                    right.map[x - rightPos.x, y1 - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x + 1, y1)))
                    right.map[x - rightPos.x + 1, y1 - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x, y1 + 1)))
                    right.map[x - rightPos.x, y1 - rightPos.y + 1] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x + 1, y1 + 1)))
                    right.map[x - rightPos.x + 1, y1 - rightPos.y + 1] = ROAD;
            }
            for (int y = minY; y <= maxY; y++)
            {
                if (left.node.mapRect.Contains(new Vector2Int(x2, y)))
                    left.map[x2 - leftPos.x, y - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x2 + 1, y)))
                    left.map[x2 - leftPos.x + 1, y - leftPos.y] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x2, y + 1)))
                    left.map[x2 - leftPos.x, y - leftPos.y + 1] = ROAD;
                if (left.node.mapRect.Contains(new Vector2Int(x2 + 1, y + 1)))
                    left.map[x2 - leftPos.x + 1, y - leftPos.y + 1] = ROAD;


                if (right.node.mapRect.Contains(new Vector2Int(x2, y)))
                    right.map[x2 - rightPos.x, y - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x2 + 1, y)))
                    right.map[x2 - rightPos.x + 1, y - rightPos.y] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x2, y + 1)))
                    right.map[x2 - rightPos.x, y - rightPos.y + 1] = ROAD;
                if (right.node.mapRect.Contains(new Vector2Int(x2 + 1, y + 1)))
                    right.map[x2 - rightPos.x + 1, y - rightPos.y + 1] = ROAD;
            }
        }
    }
}

//���, ������ ���� Block���� ����
public class Block
{
    public BSPNode node;
    public Tilemap frontTilemap;
    public Tilemap backTilemap;
    public Tilemap wallTilemap;
    public int[,] map;

    private TilemapRenderer frontTilemapRenderer;
    private TilemapRenderer backTilemapRenderer;
    private TilemapRenderer wallTilemapRenderer;

    public TilemapRenderer FrontTilemapRenderer
    {
        get
        {
            if (frontTilemapRenderer == null)
                frontTilemapRenderer = frontTilemap.GetComponent<TilemapRenderer>();
            return frontTilemapRenderer;
        }
    }
    public TilemapRenderer BackTilemapRenderer
    {
        get
        {
            if (backTilemapRenderer == null)
                backTilemapRenderer = backTilemap.GetComponent<TilemapRenderer>();
            return backTilemapRenderer;
        }
    }
    public TilemapRenderer WallTilemapRenderer
    {
        get
        {
            if (wallTilemapRenderer == null)
                wallTilemapRenderer = wallTilemap.GetComponent<TilemapRenderer>();
            return wallTilemapRenderer;
        }
    }

    public List<(int, int)> coordinate = null;

    public List<Block> adjacencyBlock = new List<Block>();

    public Block(BSPNode node, Tilemap frontTilemap, Tilemap backTilemap, Tilemap wallTilemap)
    {
        this.node = node;
        this.frontTilemap = frontTilemap;
        this.backTilemap = backTilemap;
        this.wallTilemap = wallTilemap;
    }

    public Vector2 RandomPlainPos()
    {
        if(coordinate == null)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            coordinate = new List<(int, int)>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == 0)
                        coordinate.Add((x, y));
                }
            }
        }

        int index = Random.Range(0, coordinate.Count);
        Vector3Int tilePos = new Vector3Int(coordinate[index].Item1, coordinate[index].Item2);
        Vector3 vec = frontTilemap.CellToWorld(tilePos);

        return vec + new Vector3(0.5f, 0.5f);
    }
}