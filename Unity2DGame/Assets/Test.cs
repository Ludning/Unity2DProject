using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    RectInt rectInt;
    // Start is called before the first frame update
    void Start()
    {
        rectInt = new RectInt(0, 0, 5, 5);

        Debug.Log($"{(int)rectInt.center.x}, {(int)rectInt.center.y}");
        Debug.Log($"{(int)rectInt.max.x}, {(int)rectInt.max.y}");
        Debug.Log($"{(int)rectInt.x}, {(int)rectInt.y}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
