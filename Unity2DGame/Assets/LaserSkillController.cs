using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LaserSkillController : SkillController
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    List<Animator> extendAnimator;

    List<GameObject> targetList = new List<GameObject>();

    [SerializeField]
    GameObject skillPoint;

    [SerializeField]
    float speed = 5f;

    SkillData skillData;
    PlayerController player;
    WeaponController weapon;

    [Range(0.1f, 10f)]
    [SerializeField]
    float range;

    int MaxCount = 20;
    int currentCount = 0;

    private void FixedUpdate()
    {
        if(targetList.Count > 0)
        {
            var data = targetList.Select(t => new { target = t, distance = CalDistance(t.transform.position) })
            .OrderBy(t => t.distance)
            .Select(t => t.target)
            .ToList();

            transform.position = Vector3.MoveTowards(transform.position, data.First().transform.position, speed);
        }
    }
    private float CalDistance(Vector3 targetPosition)
    {
        return (transform.position - targetPosition).sqrMagnitude;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            targetList.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && targetList.Contains(collision.gameObject))
        {
            targetList.Remove(collision.gameObject);
        }
    }
    
    public override void AnimationEventStart()
    {

    }
    public override void AnimationEventInvoke()
    {
        if (currentCount >= MaxCount)
            animator.SetTrigger("IsEnd");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, 1 << 7);
        int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        Monster[] monsters = GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
        foreach (var monster in monsters)
        {
            monster.OnDamaged((int)(skillData.skillDamage * 0.01 * GameManager.Instance.UserData.playerStatus.attack));
        }
        currentCount++;
    }

    public override void AnimationEventEnd()
    {

        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    
    public override void InstallSkill(PlayerController player, WeaponController weapon, SkillData skillData)
    {
        this.skillData = skillData;
        this.player = player;
        this.weapon = weapon;

        transform.position = weapon.transform.position;

        //FollowPlayerRotation(player, weapon);
        currentCount = 0;

        animator.enabled = true;
        extendAnimator.ForEach(animator => { animator.enabled = true; });
    }

}
