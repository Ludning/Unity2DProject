using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Status
{
    public int level;
    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int attack;
    public int defence;
    public List<IStatusEffect> statusEffects;

    public Status()
    {
        level = 1;
        maxHp = 1000;
        hp = 800;
        maxMp = 100;
        mp = 100;
        attack = 100;
        defence = 20;
        statusEffects = new List<IStatusEffect>();
    }
    public void RecoveryHp(int value)
    {
        if (hp >= maxHp)
        {
            hp = maxHp;
            return;
        }
        hp += value;
    }
    public void RestoreMp(float time)
    {
        int restoreSpeed = 5;
        if (mp >= maxMp)
        {
            mp = maxMp;
            return;
        }
        mp += (int)(time * restoreSpeed);
    }
}
