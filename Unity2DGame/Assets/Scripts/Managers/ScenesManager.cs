using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Manager<ScenesManager>
{
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        // �񵿱������� ���� �ε��մϴ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // �ε尡 �Ϸ�� ������ ����մϴ�.
        while (!asyncLoad.isDone)
        {
            // ���⿡�� asyncLoad.progress�� ����Ͽ� �ε� ���� ��Ȳ�� ����ڿ��� ǥ���� �� �ֽ��ϴ�.
            // ���� ���, �ε� �ٸ� ������Ʈ�� �� �ֽ��ϴ�.
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");

            yield return null;
        }

        ObjectPool.Instance.SceneRig = GameObject.Find("SceneInstaller");
    }
}
