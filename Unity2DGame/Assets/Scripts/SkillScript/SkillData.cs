using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("��ų ID")]
    public int skillId;
    [Space]
    [Header("��ų �̸�")]
    public string skillName;
    [Space]
    [Header("��ų ����")]
    public string skillDiscription;
    [Space]
    [Header("��ų ������")]
    public Sprite skillIcon;
    [Space]
    [Header("��ų ���� Ÿ��")]
    public SkillEquipmentType skillEquipmentType;
    [Space]
    [Header("��ų 1Ÿ�� ������ ����Ʈ")]
    public List<SkillOneShotData> skillOneShotDatas;
    [Space]
    [Header("��ų ��Ÿ��")]
    public float coolTime;
    [Space]
    [Header("��ų �Ҹ�")]
    public int manaCost;
    [Space]
    [Header("��ų ������")]
    public GameObject skillPrefab;
    
}

//�ִϸ��̼� �̺�Ʈ�� ���Ͽ� �� �̺�Ʈ���� ȣ��
[Serializable]
public struct SkillOneShotData
{
    [Header("�ڽſ��� ���� ���� ������")]
    public SelfEffectData selfEffectData;

    [Header("������ ����")]
    public int value;

    [Header("��ų ���� ����")]
    public SkillRangeData skillRangeData;

    [Header("��ǥ���� ���� ����� ����")]
    public TargetEffectData targetEffectData;

    [Header("�÷��̾� ������")]
    public PlayerMovementData playerMovementData;

    [Header("���̵� ������")]
    public BladeMovementData bladeMovementData;

    [Header("Ÿ�� ������ ����")]
    public TargetMovementData targetMovementData;
}

[Serializable]
public struct SkillRangeData
{
    [Header("��ų ����")]
    public float range;

    [Header("��ų ����")]
    [Range(1,179)]
    public float angle;

    [Header("��ų ���� ����")]
    public SkillRangeType skillRangeType;
}

[Serializable]
public struct SelfEffectData
{
    [Header("���� ���ӽð�")]
    public float value;
    [Header("���� Ÿ��")]
    public SkillBuffType skillBuffType;
}

[Serializable]
public struct TargetEffectData
{
    [Header("����� ���ӽð�")]
    public float value;
    [Header("����� Ÿ��")] 
    public SkillDebuffType skillDebuffType;
}

[Serializable]
public struct PlayerMovementData
{
    [Header("������ Ÿ��")]
    public SkillMovementType skillMovementType;
    [Header("������ ��ġ")]
    public float skillMovementValue;
    [Header("������ ����")]
    public SkillDirectionType skillDirectionType;
}
[Serializable]
public struct BladeMovementData
{
    [Header("������ Ÿ��")]
    public SkillMovementType skillMovementType;
    [Header("������ ��ġ")]
    public float skillMovementValue;
    [Header("������ ����")]
    public SkillDirectionType skillDirectionType;
}

[Serializable]
public struct TargetMovementData
{
    [Header("������ ��")]
    public float skillMovementValue;
    [Header("������ Ÿ��")]
    public SkillTargetMovementType skillTargetMovementType;
}
