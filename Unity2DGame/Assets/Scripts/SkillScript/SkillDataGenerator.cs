using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
public class SkillDataGenerator : MonoBehaviour
{
    #region SkillData정보
    [Header("스킬 ID")]
    public int skillId;
    [Space]
    [Header("스킬 이름")]
    public string skillName;
    [Space]
    [Header("스킬 설명")]
    public string skillDiscription;
    [Space]
    [Header("스킬 아이콘")]
    public Sprite skillIcon;
    [Space]
    [Header("스킬 장착 타입")]
    public SkillEquipmentType skillEquipmentType;
    [Space]
    [Header("스킬 1타의 데이터 리스트")]
    public List<SkillOneShotData> skillOneShotDatas;
    [Space]
    [Header("스킬 쿨타임")]
    public float coolTime;
    [Space]
    [Header("스킬 소모값")]
    public int manaCost;
    [Space]
    [Header("스킬 프리팹")]
    public GameObject skillPrefab;
    #endregion


    [Space]
    [Space]

    [Header("스킬 애니메이션 프레임")]
    [Range(1, 10)]
    public int frame;



    //보여주는 인스턴스
    private GameObject skillIconInstance;
    private GameObject skillAnimationInstance;

    // 프리팹 인스턴스를 생성하거나 업데이트하는 메서드
    public void InstantiateOrUpdatePrefab()
    {
        // 기존 인스턴스가 있다면 삭제
        if (skillIconInstance != null)
        {
            DestroyImmediate(skillIconInstance);
        }
        if (skillAnimationInstance != null)
        {
            DestroyImmediate(skillAnimationInstance);
        }
        // 새 인스턴스 생성
        if (skillIcon != null)
        {
            skillIconInstance = new GameObject("skillIcon");
            skillIconInstance.AddComponent<Image>().sprite = skillIcon;
            skillIconInstance.transform.position = new Vector3(-7, -3, 0);
            skillIconInstance.transform.SetParent(GameObject.Find("Canvas").transform);
            skillIconInstance.transform.localScale = Vector3.one;
        }
        if (skillPrefab != null)
        {
            skillAnimationInstance = Instantiate(skillPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (skillOneShotDatas.Count < frame)
        {
            Debug.Log($"skillOneShotDatas.Count : {skillOneShotDatas.Count}");
            return; 
        }

        

        BladeMovementData movementData = skillOneShotDatas[frame - 1].bladeMovementData;
        SkillRangeData rangeData = skillOneShotDatas[frame - 1].skillRangeData;


        Vector2 dir = GetMovementDir(movementData) * GetMovementType(movementData) * movementData.skillMovementValue;

        Vector3 destination = Vector3.zero + new Vector3(dir.x, dir.y, 0);
        if(skillAnimationInstance != null)
            skillAnimationInstance.transform.position = destination;

        Gizmos.DrawLine(Vector3.zero, destination);
        switch (rangeData.skillRangeType)
        {
            case SkillRangeType.Circle:
                Gizmos.DrawWireSphere(destination, rangeData.range);
                break;
            case SkillRangeType.SemiCircle:
                Vector3 lVec = RotateVector(dir.normalized, -rangeData.angle);
                Vector3 rVec = RotateVector(dir.normalized, rangeData.angle);
                Gizmos.DrawLine(destination, destination + lVec);
                Gizmos.DrawLine(destination, destination + rVec);
                Gizmos.DrawLine(destination + lVec, destination + rVec);
                break;
            case SkillRangeType.Box:
                Gizmos.DrawWireCube(destination, new Vector3(rangeData.range / 2, rangeData.range / 2, 0));
                break;
        }
    }
    Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad; // 도를 라디안으로 변환
        float cosTheta = Mathf.Cos(radians);
        float sinTheta = Mathf.Sin(radians);

        float newX = vector.x * cosTheta - vector.y * sinTheta;
        float newY = vector.x * sinTheta + vector.y * cosTheta;

        return new Vector2(newX, newY);
    }
    public Vector2 GetMovementDir(BladeMovementData movementData)
    {
        switch (movementData.skillDirectionType)
        {
            case SkillDirectionType.Idle:
                return Vector2.zero;
            case SkillDirectionType.Front:
                return Vector2.right;
            case SkillDirectionType.Back:
                return Vector2.left;
            case SkillDirectionType.Left:
                return Vector2.up;
            case SkillDirectionType.Right:
                return Vector2.down;
        }
        return Vector2.zero;
    }
    public int GetMovementType(BladeMovementData movementData)
    {
        switch (movementData.skillMovementType)
        {
            case SkillMovementType.Idle:
                return 0;
            case SkillMovementType.Rush:
                return 1;
            case SkillMovementType.Follow:
                return 0;
            case SkillMovementType.SwapPosition:
                return 0;
            case SkillMovementType.Teleportation:
                return 0;
        }
        return 0;
    }
}
#endif
