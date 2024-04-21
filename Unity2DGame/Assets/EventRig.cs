using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRig : MonoBehaviour
{
    [SerializeField]
    SkillController controller;
    public void AnimationEventEnd()
    {
        controller.AnimationEventEnd();
    }

    public void AnimationEventInvoke()
    {
        controller.AnimationEventInvoke();
    }

    public void AnimationEventStart()
    {
        controller.AnimationEventStart();
    }
}
