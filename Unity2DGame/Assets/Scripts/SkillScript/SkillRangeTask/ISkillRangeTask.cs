using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillRangeTask
{
    public Unit[] Activate(InteractiveObject caster, SkillRangeData skillRangeData, LayerMask layerMask);
}
