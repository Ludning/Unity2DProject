using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id;
    public string name;
    public string description;
    public Sprite itemIcon;
    public EquipType equipType;
    public IntrinsicProperties intrinsicProperties;

    public List<StatData> itemStat;
}
[Serializable]
public class StatData
{
    public ItemStatType itemStatType;
    public int value;
}
