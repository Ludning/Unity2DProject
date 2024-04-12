using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    //�̸�
    public string skillName;
    //����
    public string skillDiscription;
    //��ų ������
    public Sprite skillIcon;
    //��ų 1Ÿ�� ������
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
public class SkillOneShotData
{
    //��ų ����
    public float range;
    //��ġ
    public int value;
    //���� ����
    public SkillRangeType skillRangeType;
    //��ų Ÿ��
    public SkillType skillType;
    //��ų ������ Ÿ��
    public SkillMovementType skillMovementType;
    //��ų ������ ��
    public float skillMovementValue;
    //��ų ������ ����
    public SkillDirectionType skillDirectionType;
}
