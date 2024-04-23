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
    [SerializeField] private Tile upperLeftCorner;  //좌상
    [SerializeField] private Tile upperRightCorner; //우상
    [SerializeField] private Tile bottomLeftCorner; //좌하
    [SerializeField] private Tile bottomRightCorner; //우하
    [SerializeField] private Tile topCorner;        //위
    [SerializeField] private Tile bottomCorner;     //아래
    [SerializeField] private Tile leftCorner;       //왼
    [SerializeField] private Tile rightCorner;      //오

    [Space]
    [SerializeField] private Tile upperLeftEdge;  //좌상
    [SerializeField] private Tile upperRightEdge; //우상
    [SerializeField] private Tile bottomLeftEdge; //좌하
    [SerializeField] private Tile bottomRightEdge; //우하


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
            //타일맵 오브젝트 생성
            GameObject front = new GameObject("PlainTilemap");
            //front.isStatic = true;
            TilemapRenderer frontRenderer = front.AddComponent<TilemapRenderer>();
            frontRenderer.sortingLayerName = "Background";
            front.transform.SetParent(frontParent.transform, false);
            front.transform.localPosition = new Vector2(node.mapRect.position.x, node.mapRect.position.y);
            //네비메쉬
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
            //네비메쉬
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
            //시드로 부터 의사 난수 생성
            if (useRandomSeed) seed = Time.time.ToString();
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            block.map = new int[block.node.mapRect.width, block.node.mapRect.height];
            NodeRandomFill(block, pseudoRandom);

            for (int i = 0; i < smoothNum; i++) //반복이 많을수록 동굴의 경계면이 매끄러워진다.
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
        // 첫 번째 타일맵의 모든 셀을 순회
        BoundsInt bounds = block.frontTilemap.cellBounds;
        
        for (int x = 0; x < block.map.GetLength(0); x++)
        {
            for (int y = 0; y < block.map.GetLength(1); y++)
            {
                Tile tile = (block.map[x,y] == EMPTY) ? backgroungTile : null;

                // 첫 번째 타일맵에 타일이 없는 경우 두 번째 타일맵에 타일을 채웁니다.
                if (tile == backgroungTile)
                {
                    Vector3Int pos = new Vector3Int(x, y);
                    block.backTilemap.SetTile(pos, tile);
                }
            }
        }
    }

    //맵을 비율에 따라 벽 혹은 빈 공간으로 랜덤하게 채움
    private void NodeRandomFill(Block block, System.Random pseudoRandom)
    {
        //연결 맵 데이터를 가져오고
        //맵을 그릴 때 연결까지 Route를 그린다
        //그릴 때 Route를 연결노드 방향의 자신노드의 끝까지 그림
        //var data = BSP.map.GetNearNodes();

        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                //가장자리는 빈 공간 채움
                if (x == 0 || x == block.node.mapRect.width - 1 || y == 0 || y == block.node.mapRect.height - 1)
                    block.map[x, y] = EMPTY;
                //비율에 따라 빈 공간 혹은 바닥 생성
                else
                    block.map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? EMPTY : PLAIN;
            }
        }
    }
    //노드 채우기
    private void OnDrawNode(Block block)
    {
        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                OnDrawTile(block, x, y); //타일 생성
            }
        }
    }
    //타일 그리기
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
    //테두리 부드럽게
    private void SmoothMap(Block block)
    {
        for (int x = 0; x < block.node.mapRect.width; x++)
        {
            for (int y = 0; y < block.node.mapRect.height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(block, x, y);
                if (neighbourWallTiles > 4) block.map[x, y] = EMPTY; //주변 칸 중 벽이 4칸을 초과할 경우 현재 타일을 빈 공간으로 바꿈
                else if (neighbourWallTiles < 4) block.map[x, y] = PLAIN; //주변 칸 중 벽이 4칸 미만일 경우 현재 타일을 바닥으로 바꿈
                //SetTileColor(node, x, y); //타일 색상 변경
            }
        }
    }
    //벽 생성
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
                //주변 칸 중 벽이 1칸을 초과할 경우 벽타일 그리기
                if (neighbourWallTiles >= 1 && (block.map[x, y] == PLAIN || block.map[x, y] == ROAD))
                {
                    block.wallTilemap.SetTile(pos, ProcessWallPattern(wallPattern));
                }
                if(edgePattern != 0)
                    block.wallTilemap.SetTile(pos, ProcessEdgePattern(edgePattern));
            }
        }
    }

    #region 패턴 계산
    //맵 테두리
    private int GetWallPattern(Block block, int gridX, int gridY)
    {
        int pattern = 0;

        //경계선에 있으면서 자신은 ROAD일 떄
        //위
        if (gridY == block.node.mapRect.height - 1 && block.map[gridX, gridY] == ROAD)
        {
            //왼
            if (gridX - 1 < 0 || block.map[gridX - 1, gridY] == EMPTY)
                return pattern |= 1 << 0;
            //오
            if (gridX + 1 >= block.node.mapRect.width || block.map[gridX + 1, gridY] == EMPTY)
                return pattern |= 1 << 2;
            
        }
        //오
        if (gridX == block.node.mapRect.width - 1 && block.map[gridX, gridY] == ROAD)
        {
            //위
            if (gridY + 1 >= block.node.mapRect.height || block.map[gridX, gridY + 1] == EMPTY)
                return pattern |= 1 << 3;
            //아래
            if (gridY - 1 < 0 || block.map[gridX, gridY - 1] == EMPTY)
                return pattern |= 1 << 1;
        }
        //아래
        if (gridY == 0 && block.map[gridX, gridY] == ROAD)
        {
            //왼
            if (gridX - 1 < 0 || block.map[gridX - 1, gridY] == EMPTY)
                return pattern |= 1 << 0;
            //오
            if (gridX + 1 >= block.node.mapRect.width || block.map[gridX + 1, gridY] == EMPTY)
                return pattern |= 1 << 2;
        }
        //왼
        if (gridX == 0 && block.map[gridX, gridY] == ROAD)
        {
            //위
            if (gridY + 1 >= block.node.mapRect.height || block.map[gridX, gridY + 1] == EMPTY)
                return pattern |= 1 << 3;
            //아래
            if (gridY - 1 < 0 || block.map[gridX, gridY - 1] == EMPTY)
                return pattern |= 1 << 1;
        }



        if (gridY + 1 >= block.node.mapRect.height) pattern |= 1 << 3; //위
        else if (gridY + 1 < block.node.mapRect.height && block.map[gridX, gridY + 1] == EMPTY) pattern |= 1 << 3; //위

        if (gridX + 1 >= block.node.mapRect.width) pattern |= 1 << 2; //오
        else if (gridX + 1 < block.node.mapRect.width && block.map[gridX + 1, gridY] == EMPTY) pattern |= 1 << 2; //오

        if (gridY - 1 < 0) pattern |= 1 << 1; //아래
        else if (gridY - 1 >= 0 && block.map[gridX, gridY - 1] == EMPTY) pattern |= 1 << 1; //아래

        if (gridX - 1 < 0) pattern |= 1 << 0; //왼
        else if (gridX - 1 >= 0 && block.map[gridX - 1, gridY] == EMPTY) pattern |= 1 << 0; //왼

        return pattern;
    }
    //맵 모서리
    private int GetEdgePattern(Block block, int gridX, int gridY)
    {
        int pattern = 0;

        if(GetWallPattern(block, gridX, gridY) == 0b0000)
        {
            if ((gridX + 1 < 0 && gridY + 1 >= block.node.mapRect.height)) return 1 << 3;//좌상
            else if ((gridX - 1 >= 0 && gridY + 1 < block.node.mapRect.height) && block.map[gridX - 1, gridY + 1] == EMPTY) return 1 << 3; //좌상

            if ((gridX + 1 >= block.node.mapRect.width && gridY + 1 >= block.node.mapRect.height)) return 1 << 2; //우상
            else if ((gridX + 1 < block.node.mapRect.width && gridY + 1 < block.node.mapRect.height) && block.map[gridX + 1, gridY + 1] == EMPTY) return 1 << 2; //우상

            if ((gridX + 1 >= block.node.mapRect.width && gridY - 1 < 0)) return 1 << 0; //우하
            else if ((gridX + 1 < block.node.mapRect.width && gridY - 1 >= 0) && block.map[gridX + 1, gridY - 1] == EMPTY) return 1 << 1; //우하

            if ((gridX + 1 < 0 && gridY + 1 < 0)) return 1 << 1; //좌하
            else if ((gridX - 1 >= 0 && gridY - 1 >= 0) && block.map[gridX - 1, gridY - 1] == EMPTY) return 1 << 0; //좌하
        }

        return pattern;
    }
    //맵 평준화
    private int GetSurroundingWallCount(Block block, int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //현재 좌표를 기준으로 주변 8칸 검사
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < block.node.mapRect.width && neighbourY >= 0 && neighbourY < block.node.mapRect.height)
                { 
                    //맵 범위를 초과하지 않게 조건문으로 검사
                    if (neighbourX != gridX || neighbourY != gridY) wallCount += block.map[neighbourX, neighbourY]; //벽은 1이고 빈 공간은 0이므로 벽일 경우 wallCount 증가
                }
                else wallCount++; //주변 타일이 맵 범위를 벗어날 경우 wallCount 증가
            }
        }
        return wallCount;
    }
    #endregion

    #region 패턴 처리
    private Tile ProcessWallPattern(int pattern)
    {
        //벽의 경우
        /*
        
            1
          4 ㅁ 2
            3
        좌상단 모서리
        우상단 모서리
        
        좌하단 모서리
        우하단 모서리
        
        상단 모서리
        하단 모서리
        좌단 모서리
        우단 모서리
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
        //벽의 경우
        /*
        
          1   2
            ㅁ 
          3   4

        좌상단 모서리
        우상단 모서리
        우하단 모서리
        좌하단 모서리
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

//배경, 지형을 묶어 Block으로 보관
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