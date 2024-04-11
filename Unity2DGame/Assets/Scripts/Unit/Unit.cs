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

    GameObject hpBar;
    GameObject hpBarPrefab;
    Slider hpBarSlider;

    Status status = new Status();

    public void Init()
    {
        Debug.Log("OnEnable!");
        #region HPBar 생성
        string name = gameObject.name;
        if(hpBarPrefab == null)
        {
            hpBarPrefab = Addressables.LoadAssetAsync<GameObject>("HPBar_Fire").WaitForCompletion();
        }
        hpBar = ObjectPool.Instance.GetGameObject(hpBarPrefab);
        hpBar.GetComponent<UI_TrackTarget>().Target = gameObject;
        hpBar.transform.parent = UIManager.Instance.Canvas.transform;
        hpBar.transform.localScale = Vector3.one;
        #endregion
        #region hpBar 초기화
        hpBarSlider = hpBar.GetComponent<Slider>();
        HPBarRefresh();
        #endregion
    }
    public void OnDie()
    {
        ObjectPool.Instance.ReturnToPool(hpBar);
        hpBar = null;
        hpBarSlider = null;
    }

    public void HPChange(int value)
    {
        if (status.hp <= value)
            status.hp = 0;
        else
            status.hp -= value;
        HPBarRefresh();
    }
    public float HpRatio
    {
        get
        {
            return (status.maxHp != 0) ? status.hp / (float)status.maxHp : 0f;
        }
    }
    public void HPBarRefresh()
    {
        hpBarSlider.value = HpRatio;
    }
}
