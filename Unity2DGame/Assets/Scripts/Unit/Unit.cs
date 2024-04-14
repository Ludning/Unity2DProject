using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Unit : InteractiveObject
{
    //����, ������� ������������ ����

    protected GameObject statusBarBar;
    protected GameObject statusBarPrefab;
    protected UIStatusBar uiStatusBar;

    Status status = new Status();

    //���� �ʱ�ȭ, ü�¹� ȣ��
    public virtual void Init(string statusBarName)
    {
        #region HPBar ����
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
    public virtual void OnDie()
    {
        ObjectPool.Instance.ReturnToPool(statusBarBar);
        statusBarBar = null;
        uiStatusBar = null;
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
    public void OnDamaged(int value)
    {
        if (status.hp <= value)
            status.hp = 0;
        else
            status.hp -= value;

        uiStatusBar.HPBarRefresh(HpRatio);

        if (status.hp <= 0)
            OnDie();
    }
    public float HpRatio
    {
        get
        {
            return (status.maxHp != 0) ? status.hp / (float)status.maxHp : 0f;
        }
    }
    
}
