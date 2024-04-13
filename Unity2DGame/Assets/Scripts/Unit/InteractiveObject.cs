using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    private Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection
    {
        get
        {
            return lookDirection;
        }
        set
        {
            lookDirection = value.normalized;
        }
    }

    private Collider2D interactiveCollider;
    public Collider2D InteractiveCollider
    {
        get
        {
            if (interactiveCollider == null)
                interactiveCollider = GetComponent<Collider2D>();
            return interactiveCollider;
        }
    }
}
