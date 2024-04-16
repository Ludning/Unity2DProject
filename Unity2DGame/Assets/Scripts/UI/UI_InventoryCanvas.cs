using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryCanvas : UI_Canvas
{
    [SerializeField]
    UI_Equipment equipment;
    [SerializeField]
    UI_Inventory inventory;
    [SerializeField]
    UI_StatusText statusText;

    public GameObject frontCanvas;
    public GameObject backCanvas;

    private void OnEnable()
    {
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
    public void OnDisableCanvas()
    {
        EventBusManager.Instance.Unsubscribe<EquipmentItemEvent>(equipment.ItemSpriteChange);
        EventBusManager.Instance.Unsubscribe<InventoryItemEvent>(inventory.ItemSpriteChange);
        EventBusManager.Instance.Unsubscribe<StatusTextEvent>(statusText.StatusTextChange);

        frontCanvas.SetActive(false);
        backCanvas.SetActive(false);
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
