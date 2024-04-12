using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    //이름
    public string skillName;
    //설명
    public string skillDiscription;
    //스킬 아이콘
    public Sprite skillIcon;
    //스킬 1타의 데이터
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
public class SkillOneShotData
{
    //스킬 범위
    public float range;
    //수치
    public int value;
    //범위 종류
    public SkillRangeType skillRangeType;
    //스킬 타입
    public SkillType skillType;
    //스킬 움직임 타입
    public SkillMovementType skillMovementType;
    //스킬 움직임 값
    public float skillMovementValue;
    //스킬 움직임 방향
    public SkillDirectionType skillDirectionType;
}
