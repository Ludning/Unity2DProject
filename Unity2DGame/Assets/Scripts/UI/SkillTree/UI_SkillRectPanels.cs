using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillRectPanels : MonoBehaviour
{
    [SerializeField]
    List<UI_SkillTree> skillTreeRects;

    public List<UI_SkillTree> SkillTreeRects
    {
        get { return skillTreeRects; }
    }

    public void OnEnableSkillTree(int index)
    {
        skillTreeRects.ForEach(rect => rect.gameObject.SetActive(false));
        skillTreeRects[index].gameObject.SetActive(true);
    }
}
