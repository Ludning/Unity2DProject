using System.Collections;
using System.Collections.Generic;
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

    

    //클릭이벤트 감지
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

    //장비 해제(인벤토리가 찼을 경우 불가)
    public void OnClickSkillTree()
    {
        GameManager.Instance.UserData.EquipSkill(skillEquipmentType, skillIndex);
    }
    //장비 장착
    public void OnClickEquipmentSkill()
    {
        GameManager.Instance.UserData.UnequipSkill(skillEquipmentType);
    }

    public void ChangeSkill(int skillIndex)
    {
        SkillDataBundle skillDataBundle = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle");
        if (skillIndex == 0)
        {
            image.sprite = skillDataBundle.nullData.skillIcon;
            return;
        }
        this.skillIndex = skillIndex;
        image.sprite = skillDataBundle.GetAllData().Find(x => x.skillId == skillIndex).skillIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.GetElementData(ElementType.InformationOverlay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideElement(ElementType.InformationOverlay);
    }
}
