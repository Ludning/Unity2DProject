using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    //버프, 디버프는 상태패턴으로 갱신

    protected GameObject statusBarBar;
    protected GameObject statusBarPrefab;
    protected UIStatusBar uiStatusBar;

    Status status = new Status();

    public virtual void Init(string statusBarName)
    {
        #region HPBar 생성
        string name = gameObject.name;
        if(statusBarPrefab == null)
        {
            statusBarPrefab = Addressables.LoadAssetAsync<GameObject>(statusBarName).WaitForCompletion();
        }
        statusBarBar = ObjectPool.Instance.GetGameObject(statusBarPrefab);
        statusBarBar.GetComponent<UI_TrackTarget>().Target = gameObject;
        uiStatusBar = statusBarBar.GetComponent<UIStatusBar>();
        statusBarBar.transform.parent = UIManager.Instance.Canvas.transform;
        statusBarBar.transform.localScale = Vector3.one;
        #endregion
    }
    public void OnDie()
    {
        ObjectPool.Instance.ReturnToPool(statusBarBar);
        statusBarBar = null;
        uiStatusBar = null;
    }
    public void HPChange(int value)
    {
        if (status.hp <= value)
            status.hp = 0;
        else
            status.hp -= value;
        uiStatusBar.HPBarRefresh(HpRatio);
    }
    public float HpRatio
    {
        get
        {
            return (status.maxHp != 0) ? status.hp / (float)status.maxHp : 0f;
        }
    }
    
}
