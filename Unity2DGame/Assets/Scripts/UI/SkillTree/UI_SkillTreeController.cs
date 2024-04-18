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

    //�ʱ�ȭ
    public override void OnEnableElements()
    {
        SkillDataBundle skillDataBundle = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle");
        //��ųƮ�� �ʱ�ȭ

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

        //AttackButton �ʱ�ȭ
        foreach (var skill in equipment.AttackButton)
        {
            skill.ChangeSkill(GameManager.Instance.UserData.equipmentAttack[skill.SkillSlotIndex]);
        }
        //SkillButton �ʱ�ȭ
        foreach (var skill in equipment.SkillButton)
        {
            skill.ChangeSkill(GameManager.Instance.UserData.equipmentSkill[skill.SkillSlotIndex]);
        }
        //SpecialButton �ʱ�ȭ
        equipment.SpecialButton.ChangeSkill(GameManager.Instance.UserData.equipmentSpecial);


        //equipmentSkill �ٲٴ°�
        EventBusManager.Instance.Subscribe<EquipmentSkillEvent>(equipment.SkillSpriteChange);
        //skillTree �ٲٴ°�
        foreach (var tree in skillTree)
        {
            EventBusManager.Instance.Subscribe<SkillTreeEvent>(tree.SkillSpriteChange);
        }
    }
    //�̺�Ʈ ����, ��Ȱ��ȭ
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
