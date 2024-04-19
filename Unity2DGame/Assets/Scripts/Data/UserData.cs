using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public int attackCursor = 0;
    public float[] equipmentSkillCooltime = new float[3];
    public float equipmentSpecialCooltime = 0;

    public int gold;


    public void UpdateCoolTime(float time)
    {
        for (int i = 0; i < equipmentSkillCooltime.Count(); i++)
        {
            if (!(equipmentSkillCooltime[i] - time <= 0f))
                equipmentSkillCooltime[i] -= time;
            else
                equipmentSkillCooltime[i] = 0f;
        }
        if (!(equipmentSpecialCooltime - time <= 0f))
            equipmentSpecialCooltime -= time;
        else
            equipmentSpecialCooltime = 0f;

    }

    public int GetCurrentAttack
    {
        get
        {
            return equipmentAttack[attackCursor];
        }
    }
    public int GetCurrentSkill
    {
        get
        {
            if (equipmentSkillCooltime[0] == 0 && equipmentSkill[0] != 0)
                return equipmentSkill[0];
            else if (equipmentSkillCooltime[1] == 0 && equipmentSkill[1] != 0)
                return equipmentSkill[1];
            else if (equipmentSkillCooltime[2] == 0 && equipmentSkill[2] != 0)
                return equipmentSkill[2];
            return equipmentSkill[0];
        }
    }
    public int GetCurrentSkillIndex
    {
        get
        {
            if (equipmentSkillCooltime[0] == 0 && equipmentSkill[0] != 0)
                return 0;
            else if (equipmentSkillCooltime[1] == 0 && equipmentSkill[1] != 0)
                return 1;
            else if (equipmentSkillCooltime[2] == 0 && equipmentSkill[2] != 0)
                return 2;
            return 0;
        }
    }
    public float GetCurrentSkillCoolTiem
    {
        get
        {
            return equipmentSkillCooltime[GetCurrentSkillIndex];
        }
    }
    public int GetCurrentSpecial
    {
        get
        {
            return equipmentSpecial;
        }
    }
    public float GetCurrentSpecialCoolTime
    {
        get
        {
            return equipmentSpecialCooltime;
        }
    }
    private void AddAttackCursor()
    {
        attackCursor++;
        if (attackCursor > 2 || equipmentAttack[attackCursor] == 0)
            attackCursor = 0;
    }

    public void UseActiveAttack()
    {
        //사용
        //equipmentSkill[attackCursor];
        AddAttackCursor();
    }
    public void UseActiveSkill()
    {

        equipmentSkillCooltime[GetCurrentSkillIndex] = GetSkillData(GetCurrentSkill).coolTime;
    }
    public void UseActiveSpecial()
    {
        if (equipmentSpecial == 0)
            return;
        equipmentSpecialCooltime = GetSkillData(equipmentSpecial).coolTime;
    }

    public SkillData GetSkillData(int skillId)
    {
        List<SkillData> skilldatas = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle").GetAllData();
        return skilldatas.Find(x => x.skillId == skillId);
    }
    public void SkillCooltime()
    {
        List<SkillData> skilldatas = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle").GetAllData();
    }

    //스킬 장착
    public void SkillEquip(SkillData data, SkillEquipmentType type, int index = 0)
    {
        if (type != SkillEquipmentType.Special && (index < 0 || index > 2))
            return;
        switch (type)
        {
            case SkillEquipmentType.Attack:
                equipmentAttack[index] = data.skillId;
                break;
            case SkillEquipmentType.Skill:
                equipmentSkill[index] = data.skillId;
                break;
            case SkillEquipmentType.Special:
                equipmentSpecial = data.skillId;
                break;
        }
    }
    public void SkillUnequip(SkillData data, SkillEquipmentType type)
    {
        int index = LastEmptySlotIndex(type);

        if (type != SkillEquipmentType.Special && (index < 0 || index > 2))
            return;
        switch (type)
        {
            case SkillEquipmentType.Attack:
                equipmentAttack[index] = 0;
                SortAttack();
                break;
            case SkillEquipmentType.Skill:
                equipmentSkill[index] = 0;
                SortSkill();
                break;
            case SkillEquipmentType.Special:
                equipmentSpecial = 0;
                break;
        }
    }
    public void SortAttack()
    {
        for(int i = 0; i < equipmentAttack.Length; i++)
        {
            if (equipmentAttack[i] == 0 && i + 1 < equipmentAttack.Length)
            {
                int temp = equipmentAttack[i];
                equipmentAttack[i] = equipmentAttack[i + 1];
                equipmentAttack[i + 1] = temp;
            }
        }
    }
    public void SortSkill()
    {
        for (int i = 0; i < equipmentSkill.Length; i++)
        {
            if (equipmentSkill[i] == 0 && i + 1 < equipmentSkill.Length)
            {
                int temp = equipmentSkill[i];
                equipmentSkill[i] = equipmentSkill[i + 1];
                equipmentSkill[i + 1] = temp;

                float tempCool = equipmentSkillCooltime[i];
                equipmentSkillCooltime[i] = equipmentSkillCooltime[i + 1];
                equipmentSkillCooltime[i + 1] = tempCool;
            }
        }
    }
    public int LastEmptySlotIndex(SkillEquipmentType type)
    {
        switch (type)
        {
            case SkillEquipmentType.Attack:
                for (int i = 0; i < 3; i++)
                {
                    if (equipmentAttack[i] == 0 || i == 2)
                        return i;
                }
                break;
            case SkillEquipmentType.Skill:
                for (int i = 0; i < 3; i++)
                {
                    if (equipmentSkill[i] == 0 || i == 2)
                        return i;
                }
                break;
            case SkillEquipmentType.Special:
                return 0;
        }
        return 0;
    }

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
        playerStatus = GameManager.Instance.player.status;
        DataManager.Instance.SaveJsonData("UserData", this);
    }
}
