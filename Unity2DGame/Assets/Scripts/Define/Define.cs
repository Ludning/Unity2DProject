

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