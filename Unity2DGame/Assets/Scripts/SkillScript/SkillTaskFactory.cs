using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTaskFactory
{
    public static ISkillRangeTask SkillRangeTaskFactory(SkillRangeType type)
    {
        switch (type)
        {
            case SkillRangeType.Circle:
                return new CircleTask();
            case SkillRangeType.SemiCircle:
                return new SemiCircleTask();
            case SkillRangeType.Box:
                return new BoxTask();
        }
        return null;
    }
    public static ISkillBuffTask SkillBuffTaskFactory(SkillBuffType type)
    {
        switch (type)
        {
            case SkillBuffType.IncreaseAttack:
                return new IncreaseAttackTask();
            case SkillBuffType.IncreaseMoveSpeed:
                return new IncreaseMoveSpeedTask();
            case SkillBuffType.StiffImmunity:
                return new StiffImmunityTask();
            case SkillBuffType.StaminaRecovery:
                return new StaminaRecoveryTask();
        }
        return null;
    }
    public static ISkillDebuffTask SkillDebuffTaskFactory(SkillDebuffType type)
    {
        switch (type)
        {
            case SkillDebuffType.Slowdown:
                return new SlowdownTask();
            case SkillDebuffType.Weak:
                return new WeakTask();
            case SkillDebuffType.Freezing:
                return new FreezingTask();
            case SkillDebuffType.Shock:
                return new ShockTask();
            case SkillDebuffType.Stun:
                return new StunTask();
            case SkillDebuffType.Bleeding:
                return new BleedingTask();
        }
        return null;
    }
    public static ISkillMovementTask SkillMovementTaskFactory(SkillMovementType type)
    {
        switch (type)
        {
            case SkillMovementType.Idle:
                return new IdleTask();
            case SkillMovementType.Rush:
                return new RushTask();
            case SkillMovementType.Follow:
                return new FollowTask();
            case SkillMovementType.SwapPosition:
                return new SwapPositionTask();
            case SkillMovementType.Teleportation:
                return new TeleportationTask();
        }
        return null;
    }
    public static ISkillDirectionTask SkillDirectionTaskTaskFactory(SkillDirectionType type)
    {
        switch (type)
        {
            case SkillDirectionType.Idle:
                return new IdleDirectionTask();
            case SkillDirectionType.Front:
                return new FrontDirectionTask();
            case SkillDirectionType.Back:
                return new BackDirectionTask();
            case SkillDirectionType.Left:
                return new LeftDirectionTask();
            case SkillDirectionType.Right:
                return new RightDirectionTask();
        }
        return null;
    }
    public static ISkillTargetMovementTask SkillTargetMovementTaskFactory(SkillTargetMovementType type)
    {
        switch (type)
        {
            case SkillTargetMovementType.None:
                return new NoneTask();
            case SkillTargetMovementType.Grab:
                return new GrabTask();
            case SkillTargetMovementType.Thrust:
                return new ThrustTask();
        }
        return null;
    }
}
