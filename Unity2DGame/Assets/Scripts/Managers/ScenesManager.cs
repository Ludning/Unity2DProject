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
        // 비동기적으로 씬을 로드합니다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 로드가 완료될 때까지 대기합니다.
        while (!asyncLoad.isDone)
        {
            // 여기에서 asyncLoad.progress를 사용하여 로딩 진행 상황을 사용자에게 표시할 수 있습니다.
            // 예를 들어, 로딩 바를 업데이트할 수 있습니다.
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");

            yield return null;
        }

        ObjectPool.Instance.SceneRig = GameObject.Find("SceneInstaller");
    }
}
