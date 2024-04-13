

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
//������ ����
public enum SkillBuffType
{
    //���ݷ� ����
    IncreaseAttack,
    //�̵��ӵ� ����
    IncreaseMoveSpeed,
    //���� �鿪
    StiffImmunity,
    //ü�� ȸ��
    StaminaRecovery,
}
//������� ����
public enum SkillDebuffType
{
    //����
    None,
    //��ȭ
    Slowdown,
    //���
    Weak,
    //����
    Freezing,
    //����
    Shock,
    //����
    Stun,
    //����
    Bleeding,
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
public enum SkillMovementType
{
    Idle,
    Rush,
    Follow,
    SwapPosition,
    Teleportation,
}
public enum SkillDirectionType
{
    Idle,
    Front,
    Back,
    Left,
    Right,
}
public enum SkillTargetMovementType
{
    //����
    None,
    //����
    Grab,
    //��ġ��
    Thrust,
}
#endregion