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
        //비동기적으로 씬을 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        //로드가 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");

            yield return null;
        }
        ObjectPool.Instance.SceneRig = GameObject.Find("SceneInstaller");
    }
}
