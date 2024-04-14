using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.U2D;

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
        // ���� ���� ��ü�� ���͸�
        List<Collider2D> filteredHits = new List<Collider2D>();
        foreach (var hit in colliders)
        {
            Vector2 toTarget = (hit.transform.position - caster.transform.position).normalized;
            float angle = Vector2.Angle(caster.LookDirection, toTarget);
            if (angle <= skillRangeData.angle / 2)
                filteredHits.Add(hit);
        }

        int[] instanceIDArray = filteredHits.Select(collider => collider.GetInstanceID()).ToArray();
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