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
        maxHp = 100;
        hp = 100;
        maxMp = 100;
        mp = 100;
        attack = 100;
        defence = 100;
        statusEffects = new List<IStatusEffect>();
    }
}
