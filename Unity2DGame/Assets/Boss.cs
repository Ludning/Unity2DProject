using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Unit
{
    private AIStateMachine<Boss> aiStateMachine;
    public AIStateMachine<Boss> AIStateMachine
    {
        get
        {
            if (aiStateMachine == null)
                aiStateMachine = new AIStateMachine<Boss>(this, idle);
            return aiStateMachine;
        }
    }
    public GameObject target = null;

    public BossIdle idle = new BossIdle();
    public BossWalk walk = new BossWalk();
    public BossTracking tracking = new BossTracking();
    public BossAttack attack = new BossAttack();


    Rigidbody rigid;

    [SerializeField]
    public NavMeshAgent agent;
    [SerializeField]
    Collider2D collider;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Animator animator;

    public int GetColliderInstanceID()
    {
        return collider.GetInstanceID();
    }

    public Rigidbody Rigid
    {
        get
        {
            if (rigid == null)
                rigid = GetComponent<Rigidbody>();
            return rigid;
        }
    }
    private void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Init(string statusBarName)
    {
        base.Init(statusBarName);
        MonsterData data = ResourceManager.Instance.GetScriptableData<MonsterData>(gameObject.name);
        status.maxHp = data.maxHp;
        status.hp = data.maxHp;
        status.attack = data.attack;
        status.defence = data.defence;

        rigid = GetComponent<Rigidbody>();
        #region uiStatusBar 초기화
        uiStatusBar.Init(gameObject.name, HpRatio);
        #endregion
    }


    public override void OnDie()
    {
        ItemData itemData = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData");
        int index = Random.Range(0, itemData.items.Count * 3);

        if (index < itemData.items.Count)
        {
            GameObject go = ObjectPool.Instance.GetGameObject(ResourceManager.Instance.GetPrefab("DropItem"));
            go.GetComponent<DropItem>().Init(itemData.items[index]);
            go.transform.position = transform.position;
        }

        base.OnDie();

        GameManager.Instance.UserData.Save();
        ScenesManager.Instance.LoadScene("EndScene");
    }


    private void FixedUpdate()
    {
        if (agent.velocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (agent.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        AIStateMachine.DoOperateUpdate();
    }

}


public class BossIdle : IState<Boss>
{
    private Boss _boss;


    public void OperateEnter(Boss sender)
    {
        _boss = sender;
    }

    public void OperateExit(Boss sender)
    {

    }

    public void OperateUpdate(Boss sender)
    {
        //Debug.Log("Idle");
    }
}
public class BossWalk : IState<Boss>
{
    private Boss _boss;


    public void OperateEnter(Boss sender)
    {
        _boss = sender;
    }

    public void OperateExit(Boss sender)
    {

    }

    public void OperateUpdate(Boss sender)
    {
        Debug.Log("Walk");
    }
}
public class BossTracking : IState<Boss>
{
    private Boss _boss;


    public void OperateEnter(Boss sender)
    {
        _boss = sender;
    }

    public void OperateExit(Boss sender)
    {

    }

    public void OperateUpdate(Boss sender)
    {
        float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag < 0.8f)
            sender.AIStateMachine.SetState(sender.attack);
        sender.agent.SetDestination(new Vector3(sender.target.transform.position.x, sender.target.transform.position.y));
    }
}
public class BossAttack : IState<Boss>
{
    private Boss _boss;
    private float timer = 0;
    private float delay = 2f;

    public void OperateEnter(Boss sender)
    {
        _boss = sender;
    }

    public void OperateExit(Boss sender)
    {

    }

    public void OperateUpdate(Boss sender)
    {
        float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag > 2)
            sender.AIStateMachine.SetState(sender.tracking);

        timer += Time.deltaTime;  // 매 프레임마다 경과된 시간을 타이머에 더합니다.

        if (timer >= delay)  // 타이머가 지정된 지연 시간을 초과하면,
        {
            GameManager.Instance.player.OnDamaged(sender.status.attack);
            timer = 0;  // 타이머를 재설정합니다 (필요한 경우).
        }
    }
}