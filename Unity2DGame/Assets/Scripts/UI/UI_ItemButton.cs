using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemButton : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    TextMeshProUGUI textComponent;

    [SerializeField]
    int itemIndex = 0;

    public SlotType ItemSlotType;
    public int ItemSlotIndex;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button clicked!");
    }
    //클릭이벤트 감지
    public void OnClick()
    {
        Debug.Log($"{ItemSlotIndex} is click");
        switch (ItemSlotType)
        {
            case SlotType.Equipment:
                OnClickEquipment();
                break;
            case SlotType.Inventory:
                OnClickInventory();
                break;
        }
    }

    //장비 해제(인벤토리가 찼을 경우 불가)
    public void OnClickEquipment()
    {
        //클릭한 객체가 누군지 확인하고
        GameManager.Instance.UserData.Unequip(ItemSlotIndex);
    }
    //장비 장착
    public void OnClickInventory()
    {
        //클릭한 객체가 누군지 확인하고
        GameManager.Instance.UserData.EquipItem(ItemSlotIndex);
    }
    

    public void ChangeItem(int itemIndex)
    {
        if (itemIndex == 0)
        {
            EmptyItem();
            return;
        }
        this.itemIndex = itemIndex;
        image.sprite = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData").items.Find(x => x.id == itemIndex).itemIcon;
        textComponent.enabled = false;
    }
    public void EmptyItem()
    {
        itemIndex = 0;
        //빈 배경 이미지 출력
        image.sprite = null;
        textComponent.enabled = true;
        textComponent.text = "Empty";
    }
}
