using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_SkillTreeController;

public class UI_EquipmentSkill : MonoBehaviour
{
    [SerializeField]
    List<UI_SkillButton> attackButton;
    [SerializeField]
    UI_SkillButton skillButton;
    [SerializeField]
    UI_SkillButton specialButton;

    public List<UI_SkillButton> AttackButton
    {
        get { return attackButton; }
    }
    public UI_SkillButton SkillButton
    {
        get { return skillButton; }
    }
    public UI_SkillButton SpecialButton
    {
        get { return specialButton; }
    }



    public void SkillSpriteChange(EquipmentSkillEvent equipmentSkill)
    {
        switch(equipmentSkill.skillSlotType)
        {
            case SkillEquipmentType.Attack:
                attackButton[equipmentSkill.skillSlotIndex].ChangeSkill(GameManager.Instance.UserData.equipmentAttack[equipmentSkill.skillSlotIndex]);
                break;
            case SkillEquipmentType.Skill:
                skillButton.ChangeSkill(GameManager.Instance.UserData.equipmentSkill);
                break;
            case SkillEquipmentType.Special:
                specialButton.ChangeSkill(GameManager.Instance.UserData.equipmentSpecial);
                break;
        }
        
    }
}
