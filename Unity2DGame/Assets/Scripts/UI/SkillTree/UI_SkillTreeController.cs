using MBT;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_SkillTreeController : UI_PopupController
{
    [SerializeField]
    UI_EquipmentSkill equipment;
    [SerializeField]
    List<UI_SkillTree> skillTree;

    //초기화
    public override void OnEnableElements()
    {
        SkillDataBundle skillDataBundle = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle");
        //스킬트리 초기화

        for (int i = 0; i < skillTree.Count; i++)
        {
            for (int k = 0; k < skillTree[i].SkillButton.Count; k++)
            {
                if(skillTree[i].skillTreeType == SkillTreeType.Fire)
                    skillTree[i].SkillButton[k].ChangeSkill(skillDataBundle.fire[k].skillId);
                else if(skillTree[i].skillTreeType == SkillTreeType.Frost)
                    skillTree[i].SkillButton[k].ChangeSkill(skillDataBundle.fire[k].skillId);
                else if (skillTree[i].skillTreeType == SkillTreeType.Tetanus)
                    skillTree[i].SkillButton[k].ChangeSkill(skillDataBundle.fire[k].skillId);
            }
        }

        //AttackButton 초기화
        foreach (var skill in equipment.AttackButton)
        {
            skill.ChangeSkill(GameManager.Instance.UserData.equipmentAttack[skill.SkillSlotIndex]);
        }
        //SkillButton 초기화
        foreach (var skill in equipment.SkillButton)
        {
            skill.ChangeSkill(GameManager.Instance.UserData.equipmentSkill[skill.SkillSlotIndex]);
        }
        //SpecialButton 초기화
        equipment.SpecialButton.ChangeSkill(GameManager.Instance.UserData.equipmentSpecial);


        //equipmentSkill 바꾸는거
        EventBusManager.Instance.Subscribe<EquipmentSkillEvent>(equipment.SkillSpriteChange);
        //skillTree 바꾸는거
        foreach (var tree in skillTree)
        {
            EventBusManager.Instance.Subscribe<SkillTreeEvent>(tree.SkillSpriteChange);
        }
    }
    //이벤트 해제, 비활성화
    public override void OnDisablePopupElements()
    {
        EventBusManager.Instance.Unsubscribe<EquipmentSkillEvent>(equipment.SkillSpriteChange);
        foreach (var tree in skillTree)
        {
            EventBusManager.Instance.Unsubscribe<SkillTreeEvent>(tree.SkillSpriteChange);
        }

        UIManager.Instance.HidePopupElement(ElementType.SkillTreeBack, ElementType.SkillTreeFront);
    }
    public class EquipmentSkillEvent : BaseEvent
    {
        public SkillEquipmentType skillSlotType;
        public int skillSlotIndex;
    }
    public class SkillTreeEvent : BaseEvent
    {
        public int skillTreeIndex;
        public int skillSlotIndex;
    }
}
