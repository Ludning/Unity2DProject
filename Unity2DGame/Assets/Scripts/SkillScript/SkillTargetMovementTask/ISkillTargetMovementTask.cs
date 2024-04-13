using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetMovementTask
{
    public void Activate(Unit[] unit, float skillMovementValue);
}
