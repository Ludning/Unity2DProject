using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public int maxHp;
    public int hp;
    public int attack;
    public int defence;
    public List<StatusEffect> statusEffects;

    public Status()
    {
        maxHp = 100;
        hp = 10;
        attack = 10;
        defence = 10;
        statusEffects = new List<StatusEffect>();
    }
}
