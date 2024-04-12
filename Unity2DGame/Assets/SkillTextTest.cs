using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillTextTest : MonoBehaviour
{
    [SerializeField]
    SkillData skillScriptable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(skillScriptable.skillDiscription);
    }
}
