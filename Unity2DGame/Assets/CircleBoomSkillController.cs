using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleBoomSkillController : SkillController
{
    [SerializeField]
    Animator animator;
    SkillData skillData;
    PlayerController player;
    WeaponController weapon;

    [Range(0.1f, 10f)]
    [SerializeField]
    float range;


    bool isHeal = true;
    public override void AnimationEventEnd()
    {
        weapon.AIStateMachine.SetState(weapon.tracking);
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    public override void AnimationEventInvoke()
    {
        if(isHeal)
        {
            GameManager.Instance.player.OnHeal((int)(skillData.skillDamage * GameManager.Instance.UserData.playerStatus.attack * 0.4f));
            isHeal = false;
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, 1 << 7);
            int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
            Monster[] monsters = GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
            foreach (var monster in monsters)
            {
                monster.OnDamaged((int)(skillData.skillDamage * 0.01 * GameManager.Instance.UserData.playerStatus.attack));
            }
        }
    }

    public override void AnimationEventStart()
    {
        weapon.AIStateMachine.SetState(weapon.attack);
    }

    public override void InstallSkill(PlayerController player, WeaponController weapon, SkillData skillData)
    {
        this.skillData = skillData;
        this.player = player;
        this.weapon = weapon;

        transform.position = weapon.transform.position;
        FollowPlayerRotation(player, weapon);
        direction = player.LookDirection;
        animator.enabled = true;
    }
}
