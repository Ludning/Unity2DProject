using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuController : UI_PopupController
{
    public override void OnDisablePopupElements()
    {
        UIManager.Instance.HidePopupElement(ElementType.MenuBack, ElementType.MenuFront);
    }

    public void OnClickResume()
    {
        OnDisablePopupElements();
    }
    public void OnClickMain()
    {
        //저장후 메인
        GameManager.Instance.UserData.Save();
        ScenesManager.Instance.LoadScene("MainScene");
    }
    public void OnClickExit()
    {
        //저장후 종료
        GameManager.Instance.UserData.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
