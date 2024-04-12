

using System;

public enum UIType
{
    HPBar,
}
public enum ObjectType
{
    Player,
    Monster,
}

#region ����
public enum MonsterType
{
    Slime,
    Goblin,
    Orc,
}
#endregion

#region ����Ʈ ����
public enum RewardType
{
    //��
    Gold,
    //���
    Equipment,
    //����ġ
    Exp,
    //��ų ����Ʈ
    SkillPoint,
}
#endregion

#region �����̻�
//�����̻��� ����
public enum StatusEffectType
{
    //���� ����
    ContinuousDamage,
    //�ൿ ����
    RestrictedBehavior,
    //�����
    Debuff,
}
//���� ������ ����
public enum ContinuousDamageType
{
    //�ߵ�
    Poisoning,
    //����
    Bleeding,
    //ȭ��
    Burning,
}
//�ൿ ������ ����
public enum RestrictedBehaviorType
{
    //����
    Stun,
    //�ӹ�
    Restraint,
    //����
    Sleep,
    //����
    Freezing,
    //����
    Shock,
}
//������� ����
public enum DebuffType
{
    //����ȭ
    Berserk,
    //��ȭ
    Slowdown,
    //��ȭ
    Weakening,
    //���� ����
    Disarm,
    //���
    Weak,
    //�ν�
    Corrosion,
}
#endregion

#region ��ųŸ��
public enum SkillEquipmentType
{
    Attack,
    Skill,
    Special,
}
public enum SkillRangeType
{
    Circle,
    SemiCircle,
    Box,
}
[Flags]
public enum SkillType
{
    BuffType = 1 << 0,
    DamageType = 1 << 1,
    DebuffType = 1 << 2,
}
[Flags]
public enum SkillMovementType
{
    RushType = 1 << 0,
    FollowType = 1 << 1,
    IdleType = 1 << 2,
    SwapPositionType = 1 << 3,
    TeleportationType = 1 << 4,
}
public enum SkillDirectionType
{
    Idle,
    Front,
    Back,
    Left,
    Right,
}
#endregion