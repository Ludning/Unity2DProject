

public enum UIType
{
    HPBar,
}
public enum ObjectType
{
    Player,
    Monster,
}

#region 몬스터
public enum MonsterType
{
    Slime,
    Goblin,
    Orc,
}
#endregion

#region 퀘스트 보상
public enum RewardType
{
    //돈
    Gold,
    //장비
    Equipment,
    //경험치
    Exp,
    //스킬 포인트
    SkillPoint,
}
#endregion

#region 상태이상
//상태이상의 종류
public enum StatusEffectType
{
    //지속 피해
    ContinuousDamage,
    //행동 제한
    RestrictedBehavior,
    //디버프
    Debuff,
}
//지속 피해의 종류
public enum ContinuousDamageType
{
    //중독
    Poisoning,
    //출혈
    Bleeding,
    //화상
    Burning,
}
//행동 제한의 종류
public enum RestrictedBehaviorType
{
    //기절
    Stun,
    //속박
    Restraint,
    //수면
    Sleep,
    //빙결
    Freezing,
    //감전
    Shock,
}
//디버프의 종류
public enum DebuffType
{
    //광폭화
    Berserk,
    //둔화
    Slowdown,
    //약화
    Weakening,
    //무장 해제
    Disarm,
    //취약
    Weak,
    //부식
    Corrosion,
}
#endregion