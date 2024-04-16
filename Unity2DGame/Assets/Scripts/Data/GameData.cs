using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    //게임 데이터로 필요한 건
    //레벨업 데이터
    //몬스터 데이터
    [SerializeField]
    LevelUpData levelUpData;

    [SerializeField]
    MonsterData monsterData;

    public void Init()
    {

    }
}
