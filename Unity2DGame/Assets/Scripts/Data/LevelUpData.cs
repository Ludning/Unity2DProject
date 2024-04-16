using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpData", menuName = "Data/LevelUpData", order = 0)]
public class LevelUpData : ScriptableObject
{
    public List<LevelData> levelDatas;
}
[Serializable]
public class LevelData
{
    public int level;
    public int Exp;

    public int maxHp;
    public int attack;
    public int defence;
}
