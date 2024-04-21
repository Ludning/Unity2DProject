using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemFinder : MonoBehaviour
{
    List<DropItem> dropItemList = new List<DropItem>();
    DropItem nearestItem = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Prop"))
            dropItemList.Add(collision.GetComponent<DropItem>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Prop"))
        {
            DropItem di = collision.GetComponent<DropItem>();
            //di.SetUnhighlight();
            dropItemList.Remove(di);
        }
    }
    private void FixedUpdate()
    {
        //dropItemList.ForEach(x => x.SetUnhighlight());
        //GetNearestItem()?.SetHighlight();
    }
    DropItem GetNearestItem()
    {
        float minDistance = Mathf.Infinity;

        foreach (DropItem item in dropItemList)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestItem = item;
            }
        }

        return nearestItem; // 가장 가까운 아이템 반환
    }
}
