using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    public int maxHp;
    [SerializeField]
    public int attack;
    [SerializeField]
    public int defence;
}
