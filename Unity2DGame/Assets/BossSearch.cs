using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossSearch : MonoBehaviour
{
    [SerializeField]
    Boss boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.target = collision.gameObject;
            boss.AIStateMachine.SetState(boss.tracking);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.target = null;
            boss.AIStateMachine.SetState(boss.idle);
        }
    }
}
