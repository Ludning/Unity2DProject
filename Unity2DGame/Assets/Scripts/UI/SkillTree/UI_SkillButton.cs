using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Image image;

    [Space]
    public SkillEquipmentType skillEquipmentType;
    [SerializeField]
    int skillIndex = 0;

    public SkillSlotType skillSlotType;
    public int SkillSlotIndex;

    

    //Ŭ���̺�Ʈ ����
    public void OnClick()
    {
        Debug.Log($"{skillSlotType} is click");
        switch (skillSlotType)
        {
            case SkillSlotType.SkillTree:
                OnClickSkillTree();
                break;
            case SkillSlotType.SkillEquip:
                OnClickEquipmentSkill();
                break;
        }
    }

    //��ų ����
    public void OnClickSkillTree()
    {
        GameManager.Instance.UserData.EquipSkill(skillEquipmentType, SkillSlotIndex, skillIndex);
    }
    //��ų ����
    public void OnClickEquipmentSkill()
    {
        GameManager.Instance.UserData.UnequipSkill(skillEquipmentType, SkillSlotIndex);
    }

    public void ChangeSkill(int skillIndex)
    {
        SkillDataBundle skillDataBundle = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle");
        this.skillIndex = skillIndex;
        image.sprite = skillDataBundle.GetAllData().Find(x => x.skillId == skillIndex).skillIcon;
        skillEquipmentType = skillDataBundle.GetAllData().Find(x => x.skillId == skillIndex).skillEquipmentType;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowSkillOverlayElement(skillIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideSkillOverlayElement();
    }
}
