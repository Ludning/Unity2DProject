using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterSearch : MonoBehaviour
{
    [SerializeField]
    Monster monster;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            monster.target = collision.gameObject;
            monster.AIStateMachine.SetState(monster.tracking);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            monster.target = null;
            monster.AIStateMachine.SetState(monster.idle);
        }
    }
}
