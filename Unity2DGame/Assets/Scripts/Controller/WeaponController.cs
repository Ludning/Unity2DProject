using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
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
            player.weapon = this;
            player.GetComponent<SkillSystem>().weaponController = this;
        }
    }
    public float lerpSpeed = 1.0f;

    private Vector3 offset = new Vector3(0.38f, 0.32f, 0);
    private Quaternion leftQuaternion = new Quaternion(0, 0, -0.2f, 1.0f);
    private Quaternion rightQuaternion = new Quaternion(0, 0, 0.2f, 1.0f);

    private Vector3 playerPos;

    private Direction targetDir;

    [SerializeField]
    private Transform Sprite;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null) return;

        //offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        SetPosition();
        SetRotation();
    }

    IEnumerator FollowPlayer()
    {
        SetPosition();
        SetRotation();
        yield return null;
    }

    public void SetPosition()
    {
        Vector3 dis;
        if (player.transform.position.x > transform.position.x)
        {
            dis = new Vector3(-offset.x, offset.y, offset.z);
            Debug.Log("blade is left");
        }
        else if (player.transform.position.x < transform.position.x)
        {
            dis = new Vector3(offset.x, offset.y, offset.z);
            Debug.Log("blade is right");
        }
        else
        {
            dis = Vector3.zero;
        }
        playerPos = player.transform.position + dis;
        transform.position = Vector3.Lerp(transform.position, playerPos, lerpSpeed * Time.deltaTime);
    }
    public void SetRotation()
    {
        Quaternion flippedRotation;
        if(player.transform.position.x > transform.position.x)
        {
            flippedRotation = leftQuaternion;
            Debug.Log("blade is left");
        }
        else if(player.transform.position.x < transform.position.x)
        {
            flippedRotation = rightQuaternion;
            Debug.Log("blade is right");
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
}
