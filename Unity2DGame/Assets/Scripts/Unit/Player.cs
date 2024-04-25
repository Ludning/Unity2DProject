using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public int attackIndex = 0;
    public int skillIndex = 0;

    [SerializeField]
    private PlayerController playerController;
    private WeaponController weapon;

    //초기화
    public override void Init(string statusBarName)
    {
        base.Init(statusBarName);
        #region uiStatusBar 초기화
        uiStatusBar.Init(gameObject.name, HpRatio);
        #endregion
        GameManager.Instance.player = this;
        GameManager.Instance.playerController = playerController;
    }
    public override void OnDamaged(int value)
    {
        UserData userData = GameManager.Instance.UserData;
        int calValue = value - userData.playerStatus.defence;
        if (calValue <= 0)
        {
            return;
        }

        Debug.Log(userData.playerStatus.hp);

        if (userData.playerStatus.hp - calValue <= 0)
            userData.playerStatus.hp = 0;
        else if (userData.playerStatus.hp - calValue >= userData.playerStatus.maxHp)
            userData.playerStatus.hp = userData.playerStatus.maxHp;
        else
            userData.playerStatus.hp -= calValue;

        if(uiStatusBar != null)
            uiStatusBar.HPBarRefresh(HpRatio);

        Debug.Log(HpRatio);

        if (userData.playerStatus.hp <= 0)
            OnDie();
    }
    public void OnHeal(int value)
    {
        if (value < 0)
            return;
        GameManager.Instance.UserData.playerStatus.RecoveryHp(value);

        uiStatusBar.HPBarRefresh(HpRatio);
    }
    public PlayerController GetPlayerController()
    {
        return playerController;
    }
    public WeaponController GetWeaponController()
    {
        return weapon;
    }
    public GameObject GetWeaponObject()
    {
        return weapon.gameObject;
    }
    //무기 장착
    public void SetWeaponController(WeaponController weaponController)
    {
        weapon = weaponController;
    }
    public override float HpRatio
    {
        get
        {
            UserData userData = GameManager.Instance.UserData;
            return (userData.playerStatus.maxHp != 0) ? userData.playerStatus.hp / (float)userData.playerStatus.maxHp : 0f;
        }
    }
    public override void OnDie()
    {
        if(statusBarBar != null)
        {
            statusBarBar.SetActive(false);
            ObjectPool.Instance.ReturnToPool(statusBarBar);
            statusBarBar = null;
            uiStatusBar = null;
        }
        
        ObjectPool.Instance.ReturnToPool(gameObject);


        ScenesManager.Instance.LoadScene("EndScene");
    }
}
