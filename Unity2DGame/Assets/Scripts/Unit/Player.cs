using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public int attackIndex = 0;
    public int skillIndex = 0;

    private SkillData[] attackData = null;
    private SkillData[] skillData = null;
    private SkillData specialData = null;
    private WeaponController weapon;

    [SerializeField]
    private SkillSystem skillSystem;
    
    public SkillData[] AttackData
    {
        get 
        {
            if (attackData == null)
            {
                attackData = new SkillData[3];
            }
            return attackData; 
        }
    }
    public SkillData[] SkillData
    {
        get
        {
            if (skillData == null)
            {
                skillData = new SkillData[3];
            }
            return skillData;
        }
    }
    public SkillData SpecialData
    {
        get
        {
            return specialData;
        }
        set
        {
            specialData = value;
        }
    }

    //�ʱ�ȭ
    public override void Init(string statusBarName)
    {
        base.Init(statusBarName);
        #region uiStatusBar �ʱ�ȭ
        uiStatusBar.Init(gameObject.name, HpRatio);
        #endregion
    }

    public WeaponController GetWeaponController()
    {
        return weapon;
    }
    public GameObject GetWeaponObject()
    {
        return weapon.gameObject;
    }
    //���� ����
    public void SetWeaponController(WeaponController weaponController)
    {
        weapon = weaponController;
    }
    //��ų ����
    public void SkillEquip(SkillData data, SkillEquipmentType type, int index = 0)
    {
        /*
        ��ų�� AttackData3��, AttackData3��, SpecialData1���� ������
        ���� Special�� �ƴϸ鼭 �迭���� ����� ����
        */
        if (type != SkillEquipmentType.Special && (index < 0 || index > 2))
            return;
        switch (type)
        {
            case SkillEquipmentType.Attack:
                AttackData[index] = data;
                break;
            case SkillEquipmentType.Skill:
                SkillData[index] = data;
                break;
            case SkillEquipmentType.Special:
                SpecialData = data;
                break;
        }
    }
}