using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UI_SkillTreeController;
using static UnityEditor.Progress;

[Serializable]
public class UserData
{
    //유저 데이터
    public Status playerStatus = new Status();
    public int[] equipmentItem = new int[3];
    public int[] inventoryItem = new int[10];

    public int[] equipmentAttack = new int[3];
    public int[] equipmentSkill = new int[3];
    public int equipmentSpecial = 0;

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
        statusTextEvent.statusText = GetStatusText();

        //이벤트버스 호출
        EventBusManager.Instance.Publish(equipmentItemEvent);
        EventBusManager.Instance.Publish(inventoryItemEvent);
        EventBusManager.Instance.Publish(statusTextEvent);
    }
    //장비 해제
    public void UnequipItem(int equipmentSlot)
    {
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
        statusTextEvent.statusText = GetStatusText();

        //이벤트버스 호출
        EventBusManager.Instance.Publish(equipmentItemEvent);
        EventBusManager.Instance.Publish(inventoryItemEvent);
        EventBusManager.Instance.Publish(statusTextEvent);
    }


    //스킬 장착(요구사항 체크)
    public void EquipSkill(SkillEquipmentType skillEquipmentType, int skillIndex)
    {
        var slot = LastEmptySkillSlot(skillEquipmentType);
        if (slot.Key == SkillEquipmentType.MaxCount)
            return;

        List<SkillData> skilldatas = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle").GetAllData();

        switch (slot.Key)
        {
            case SkillEquipmentType.Attack:
                equipmentAttack[slot.Value] = skilldatas[skillIndex].skillId;
                break;
            case SkillEquipmentType.Skill:
                equipmentSkill[slot.Value] = skilldatas[skillIndex].skillId;
                break;
            case SkillEquipmentType.Special:
                equipmentSpecial = skilldatas[skillIndex].skillId;
                break;
        }

        //이벤트 객체 재사용
        EquipmentSkillEvent equipmentItemEvent = EventBusManager.Instance.GetEventInstance<EquipmentSkillEvent>();

        //이벤트 객체 내용수정
        Status status = GameManager.Instance.UserData.playerStatus;
        equipmentItemEvent.skillSlotType = slot.Key;
        equipmentItemEvent.skillSlotIndex = slot.Value;

        //이벤트버스 호출
        EventBusManager.Instance.Publish(equipmentItemEvent);
    }
    //스킬 해제
    public void UnequipSkill(SkillEquipmentType skillEquipmentType)
    {
        EquipSkill(skillEquipmentType, 0);
    }


    //마지막 또는 비어있는 스킬칸 반환
    public KeyValuePair<SkillEquipmentType, int> LastEmptySkillSlot(SkillEquipmentType skillEquipmentType)
    {
        switch (skillEquipmentType)
        {
            case SkillEquipmentType.Attack:
                for (int i = 0; i< equipmentAttack.Count(); i++)
                {
                    if (equipmentAttack[i] == 0 || i == (int)SkillEquipmentType.MaxCount - 1)
                        return new KeyValuePair<SkillEquipmentType, int>(SkillEquipmentType.Attack, i);
                }
                break;
            case SkillEquipmentType.Skill:
                for (int i = 0; i < equipmentAttack.Count(); i++)
                {
                    if (equipmentAttack[i] == 0 || i == (int)SkillEquipmentType.MaxCount - 1)
                        return new KeyValuePair<SkillEquipmentType, int>(SkillEquipmentType.Skill, i);
                }
                break;
            case SkillEquipmentType.Special:
                return new KeyValuePair<SkillEquipmentType, int>(SkillEquipmentType.Special, 0);
        }
        return new KeyValuePair<SkillEquipmentType, int>(SkillEquipmentType.MaxCount, 0);
    }
    public string GetStatusText()
    {
        return $"Character Status\nLv : {playerStatus.level}\nHp : {playerStatus.maxHp}\nMp : {playerStatus.maxMp}\nDamage : {playerStatus.attack}\nDefence : {playerStatus.defence}"; ;
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
