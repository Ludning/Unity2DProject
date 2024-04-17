using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillButton : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    int skillIndex = 0;

    public SkillSlotType skillSlotType;
    public int SkillSlotIndex;

    //클릭이벤트 감지
    public void OnClick()
    {
        Debug.Log($"{skillSlotType} is click");
        switch (skillSlotType)
        {
            case SkillSlotType.SkillEquip:
                OnClickEquipment();
                break;
            case SkillSlotType.SkillTree:
                OnClickInventory();
                break;
        }
    }

    //장비 해제(인벤토리가 찼을 경우 불가)
    public void OnClickEquipment()
    {
        //클릭한 객체가 누군지 확인하고
        //GameManager.Instance.UserData.Unequip(skillSlotType);
    }
    //장비 장착
    public void OnClickInventory()
    {
        //클릭한 객체가 누군지 확인하고
        //GameManager.Instance.UserData.EquipItem(skillSlotType);
    }

    public void ChangeSkill(int skillIndex)
    {

    }
    /*public void ChangeItem(int skillIndex)
    {
        if (skillIndex == 0)
        {
            EmptyItem();
            return;
        }
        this.skillIndex = skillIndex;
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
    }*/
}
