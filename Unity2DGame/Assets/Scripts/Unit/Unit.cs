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

    public Status status = new Status();



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

        statusBarBar.transform.SetParent(UIManager.Instance.GetCanvasData(CanvasType.SceneInformationCanvas).gameObject.transform);//Canvas.transform;
        UIManager.Instance.ShowUI_Status(statusBarBar);

        statusBarBar.transform.localScale = Vector3.one;
        #endregion
    }
    public virtual void OnDie()
    {
        statusBarBar.SetActive(false);
        ObjectPool.Instance.ReturnToPool(statusBarBar);
        statusBarBar = null;
        uiStatusBar = null;
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
    public virtual void OnDamaged(int value)
    {
        int calValue = value - status.defence;
        if (calValue < 0)
        {
            calValue = 0;
        }

        if (status.hp - value <= 0)
            status.hp = 0;
        else if(status.hp - value >= status.maxHp)
            status.hp = status.maxHp;
        else
            status.hp -= value;

        uiStatusBar.HPBarRefresh(HpRatio);

        if (status.hp <= 0)
            OnDie();
    }
    public virtual float HpRatio
    {
        get
        {
            return (status.maxHp != 0) ? status.hp / (float)status.maxHp : 0f;
        }
    }
    
}
