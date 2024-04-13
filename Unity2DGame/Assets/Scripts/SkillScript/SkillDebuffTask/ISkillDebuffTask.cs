using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillDebuffTask
{
    public void Activate(Unit[] unit, TargetEffectData targetEffectData);
}
