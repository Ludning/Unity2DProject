using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryController : UI_PopupController
{
    [SerializeField]
    UI_Equipment equipment;
    [SerializeField]
    UI_Inventory inventory;
    [SerializeField]
    UI_StatusText statusText;

    private void OnEnable()
    {
        GameManager.Instance.isGamePaused = true;
        //�ʱ�ȭ
        //�κ��丮 �ʱ�ȭ
        foreach (var item in inventory.ItemButton)
        {
            item.ChangeItem(GameManager.Instance.UserData.inventoryItem[item.ItemSlotIndex]);
        }
        //����ĭ �ʱ�ȭ
        foreach (var item in equipment.ItemButton)
        {
            item.ChangeItem(GameManager.Instance.UserData.equipmentItem[item.ItemSlotIndex]);
        }
        //�ؽ�Ʈ �ʱ�ȭ
        statusText.TextUI.text = GameManager.Instance.UserData.GetStatusText();

        //equipment �ٲٴ°�
        EventBusManager.Instance.Subscribe<EquipmentItemEvent>(equipment.ItemSpriteChange);
        //inventory �ٲٴ°�
        EventBusManager.Instance.Subscribe<InventoryItemEvent>(inventory.ItemSpriteChange);
        //statusText �ٲٴ°�
        EventBusManager.Instance.Subscribe<StatusTextEvent>(statusText.StatusTextChange);
    }
    public override void OnDisablePopupElements()
    {
        EventBusManager.Instance.Unsubscribe<EquipmentItemEvent>(equipment.ItemSpriteChange);
        EventBusManager.Instance.Unsubscribe<InventoryItemEvent>(inventory.ItemSpriteChange);
        EventBusManager.Instance.Unsubscribe<StatusTextEvent>(statusText.StatusTextChange);

        gameObject.SetActive(false);
        backElement.SetActive(false);
    }
}

public class EquipmentItemEvent : BaseEvent
{
    public int itemSlotIndex;
}
public class InventoryItemEvent : BaseEvent
{
    public int itemSlotIndex;
}
public class StatusTextEvent : BaseEvent
{
    public string statusText;
}
