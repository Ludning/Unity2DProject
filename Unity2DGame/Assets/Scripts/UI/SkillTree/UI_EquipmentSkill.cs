using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_SkillTreeController;

public class UI_EquipmentSkill : MonoBehaviour
{
    [SerializeField]
    List<UI_SkillButton> attackButton;
    [SerializeField]
    List<UI_SkillButton> skillButton;
    [SerializeField]
    UI_SkillButton specialButton;

    public List<UI_SkillButton> AttackButton
    {
        get { return attackButton; }
    }
    public List<UI_SkillButton> SkillButton
    {
        get { return skillButton; }
    }
    public UI_SkillButton SpecialButton
    {
        get { return specialButton; }
    }



    public void SkillSpriteChange(EquipmentSkillEvent equipmentSkill)
    {
        skillButton[equipmentSkill.skillSlotIndex].ChangeSkill(GameManager.Instance.UserData.equipmentSkill[equipmentSkill.skillSlotIndex]);
    }
}
