
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


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

    protected Rigidbody rigid;

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

        if(index < itemData.items.Count)
        {
            GameObject go = ObjectPool.Instance.GetGameObject(ResourceManager.Instance.GetPrefab("DropItem"));
            go.GetComponent<DropItem>().Init(itemData.items[index]);
            go.transform.position = transform.position;
        }

        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        GameManager.Instance.RemoveMonster(this);
        base.OnDie();
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGamePaused)
            return;

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
        //Debug.Log("Idle");
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
        float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag < 0.8f)
            sender.AIStateMachine.SetState(sender.attack);
        sender.agent.SetDestination(new Vector3(sender.target.transform.position.x, sender.target.transform.position.y));
    }
}
public class Attack : IState<Monster>
{
    private Monster _monster;
    private float timer = 0;
    private float delay = 2f;

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

        timer += Time.deltaTime;  // 매 프레임마다 경과된 시간을 타이머에 더합니다.

        if (timer >= delay)  // 타이머가 지정된 지연 시간을 초과하면,
        {
            GameManager.Instance.player.OnDamaged(sender.status.attack);
            timer = 0;  // 타이머를 재설정합니다 (필요한 경우).
        }
    }
}