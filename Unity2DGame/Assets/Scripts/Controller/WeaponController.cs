using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;

    private Vector3 offset = new Vector3(0.38f, 0.32f, 0);
    private Quaternion leftQuaternion = new Quaternion(0, 0, -0.2f, 1.0f);
    private Quaternion rightQuaternion = new Quaternion(0, 0, 0.2f, 1.0f);

    private Vector3 targetPos;

    private Direction targetDir;

    [SerializeField]
    private Transform Sprite;


    // Start is called before the first frame update
    void Start()
    {
        if (target == null) return;

        //offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        SetPosition();
        SetRotation();

        Debug.Log($"{Sprite.rotation}");
    }

    public void SetPosition()
    {
        Vector3 dis;
        if (target.position.x > transform.position.x)
        {
            dis = new Vector3(-offset.x, offset.y, offset.z);
            Debug.Log("blade is left");
        }
        else if (target.position.x < transform.position.x)
        {
            dis = new Vector3(offset.x, offset.y, offset.z);
            Debug.Log("blade is right");
        }
        else
        {
            dis = Vector3.zero;
        }
        targetPos = target.position + dis;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
    public void SetRotation()
    {
        Quaternion flippedRotation;
        if(target.position.x > transform.position.x)
        {
            flippedRotation = leftQuaternion;
            Debug.Log("blade is left");
        }
        else if(target.position.x < transform.position.x)
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
