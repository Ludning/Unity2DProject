using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainScene : MonoBehaviour
{
    public void OnClickStart()
    {
        //������ ����
        GameManager.Instance.UserData.Save();
        ScenesManager.Instance.LoadScene("GameScene");
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
