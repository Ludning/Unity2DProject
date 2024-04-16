using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.Init();

    }
}
