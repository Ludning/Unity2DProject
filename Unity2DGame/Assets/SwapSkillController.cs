using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SwapSkillController : SkillController
{
    [SerializeField]
    Animator animator;
    SkillData skillData;
    PlayerController player;
    WeaponController weapon;

    [SerializeField]
    Animator extendAnimator;

    List<Animator> extendAnimators = new List<Animator>();

    public override void AnimationEventStart()
    {
        weapon.spriteRenderer.enabled = false;
        Vector3 temp = player.transform.position;
        player.transform.position = weapon.transform.position;
        weapon.transform.position = temp;
    }
    public override void AnimationEventInvoke()
    {
        Vector2 midPoint = (player.transform.position + weapon.transform.position) / 2; // 중간점
        Vector2 direction = (weapon.transform.position - player.transform.position).normalized; // 방향 벡터
        float distance = Vector2.Distance(player.transform.position, weapon.transform.position); // 거리

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도 계산
        Vector2 size = new Vector2(distance, 1);//박스

        Collider2D[] colliders = Physics2D.OverlapBoxAll(midPoint, size, angle, 1 << 7);


        int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        Monster[] monsters = GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
        foreach (var monster in monsters)
        {
            monster.OnDamaged((int)(skillData.skillDamage * 0.01 * GameManager.Instance.UserData.playerStatus.attack));
        }
    }
    public override void AnimationEventEnd()
    {
        weapon.spriteRenderer.enabled = true;
        foreach (var go in extendAnimators)
        {
            ObjectPool.Instance.ReturnToPool(go.gameObject);
        }
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    
    public override void InstallSkill(PlayerController player, WeaponController weapon, SkillData skillData)
    {
        this.skillData = skillData;
        this.player = player;
        this.weapon = weapon;

        transform.position = player.transform.position;
        FollowPlayerRotation(player, weapon);
        direction = player.LookDirection;

        int count = (int)((weapon.transform.position - transform.position).magnitude); // 대상을 향한 벡터 계산
        Debug.Log($"count is {count}");
        for (int i = 1; i < count; i++)
        {
            GameObject go = ObjectPool.Instance.GetGameObject(extendAnimator.gameObject);
            go.transform.SetParent(transform, true);
            go.transform.localPosition = new Vector2(i, 0);
            extendAnimators.Add(go.GetComponent<Animator>());
        }
        animator.enabled = true;
        extendAnimators.ForEach(go => { go.enabled = true; });
    }
    public override void FollowPlayerRotation(PlayerController player, WeaponController weapon)
    {
        Vector2 direction = weapon.transform.position - transform.position; // 대상을 향한 벡터 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도 계산
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Z축을 기준으로 회전 적용
    }
}

