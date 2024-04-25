using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAttackEvent : MonoBehaviour
{
    [SerializeField]
    Monster monster;
    public void Invoke()
    {
        if(GameManager.Instance.player != null)
            GameManager.Instance.player.OnDamaged(monster.status.attack);
    }
}
