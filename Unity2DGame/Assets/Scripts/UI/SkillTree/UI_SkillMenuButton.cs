using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SkillMenuButton : MonoBehaviour
{
    [SerializeField]
    int rectIndex;
    [SerializeField]
    UI_SkillRectPanels skillRectPanels;
    public void ActiveSkillTreeRect()
    {
        skillRectPanels.OnEnableSkillTree(rectIndex);
    }
}
