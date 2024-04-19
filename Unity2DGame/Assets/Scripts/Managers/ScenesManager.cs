using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Manager<ScenesManager>
{
    public void LoadScene(string sceneName)
    {
        GameManager.Instance.UserData.Save();
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        //�񵿱������� ���� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        //�ε尡 �Ϸ�� ������ ���
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");

            yield return null;
        }
        ObjectPool.Instance.SceneRig = GameObject.Find("SceneInstaller");
    }
}
