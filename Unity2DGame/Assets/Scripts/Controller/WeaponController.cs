using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : InteractiveObject
{
    //public Transform target;
    private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
            transform.position = player.transform.position;
            //skillSystem = player.GetComponent<SkillSystem>();
            //skillSystem.SetWeaponController(this);
        }
    }
    
    public float lerpSpeed = 1.0f;

    private Vector3 offset = new Vector3(0.38f, 0.32f, 0);
    private Quaternion leftQuaternion = new Quaternion(0, 0, -0.2f, 1.0f);
    private Quaternion rightQuaternion = new Quaternion(0, 0, 0.2f, 1.0f);

    public bool IsUsingSkill = false;

    private Vector3 playerPos;

    private Direction targetDir;

    [SerializeField]
    private Transform Sprite;

    public SpriteRenderer spriteRenderer;

    //SkillSystem skillSystem;

    public BladeIdle idle = new BladeIdle();
    public BladeTracking tracking = new BladeTracking();
    public BladeAttack attack = new BladeAttack();
    public BladeRush rush = new BladeRush();


    private AIStateMachine<WeaponController> aiStateMachine;
    public AIStateMachine<WeaponController> AIStateMachine
    {
        get
        {
            if (aiStateMachine == null)
                aiStateMachine = new AIStateMachine<WeaponController>(this, tracking);
            return aiStateMachine;
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePaused)
            return;

        if (player == null) return;

        AIStateMachine.DoOperateUpdate();
    }

    public void SetPosition()
    {
        Vector3 dis;
        if (player.transform.position.x > transform.position.x)
        {
            dis = new Vector3(-offset.x, offset.y, offset.z);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            dis = new Vector3(offset.x, offset.y, offset.z);
        }
        else
        {
            dis = Vector3.zero;
        }
        playerPos = player.transform.position + dis;
        transform.position = Vector3.Lerp(transform.position, playerPos, lerpSpeed * Time.deltaTime);
        if((playerPos - transform.position).magnitude > 0.1f)
            LookDirection = playerPos - transform.position;
    }
    public void SetRotation()
    {
        Quaternion flippedRotation;
        if(player.transform.position.x > transform.position.x)
        {
            flippedRotation = leftQuaternion;
        }
        else if(player.transform.position.x < transform.position.x)
        {
            flippedRotation = rightQuaternion;
        }
        else
        {
            flippedRotation = transform.rotation;
        }
        Sprite.transform.rotation = Quaternion.Lerp(Sprite.transform.rotation, flippedRotation, lerpSpeed * Time.deltaTime);
    }

    enum Direction
    {
        Up, 
        Down,
        Left, 
        Right,
    }

    /*public void SkillPlay(SkillData skillData)
    {
        //animator.
        //animator.Play(clip.name);
        //ObjectPool에서 skillObjectPrefab로 불러온다
        //skillObjectPrefab으로 객체InstanceID SkillHandler로 묶여진 딕셔너리에서 가져온다
        GameObject skill = ObjectPool.Instance.GetGameObject(skillData.skillPrefab);
        skill.transform.SetParent(transform);
        skill.transform.position = Vector3.zero;
        SkillHandler handler = GameManager.Instance.GetSkillHandler(skill);
        handler.Init(skillSystem.InvokeNextTask, skillData.name);
    }*/
}

public class BladeIdle : IState<WeaponController>
{
    private WeaponController _blade;


    public void OperateEnter(WeaponController sender)
    {
        _blade = sender;
    }

    public void OperateExit(WeaponController sender)
    {

    }

    public void OperateUpdate(WeaponController sender)
    {
        Vector3 targetPos = sender.Player.gameObject.transform.position;
        Vector3 currentPos = sender.gameObject.transform.position;
        if ((currentPos - targetPos).magnitude > 0.1f)
            sender.AIStateMachine.SetState(sender.tracking);
    }
}
public class BladeTracking : IState<WeaponController>
{
    private WeaponController _blade;


    public void OperateEnter(WeaponController sender)
    {
        _blade = sender;
    }

    public void OperateExit(WeaponController sender)
    {

    }

    public void OperateUpdate(WeaponController sender)
    {
        /*Debug.Log($"{sender.gameObject.name}Traking");
        float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag < 0.8f)
            sender.AIStateMachine.SetState(sender.attack);
        sender.agent.SetDestination(new Vector3(sender.target.transform.position.x, sender.target.transform.position.y));*/

        Vector3 targetPos = sender.Player.gameObject.transform.position;
        Vector3 currentPos = sender.gameObject.transform.position;
        if ((currentPos - targetPos).magnitude < 0.1f)
            sender.AIStateMachine.SetState(sender.idle);
        sender.SetPosition();
        sender.SetRotation();
    }
}
public class BladeAttack : IState<WeaponController>
{
    private WeaponController _blade;


    public void OperateEnter(WeaponController sender)
    {
        _blade = sender;
        _blade.IsUsingSkill = true;
    }

    public void OperateExit(WeaponController sender)
    {
        _blade.IsUsingSkill = false;
    }

    public void OperateUpdate(WeaponController sender)
    {
        /*float mag = (sender.target.transform.position - sender.transform.position).magnitude;
        if (mag > 2)
            sender.AIStateMachine.SetState(sender.tracking);
        Debug.Log("Attack");*/
    }
}
public class BladeRush : IState<WeaponController>
{
    private WeaponController _blade;


    public void OperateEnter(WeaponController sender)
    {
        _blade = sender;
        _blade.IsUsingSkill = true;
    }

    public void OperateExit(WeaponController sender)
    {
        _blade.IsUsingSkill = false;
    }

    public void OperateUpdate(WeaponController sender)
    {

    }
}