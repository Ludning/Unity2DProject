using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuController : UI_PopupController
{
    private void OnEnable()
    {
        GameManager.Instance.isGamePaused = true;
        //equipment 바꾸는거
        EventBusManager.Instance.Subscribe<ToResumeEvent>(OnClickResume);
        //inventory 바꾸는거
        EventBusManager.Instance.Subscribe<ToMainEvent>(OnClickMain);
        //statusText 바꾸는거
        EventBusManager.Instance.Subscribe<ToExitEvent>(OnClickExit);
    }
    public override void OnDisablePopupElements()
    {
        EventBusManager.Instance.Unsubscribe<ToResumeEvent>(OnClickResume);
        EventBusManager.Instance.Unsubscribe<ToMainEvent>(OnClickMain);
        EventBusManager.Instance.Unsubscribe<ToExitEvent>(OnClickExit);

        gameObject.SetActive(false);
        backElement.SetActive(false);
    }

    public void OnClickResume(ToResumeEvent toResumeEvent)
    {
        GameManager.Instance.isGamePaused = false;
    }
    public void OnClickMain(ToMainEvent toMainEvent)
    {
        GameManager.Instance.isGamePaused = false;
    }
    public void OnClickExit(ToExitEvent toExitEvent)
    {
        GameManager.Instance.isGamePaused = false;
    }
}
public class ToResumeEvent : BaseEvent
{
}
public class ToMainEvent : BaseEvent
{
}
public class ToExitEvent : BaseEvent
{
}
