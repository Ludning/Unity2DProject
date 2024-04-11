using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TrackTarget : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    public GameObject Target { set { target = value; } }

    [SerializeField]
    float distance = 1.0f;

    private void LateUpdate()
    {
        if (target == null)
            return;
        //gameObject.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        gameObject.transform.position = target.transform.position + (Vector3.up * distance);
    }
}
