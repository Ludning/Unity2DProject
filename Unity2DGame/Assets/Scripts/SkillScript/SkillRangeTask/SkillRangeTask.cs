using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleTask : ISkillRangeTask
{
    public Unit[] Activate(InteractiveObject caster, SkillRangeData skillRangeData, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(caster.transform.position, skillRangeData.range);
        int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        return GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
    }
}
public class SemiCircleTask : ISkillRangeTask
{
    public Unit[] Activate(InteractiveObject caster, SkillRangeData skillRangeData, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(caster.transform.position, skillRangeData.range);
        //colliders = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        return GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
    }
}
public class BoxTask : ISkillRangeTask
{
    public Unit[] Activate(InteractiveObject caster, SkillRangeData skillRangeData, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(caster.transform.position - new Vector3(skillRangeData.range, skillRangeData.range, 0), new Vector2(skillRangeData.range * 2, skillRangeData.range * 2), 0);
        int[] instanceIDArray = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        return GameManager.Instance.GetMonsterByInstanceID(instanceIDArray);
    }
}