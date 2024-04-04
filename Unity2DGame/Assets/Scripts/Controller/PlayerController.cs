using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //float horizontal;
    //float vertical;

    [SerializeField]
    [Range(0f, 10f)]
    float speed;

    //float currentSpeed = 0f;

    [SerializeField]
    Animator animator;
    Rigidbody2D rigidbody;
    Direction playerDir;
    bool IsRun;

    Vector2 playerInput = Vector2.zero;
    Vector2 playerNormal = Vector2.zero;
    float playerMagnitude = 0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerDir = Direction.IDLE;
    }

    void Update()
    {
        if (playerNormal == Vector2.zero)
            IsRun = false;
        else
            IsRun = true;
        animator.SetInteger("Direction", (int)playerDir);
        animator.SetBool("IsRun", IsRun);
    }
    private void FixedUpdate()
    {
        // �̵��ӵ�*���⺤��*Magnitude�� velocity��
        rigidbody.velocity = speed * playerNormal * playerMagnitude;
        int dir = animator.GetInteger("Direction");
        bool bo = animator.GetBool("IsRun");
    }
    //�÷��̾� Sprite ���� ���� �ڵ�
    private void SetDirection()
    {
        if (Mathf.Abs(playerInput.x) > Mathf.Abs(playerInput.y))
        {
            if (playerInput.x > 0)
                playerDir = Direction.RIGHT;
            else
                playerDir = Direction.LEFT;
        }
        else if (Mathf.Abs(playerInput.x) < Mathf.Abs(playerInput.y))
        {
            if (playerInput.y > 0)
                playerDir = Direction.UP;
            else
                playerDir = Direction.DOWN;
        }
        else
            playerDir = playerDir;
    }
    public void OnMove(InputValue context)
    {
        playerInput = context.Get<Vector2>();

        playerNormal = playerInput.normalized;
        playerMagnitude = Mathf.Sqrt(playerInput.x * playerInput.x + playerInput.y * playerInput.y);
        SetDirection();
    }

    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        IDLE,
    }
}
