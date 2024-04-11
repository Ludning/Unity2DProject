using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstaller : MonoBehaviour
{
    private void OnDestroy()
    {
        ObjectPool.Instance.SceneRig = null;
    }
}
