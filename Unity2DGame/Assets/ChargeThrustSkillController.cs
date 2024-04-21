using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChargeThrustSkillController : SkillController
{
    [SerializeField]
    Animator animator;
    SkillData skillData;
    PlayerController player;
    WeaponController weapon;

    [SerializeField]
    Vector2 size;
    [SerializeField]
    float force;

    public override void AnimationEventEnd()
    {
        weapon.AIStateMachine.SetState(weapon.tracking);
        weapon.spriteRenderer.enabled = true;
        weapon.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    public override void AnimationEventInvoke()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, size, Mathf.Atan2(player.LookDirection.y, player.LookDirection.x) * Mathf.Rad2Deg, 1 << 7);

        // 각도 내의 객체만 필터링
        List<Collider2D> filteredHits = new List<Collider2D>();
        int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        Monster[] monsters = GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
        foreach (var monster in monsters)
        {
            monster.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            monster.OnDamaged((int)(skillData.skillDamage * 0.01 * GameManager.Instance.UserData.playerStatus.attack));
        }
    }

    public override void AnimationEventStart()
    {
        weapon.AIStateMachine.SetState(weapon.rush);
        weapon.spriteRenderer.enabled = false;
        weapon.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        //transform.position = weapon.transform.position;
        gameObject.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, size);
    }
}
