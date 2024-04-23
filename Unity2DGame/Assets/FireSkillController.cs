using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireSkillController : SkillController
{
    [SerializeField]
    Animator animator;
    SkillData skillData;
    PlayerController player;
    WeaponController weapon;

    [Range(0.1f, 10f)]
    [SerializeField]
    float range;

    [Range(0.1f, 180f)]
    [SerializeField]
    float angle = 100f;

    public override void AnimationEventEnd()
    {
        weapon.AIStateMachine.SetState(weapon.tracking);
        weapon.spriteRenderer.enabled = true;
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    public override void AnimationEventInvoke()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, 1 << 7);

        // 각도 내의 객체만 필터링
        List<Collider2D> filteredHits = new List<Collider2D>();
        foreach (var hit in colliders)
        {
            if (hit is BoxCollider2D)
            {
                Vector2 toTarget = (hit.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(direction, toTarget);
                if (angle <= this.angle / 2)
                    filteredHits.Add(hit);
            }
        }

        int[] instanceIDArray = filteredHits.Select(collider => collider.GetInstanceID()).ToArray();
        Monster[] monsters = GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
        foreach (var monster in monsters)
        {
            monster.OnDamaged((int)(skillData.skillDamage * 0.01 * GameManager.Instance.UserData.playerStatus.attack));
        }
    }

    public override void AnimationEventStart()
    {
        weapon.AIStateMachine.SetState(weapon.attack);
        weapon.spriteRenderer.enabled = false;
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
