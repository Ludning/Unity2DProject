using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Equipment : MonoBehaviour
{
    [SerializeField]
    List<UI_ItemButton> itemButton;

    public List<UI_ItemButton> ItemButton
    {
        get { return itemButton; }
    }

    public void ItemSpriteChange(EquipmentItemEvent equipmentItem)
    {
        itemButton[equipmentItem.itemSlotIndex].ChangeItem(GameManager.Instance.UserData.equipmentItem[equipmentItem.itemSlotIndex]);
    }
}
