using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameStaticController : UI_Controller
{
    public void OnClickSkillButton()
    {
        UIManager.Instance.GetPopupElementData(ElementType.SkillTreeBack, ElementType.SkillTreeFront);
    }
    public void OnClickInventoryButton()
    {
        UIManager.Instance.GetPopupElementData(ElementType.InventoryBack, ElementType.InventoryFront);
    }
    public void OnClickPauseButton()
    {
        UIManager.Instance.GetPopupElementData(ElementType.MenuBack, ElementType.MenuFront);
    }
}
