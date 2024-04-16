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
        //초기화
        //인벤토리 초기화
        foreach (var item in inventory.ItemButton)
        {
            item.ChangeItem(GameManager.Instance.UserData.inventoryItem[item.ItemSlotIndex]);
        }
        //장착칸 초기화
        foreach (var item in equipment.ItemButton)
        {
            item.ChangeItem(GameManager.Instance.UserData.equipmentItem[item.ItemSlotIndex]);
        }
        //텍스트 초기화
        statusText.TextUI.text = GameManager.Instance.UserData.GetStatusText();

        //equipment 바꾸는거
        EventBusManager.Instance.Subscribe<EquipmentItemEvent>(equipment.ItemSpriteChange);
        //inventory 바꾸는거
        EventBusManager.Instance.Subscribe<InventoryItemEvent>(inventory.ItemSpriteChange);
        //statusText 바꾸는거
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
