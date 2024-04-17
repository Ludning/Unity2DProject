using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_SkillTreeController;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField]
    List<UI_SkillButton> skillButton;

    public List<UI_SkillButton> SkillButton
    {
        get { return skillButton; }
    }

    public void SkillSpriteChange(SkillTreeEvent skillTreeItem)
    {
        skillButton[skillTreeItem.skillSlotIndex].ChangeSkill(ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle").data[skillTreeItem.skillTreeIndex].skillId);
    }
}
