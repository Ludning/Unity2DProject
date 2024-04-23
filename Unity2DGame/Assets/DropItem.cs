using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item item;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    public void Init(Item item)
    {
        this.item = item;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.UserData.PickUpItem(item.id);
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
