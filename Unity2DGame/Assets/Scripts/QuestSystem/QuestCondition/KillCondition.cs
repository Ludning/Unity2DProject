using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillCondition", menuName = "Quest/QuestCondition/KillCondition", order = 0)]
public class KillCondition : QuestCondition
{
    public MonsterType targetType;
    public int KillCount;
    public override bool IsCompleted()
    {
        return true;
    }
}
