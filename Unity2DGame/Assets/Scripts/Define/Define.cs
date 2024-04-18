

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

public enum CanvasType
{
    PopupBackCanvas,
    PopupFrontCanvas,
    SceneInformationCanvas,
    SceneStaticCanvas,
    OverlayCanvas,
}
public enum ElementType
{
    GameStatic,
    InventoryBack,
    InventoryFront,
    SkillTreeBack,
    SkillTreeFront,
    InformationOverlay,
    MenuBack,
    MenuFront,
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
    //����
    None,
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
    MaxCount
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
public enum SkillSlotType
{
    SkillTree,
    SkillEquip,
}
public enum SkillTreeType
{
    Fire,
    Frost,
    Tetanus
}
#endregion

#region ������
public enum ItemSlotType
{
    Equipment,
    Inventory,
}
public enum EquipType
{
    First,
    Second,
    Third,
}
public enum IntrinsicProperties
{
    None,
    VenomousSnake,
    PermanentOrgan,
}
public enum ItemStatType
{
    None,
    Hp,
    Mp,
    Attack,
    Defence,
}

#endregion

public enum AtlasType
{
    TileSprite,
    UnitSprite,
}