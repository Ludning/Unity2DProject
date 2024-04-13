using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    //id
    public int skillId;
    //�̸�
    public string skillName;
    //����
    public string skillDiscription;
    //��ų ������
    public Sprite skillIcon;
    //��ų ���� Ÿ��
    public SkillEquipmentType skillEquipmentType;
    //��ų 1Ÿ�� �������� ����Ʈ
    public List<SkillOneShotData> skillOneShotDatas;
    //��Ÿ��
    public float coolTime;
    //�Ҹ�
    public int manaCost;
    //�ִϸ��̼�
    public Animation animation;
    
}

//�ִϸ��̼� �̺�Ʈ�� ���Ͽ� �� �̺�Ʈ���� ȣ��
[Serializable]
public struct SkillOneShotData
{
    //�ڽſ��� �� ���� ������
    public SelfEffectData selfEffectData;

    //������
    public int value;

    //��ų ���� ������
    public SkillRangeData skillRangeData;

    //��ǥ���� �� ����� ������
    public TargetEffectData targetEffectData;
    //�÷��̾� ������ ��
    public PlayerMovementData playerMovementData;
    //���̵� ������ ��
    public BladeMovementData bladeMovementData;
    //Ÿ�� ������ ��
    public TargetMovementData targetMovementData;
}

[Serializable]
public struct SkillRangeData
{
    //��ų ����
    public float range;
    //��ų ����
    public float angle;
    //���� ����
    public SkillRangeType skillRangeType;
}

[Serializable]
public struct SelfEffectData
{
    //���� ��ġ Ȥ�� ���ӽð�
    public float value;
    //���� Ÿ��
    public SkillBuffType skillBuffType;
}

[Serializable]
public struct TargetEffectData
{
    //����� ��ġ Ȥ�� ���ӽð�
    public float value;
    //��ų ������ Ÿ��
    public SkillDebuffType skillDebuffType;
}

[Serializable]
public struct PlayerMovementData
{
    //��ų ������ Ÿ��
    public SkillMovementType skillMovementType;
    //��ų ������ ��
    public float skillMovementValue;
    //��ų ������ ����
    public SkillDirectionType skillDirectionType;
}
[Serializable]
public struct BladeMovementData
{
    //��ų ������ Ÿ��
    public SkillMovementType skillMovementType;
    //��ų ������ ��
    public float skillMovementValue;
    //��ų ������ ����
    public SkillDirectionType skillDirectionType;
}

[Serializable]
public struct TargetMovementData
{
    //��ų ������ ��
    public float skillMovementValue;
    //Ÿ�� ������ Ÿ��
    public SkillTargetMovementType skillTargetMovementType;
}
