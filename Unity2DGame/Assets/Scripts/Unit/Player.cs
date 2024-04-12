using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public int attackIndex = 0;
    public int skillIndex = 0;

    private List<SkillData> attackData = null;
    private List<SkillData> skillData = null;
    private SkillData specialData = null;

    public override void Init(string statusBarName)
    {
        base.Init(statusBarName);
    }

    public List<SkillData> AttackData
    {
        get 
        {
            if (attackData == null)
                attackData = new List<SkillData>();
            return attackData; 
        }
    }
    public List<SkillData> SkillData
    {
        get
        {
            if(skillData == null)
                skillData = new List<SkillData>();
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

    public WeaponController weapon;

    //
    public void SkillEquipment()
    {

    }
}
