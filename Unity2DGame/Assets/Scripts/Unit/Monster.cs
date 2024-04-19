
using UnityEngine;
using UnityEngine.AI;


public class Monster : Unit
{
    private AIStateMachine<Monster> aiStateMachine;
    public AIStateMachine<Monster> AIStateMachine
    {
        get
        {
            if (aiStateMachine == null)
                aiStateMachine = new AIStateMachine<Monster>(this, idle);
            return aiStateMachine;
        }
    }

    public GameObject target = null;

    public Idle idle = new Idle();
    public Walk walk = new Walk();
    public Tracking tracking = new Tracking();
    public Attack attack = new Attack();

    Rigidbody rigid;

    [SerializeField]
    public NavMeshAgent agent;

    public Rigidbody Rigid
    {
        get
        {
            if(rigid == null)
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
        rigid = GetComponent<Rigidbody>();
        #region uiStatusBar √ ±‚»≠
        uiStatusBar.Init(gameObject.name, HpRatio);
        #endregion
    }
    public override void OnDie()
    {
        GameManager.Instance.RemoveMonster(this);
        base.OnDie();
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        AIStateMachine.DoOperateUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            target = collision.gameObject;
            AIStateMachine.SetState(tracking);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            AIStateMachine.SetState(idle);
        }
    }
}


public class Idle : IState<Monster>
{
    private Monster _monster;


    public void OperateEnter(Monster sender)
    {
        _monster = sender;
    }

    public void OperateExit(Monster sender)
    {

    }

    public void OperateUpdate(Monster sender)
    {
        Debug.Log("Idle");
    }
}
public class Walk : IState<Monster>
{
    private Monster _monster;


    public void OperateEnter(Monster sender)
    {
        _monster = sender;
    }

    public void OperateExit(Monster sender)
    {

    }

    public void OperateUpdate(Monster sender)
    {
        Debug.Log("Walk");
    }
}
public class Tracking : IState<Monster>
{
    private Monster _monster;


    public void OperateEnter(Monster sender)
    {
        _monster = sender;
    }

    public void OperateExit(Monster sender)
    {

    }

    public void OperateUpdate(Monster sender)
    {
        Debug.Log($"{sender.gameObject.name}Traking");
        float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag < 0.8f)
            sender.AIStateMachine.SetState(sender.attack);
        sender.agent.SetDestination(new Vector3(sender.target.transform.position.x, sender.target.transform.position.y));
    }
}
public class Attack : IState<Monster>
{
    private Monster _monster;


    public void OperateEnter(Monster sender)
    {
        _monster = sender;
    }

    public void OperateExit(Monster sender)
    {

    }

    public void OperateUpdate(Monster sender)
    {
        float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag > 2)
            sender.AIStateMachine.SetState(sender.tracking);
        Debug.Log("Attack");
    }
}