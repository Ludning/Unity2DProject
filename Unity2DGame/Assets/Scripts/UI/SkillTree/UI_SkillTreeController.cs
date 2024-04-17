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
    private void OnEnable()
    {
        //�κ��丮 �ʱ�ȭ
        foreach (var tree in skillTree)
        {
            foreach(var skill in tree.SkillButton)
            {
                skill.ChangeSkill(GameManager.Instance.UserData.inventoryItem[skill.SkillSlotIndex]);
            }
        }
        //AttackButton �ʱ�ȭ
        foreach (var skill in equipment.AttackButton)
        {
            skill.ChangeSkill(GameManager.Instance.UserData.equipmentItem[skill.SkillSlotIndex]);
        }
        //SkillButton �ʱ�ȭ
        foreach (var skill in equipment.SkillButton)
        {
            skill.ChangeSkill(GameManager.Instance.UserData.equipmentItem[skill.SkillSlotIndex]);
        }
        //SpecialButton �ʱ�ȭ
        equipment.SpecialButton.ChangeSkill(GameManager.Instance.UserData.equipmentItem[equipment.SpecialButton.SkillSlotIndex]);


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

        gameObject.SetActive(false);
        backElement.SetActive(false);
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
