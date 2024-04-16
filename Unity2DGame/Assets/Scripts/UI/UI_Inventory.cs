using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField]
    List<UI_ItemButton> itemButton;

    public List<UI_ItemButton> ItemButton
    {
        get { return itemButton; }
    }

    public void ItemSpriteChange(InventoryItemEvent inventoryItem)
    {
        itemButton[inventoryItem.itemSlotIndex].ChangeItem(GameManager.Instance.UserData.inventoryItem[inventoryItem.itemSlotIndex]);
    }
}
