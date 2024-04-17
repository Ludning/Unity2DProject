using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UI_SkillTreeController;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField]
    List<UI_SkillButton> skillButton;

    [SerializeField]
    public SkillTreeType skillTreeType;

    public List<UI_SkillButton> SkillButton
    {
        get { return skillButton; }
    }

    public void SkillSpriteChange(SkillTreeEvent skillTreeItem)
    {
        List<SkillData> skilldatas = ResourceManager.Instance.GetScriptableData<SkillDataBundle>("SkillDataBundle").GetAllData();
        skillButton[skillTreeItem.skillSlotIndex].ChangeSkill(skilldatas[skillTreeItem.skillTreeIndex].skillId);
    }
}
