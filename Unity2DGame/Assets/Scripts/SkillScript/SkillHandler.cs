using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Action action;
    private string skillName;

    public void Init(Action action, string skillName)
    {
        this.action = action;
        this.skillName = skillName;
        StartAnimation();
    }
    public void Invoke()
    {
        Debug.Log("SkillInvoke");
        action?.Invoke();
    }
    private void FixedUpdate()
    {
        // 첫 번째 레이어의 현재 상태
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(skillName) && stateInfo.normalizedTime >= 1.0f)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
    public void StopAnimation()
    {
        animator.enabled = false;
    }

    public void StartAnimation()
    {
        animator.enabled = true;
    }
}
