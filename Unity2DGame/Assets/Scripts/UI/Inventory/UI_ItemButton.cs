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

    public ItemSlotType ItemSlotType;
    public int ItemSlotIndex;


    //Ŭ���̺�Ʈ ����
    public void OnClick()
    {
        Debug.Log($"{ItemSlotIndex} is click");
        switch (ItemSlotType)
        {
            case ItemSlotType.Equipment:
                OnClickEquipment();
                break;
            case ItemSlotType.Inventory:
                OnClickInventory();
                break;
        }
    }

    //��� ����(�κ��丮�� á�� ��� �Ұ�)
    public void OnClickEquipment()
    {
        //Ŭ���� ��ü�� ������ Ȯ���ϰ�
        GameManager.Instance.UserData.UnequipItem(ItemSlotIndex);
    }
    //��� ����
    public void OnClickInventory()
    {
        //Ŭ���� ��ü�� ������ Ȯ���ϰ�
        GameManager.Instance.UserData.EquipItem(ItemSlotIndex);
    }
    

    public void ChangeItem(int itemIndex)
    {
        if (itemIndex == 0)
        {
            image.sprite = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData").items[0].itemIcon;
            textComponent.enabled = true;
            textComponent.text = "Empty";
        }
        this.itemIndex = itemIndex;
        image.sprite = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData").items.Find(x => x.id == itemIndex).itemIcon;
        textComponent.enabled = false;
    }
    public void EmptyItem()
    {
        itemIndex = 0;
        //�� ��� �̹��� ���
        image.sprite = null;
        textComponent.enabled = true;
        textComponent.text = "Empty";
    }
}
