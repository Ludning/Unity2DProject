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
    //이름
    public string skillName;
    //설명
    public string skillDiscription;
    //스킬 아이콘
    public Sprite skillIcon;
    //스킬 장착 타입
    public SkillEquipmentType skillEquipmentType;
    //스킬 1타의 데이터의 리스트
    public List<SkillOneShotData> skillOneShotDatas;
    //쿨타임
    public float coolTime;
    //소모값
    public int manaCost;
    //애니메이션
    public Animation animation;
    
}

//애니메이션 이벤트에 의하여 각 이벤트마다 호출
[Serializable]
public struct SkillOneShotData
{
    //자신에게 줄 버프 데이터
    public SelfEffectData selfEffectData;

    //데미지
    public int value;

    //스킬 범위 데이터
    public SkillRangeData skillRangeData;

    //목표에게 줄 디버프 데이터
    public TargetEffectData targetEffectData;
    //플레이어 움직임 값
    public PlayerMovementData playerMovementData;
    //블레이드 움직임 값
    public BladeMovementData bladeMovementData;
    //타겟 움직임 값
    public TargetMovementData targetMovementData;
}

[Serializable]
public struct SkillRangeData
{
    //스킬 범위
    public float range;
    //스킬 각도
    public float angle;
    //범위 종류
    public SkillRangeType skillRangeType;
}

[Serializable]
public struct SelfEffectData
{
    //버프 수치 혹은 지속시간
    public float value;
    //버프 타입
    public SkillBuffType skillBuffType;
}

[Serializable]
public struct TargetEffectData
{
    //디버프 수치 혹은 지속시간
    public float value;
    //스킬 데미지 타입
    public SkillDebuffType skillDebuffType;
}

[Serializable]
public struct PlayerMovementData
{
    //스킬 움직임 타입
    public SkillMovementType skillMovementType;
    //스킬 움직임 값
    public float skillMovementValue;
    //스킬 움직임 방향
    public SkillDirectionType skillDirectionType;
}
[Serializable]
public struct BladeMovementData
{
    //스킬 움직임 타입
    public SkillMovementType skillMovementType;
    //스킬 움직임 값
    public float skillMovementValue;
    //스킬 움직임 방향
    public SkillDirectionType skillDirectionType;
}

[Serializable]
public struct TargetMovementData
{
    //스킬 움직임 값
    public float skillMovementValue;
    //타겟 움직임 타입
    public SkillTargetMovementType skillTargetMovementType;
}
