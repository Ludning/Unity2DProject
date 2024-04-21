using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameStaticController : UI_Controller
{
    public void OnClickSkillButton()
    {
        UIManager.Instance.ShowPopupElement(ElementType.SkillTreeBack, ElementType.SkillTreeFront);
    }
    public void OnClickInventoryButton()
    {
        UIManager.Instance.ShowPopupElement(ElementType.InventoryBack, ElementType.InventoryFront);
    }
    public void OnClickPauseButton()
    {
        UIManager.Instance.ShowPopupElement(ElementType.MenuBack, ElementType.MenuFront);
    }
}
