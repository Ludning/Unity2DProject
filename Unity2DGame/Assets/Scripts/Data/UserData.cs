using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class UserData
{
    //유저 데이터
    public Status playerStatus = new Status();
    public int[] equipmentItem = new int[3];
    public int[] inventoryItem = new int[10];

    public int gold;

    //장비, 인벤토리와 스왑
    public void EquipItem(int inventorySlot)
    {
        if (inventoryItem[inventorySlot] == 0)
            return;
        ItemData itemData = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData");
        int equipSlot = (int)itemData.items.Find(x => x.id == inventoryItem[inventorySlot]).equipType;
        
        int temp = equipmentItem[equipSlot];
        equipmentItem[equipSlot] = inventoryItem[inventorySlot];
        inventoryItem[inventorySlot] = temp;

        //이벤트 객체 재사용
        EquipmentItemEvent equipmentItemEvent = EventBusManager.Instance.GetEventInstance<EquipmentItemEvent>();
        InventoryItemEvent inventoryItemEvent = EventBusManager.Instance.GetEventInstance<InventoryItemEvent>();
        StatusTextEvent statusTextEvent = EventBusManager.Instance.GetEventInstance<StatusTextEvent>();

        //이벤트 객체 내용수정
        Status status = GameManager.Instance.UserData.playerStatus;
        equipmentItemEvent.itemSlotIndex = equipSlot;
        inventoryItemEvent.itemSlotIndex = inventorySlot;
        statusTextEvent.statusText = $"Character Status/n" +
                        $"Lv : {status.level}" +
                        $"Hp : {status.maxHp}" +
                        $"Mp : {status.maxMp}" +
                        $"Damage : {status.attack}" +
                        $"Defence : {status.defence}"; ;

        //이벤트버스 호출
        EventBusManager.Instance.Publish(equipmentItemEvent);
        EventBusManager.Instance.Publish(inventoryItemEvent);
        EventBusManager.Instance.Publish(statusTextEvent);
    }
    //장비 해제
    public void Unequip(int equipmentSlot)
    {
        /*if (IsInventoryFull())
            return;*/

        int inventorySlot = int.MaxValue;
        for (int i = 0; i < inventoryItem.Count(); i++)
        {
            if(inventoryItem[i] == 0)
            {
                inventorySlot = i;
                break;
            }
        }
        if (inventorySlot == int.MaxValue)
        {
            return;
            /*inventorySlot = inventoryItem.Count;
            inventoryItem.Add(equipmentItem[equipmentSlot]);*/
        }
        inventoryItem[inventorySlot] = equipmentItem[equipmentSlot];
        equipmentItem[equipmentSlot] = 0;

        //이벤트 객체 재사용
        EquipmentItemEvent equipmentItemEvent = EventBusManager.Instance.GetEventInstance<EquipmentItemEvent>();
        InventoryItemEvent inventoryItemEvent = EventBusManager.Instance.GetEventInstance<InventoryItemEvent>();
        StatusTextEvent statusTextEvent = EventBusManager.Instance.GetEventInstance<StatusTextEvent>();

        //이벤트 객체 내용수정
        Status status = GameManager.Instance.UserData.playerStatus;
        equipmentItemEvent.itemSlotIndex = equipmentSlot;
        inventoryItemEvent.itemSlotIndex = inventorySlot;
        statusTextEvent.statusText = $"Character Status/n" +
                        $"Lv : {status.level}/n" +
                        $"Hp : {status.maxHp}/n" +
                        $"Mp : {status.maxMp}/n" +
                        $"Damage : {status.attack}/n" +
                        $"Defence : {status.defence}"; ;

        //이벤트버스 호출
        EventBusManager.Instance.Publish(equipmentItemEvent);
        EventBusManager.Instance.Publish(inventoryItemEvent);
        EventBusManager.Instance.Publish(statusTextEvent);
    }
    public string GetStatusText()
    {
        return $"Character Status/nLv : {playerStatus.level}/nHp : {playerStatus.maxHp}/nMp : {playerStatus.maxMp}/nDamage : {playerStatus.attack}/nDefence : {playerStatus.defence}"; ;
    }
    public bool IsInventoryFull()
    {
        if (inventoryItem.Where(x => x > 0).ToList().Count >= 10)
            return true;
        else
            return false;
    }
    public void Save()
    {
        DataManager.Instance.SaveJsonData("UserData", this);
    }
}
