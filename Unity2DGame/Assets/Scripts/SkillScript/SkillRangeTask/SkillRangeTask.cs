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
        //colliders = colliders.Select(collider => collider.GetInstanceID()).ToArray();
        // 각도 내의 객체만 필터링
        List<Collider2D> filteredHits = new List<Collider2D>();
        foreach (var hit in colliders)
        {
            // 객체의 위치 벡터 계산
            Vector2 toTarget = (hit.transform.position - caster.transform.position).normalized;

            // 두 벡터 간의 각도 계산
            float angle = Vector2.Angle(caster.LookDirection, toTarget);

            // 각도가 특정 범위 내에 있는지 확인
            if (angle <= skillRangeData.angle / 2)
            {
                filteredHits.Add(hit);
                Debug.Log("Detected within angle range: " + hit.name);
            }
        }


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