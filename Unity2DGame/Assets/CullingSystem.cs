using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CullingSystem : MonoBehaviour
{
    public void OnStartCullingMap(Dictionary<BSPNode, Block> blockDic)
    {
        StartCoroutine(CullingMap(blockDic));
    }
    IEnumerator CullingMap(Dictionary<BSPNode, Block> blockDic)
    {
        while(true)
        {
            Block block = null;
            foreach (var data in blockDic)
            {
                if (CheakOnBlock(data.Value))
                {
                    block = data.Value;
                    break;
                }
            }
            if (block != null)
            {
                foreach (var data in blockDic)
                {
                    if (block.adjacencyBlock.Contains(data.Value) || block == data.Value)
                    {
                        data.Value.FrontTilemapRenderer.enabled = true;
                        data.Value.WallTilemapRenderer.enabled = true;
                        data.Value.BackTilemapRenderer.enabled = true;
                    }
                    else
                    {
                        data.Value.FrontTilemapRenderer.enabled = false;
                        data.Value.WallTilemapRenderer.enabled = false;
                        data.Value.BackTilemapRenderer.enabled = false;
                    }
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
    private bool CheakOnBlock(Block block)
    {
        Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        return block.node.mapRect.Contains(pos);
    }
}
