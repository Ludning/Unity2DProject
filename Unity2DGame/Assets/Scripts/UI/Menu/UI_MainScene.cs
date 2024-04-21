using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainScene : MonoBehaviour
{
    public void OnClickStart()
    {
        //저장후 메인
        GameManager.Instance.UserData.Save();
        ScenesManager.Instance.LoadScene("GameScene");
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
