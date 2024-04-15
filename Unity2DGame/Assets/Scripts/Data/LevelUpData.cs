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
    public int level = 0;
    public int Exp = 0;
}
