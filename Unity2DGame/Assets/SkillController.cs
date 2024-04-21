using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour, ISkillEventReception
{
    protected Vector2 direction = Vector2.zero;
    public virtual void AnimationEventEnd()
    {
    }

    public virtual void AnimationEventInvoke()
    {
    }

    public virtual void AnimationEventStart()
    {
    }

    public virtual void InstallSkill(PlayerController player, WeaponController weapon, SkillData skillData)
    {
    }
    public virtual void FollowPlayerRotation(PlayerController player, WeaponController weapon)
    {
        float angle = Mathf.Atan2(player.LookDirection.y, player.LookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log($"playerAngle : {angle}");
    }
}
