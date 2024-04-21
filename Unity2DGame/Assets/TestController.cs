using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    private void Start()
    {
        //UIManager.Instance.GetPopupElementData(ElementType.InventoryBack, ElementType.InventoryFront);
        UIManager.Instance.ShowElement(ElementType.GameStatic);
        UIManager.Instance.ShowElement(ElementType.SkillPanel);
    }
    private void FixedUpdate()
    {
        Debug.Log(GameManager.Instance.IsGamePaused);
    }
    /*public void OnAttackInput()
    {
        UIManager.Instance.GetPopupElementData(ElementType.InventoryBack, ElementType.InventoryFront);
    }
    public void OnSkillInput()
    {
        UIManager.Instance.GetPopupElementData(ElementType.SkillTreeBack, ElementType.SkillTreeFront);
    }
    public void OnSpecialInput()
    {
        //UIManager.Instance.GetPopupElementData(ElementType.InventoryBack, ElementType.InventoryFront);
    }*/
}
