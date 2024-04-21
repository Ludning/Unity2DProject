using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EndScene : MonoBehaviour
{
    public void ToMainScene()
    {
        //������ ����
        GameManager.Instance.UserData.Save();
        ScenesManager.Instance.LoadScene("MainScene");
    }
    public void OnClickExit()
    {
        //������ ����
        GameManager.Instance.UserData.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
