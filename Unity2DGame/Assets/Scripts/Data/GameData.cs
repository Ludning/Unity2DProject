using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    //���� �����ͷ� �ʿ��� ��
    //������ ������
    //���� ������
    [SerializeField]
    LevelUpData levelUpData;

    [SerializeField]
    MonsterData monsterData;

    public void Init()
    {

    }
}
