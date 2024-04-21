using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("스킬 ID")]
    public int skillId;
    [Space]
    [Header("스킬 이름")]
    public string skillName;
    [Space]
    [Header("스킬 설명")]
    public string skillDiscription;
    [Space]
    [Header("스킬 아이콘")]
    public Sprite skillIcon;
    [Space]
    [Header("스킬 발사 타입")]
    public bool isProjectile = false;
    [Space]
    [Header("스킬 장착 타입")]
    public SkillEquipmentType skillEquipmentType;
    [Space]
    [Header("스킬 데미지 배율")]
    public float skillDamage;
    [Space]
    [Header("스킬 쿨타임")]
    public float coolTime;
    [Space]
    [Header("스킬 소모값")]
    public int manaCost;
    [Space]
    [Header("스킬 프리팹")]
    public GameObject skillPrefab;
    
}


[Serializable]
public struct SkillRangeData
{
    [Header("스킬 범위")]
    public float range;

    [Header("스킬 각도")]
    [Range(1,179)]
    public float angle;

    [Header("스킬 범위 종류")]
    public SkillRangeType skillRangeType;
}

[Serializable]
public struct SelfEffectData
{
    [Header("버프 지속시간 또는 값")]
    public float value;
    [Header("버프 타입")]
    public SkillBuffType skillBuffType;
}

[Serializable]
public struct TargetEffectData
{
    [Header("디버프 지속시간")]
    public float value;
    [Header("디버프 타입")] 
    public SkillDebuffType skillDebuffType;
}

[Serializable]
public struct PlayerMovementData
{
    [Header("움직임 타입")]
    public SkillMovementType skillMovementType;
    [Header("움직임 수치")]
    public float skillMovementValue;
    [Header("움직임 방향")]
    public SkillDirectionType skillDirectionType;
}
[Serializable]
public struct BladeMovementData
{
    [Header("움직임 타입")]
    public SkillMovementType skillMovementType;
    [Header("움직임 수치")]
    public float skillMovementValue;
    [Header("움직임 방향")]
    public SkillDirectionType skillDirectionType;
}

[Serializable]
public struct TargetMovementData
{
    [Header("움직임 값")]
    public float skillMovementValue;
    [Header("움직임 타입")]
    public SkillTargetMovementType skillTargetMovementType;
}
