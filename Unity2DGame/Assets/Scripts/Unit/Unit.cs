using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Unit : MonoBehaviour
{
    //버프, 디버프는 상태패턴으로 갱신

    GameObject hpBar;
    GameObject hpBarPrefab;

    private void OnEnable()
    {
        string name = gameObject.name;
        if(hpBarPrefab == null)
        {
            hpBarPrefab = Addressables.LoadAssetAsync<GameObject>("HPBar_Fire").WaitForCompletion();
        }
        hpBar = ObjectPool.Instance.GetGameObject(hpBarPrefab);
        hpBar.GetComponent<UI_TrackTarget>().Target = gameObject;
        hpBar.transform.parent = UIManager.Instance.Canvas.transform;
        hpBar.transform.localScale = Vector3.one;
    }
    private void OnDisable()
    {
        ObjectPool.Instance.ReturnToPool(hpBar);
    }
}
