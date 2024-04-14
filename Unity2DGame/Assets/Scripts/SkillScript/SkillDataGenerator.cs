using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
public class SkillDataGenerator : MonoBehaviour
{
    #region SkillData����
    [Header("��ų ID")]
    public int skillId;
    [Space]
    [Header("��ų �̸�")]
    public string skillName;
    [Space]
    [Header("��ų ����")]
    public string skillDiscription;
    [Space]
    [Header("��ų ������")]
    public Sprite skillIcon;
    [Space]
    [Header("��ų ���� Ÿ��")]
    public SkillEquipmentType skillEquipmentType;
    [Space]
    [Header("��ų 1Ÿ�� ������ ����Ʈ")]
    public List<SkillOneShotData> skillOneShotDatas;
    [Space]
    [Header("��ų ��Ÿ��")]
    public float coolTime;
    [Space]
    [Header("��ų �Ҹ�")]
    public int manaCost;
    [Space]
    [Header("��ų ������")]
    public GameObject skillPrefab;
    #endregion


    [Space]
    [Space]

    [Header("��ų �ִϸ��̼� ������")]
    [Range(1, 10)]
    public int frame;



    //�����ִ� �ν��Ͻ�
    private GameObject skillIconInstance;
    private GameObject skillAnimationInstance;

    // ������ �ν��Ͻ��� �����ϰų� ������Ʈ�ϴ� �޼���
    public void InstantiateOrUpdatePrefab()
    {
        // ���� �ν��Ͻ��� �ִٸ� ����
        if (skillIconInstance != null)
        {
            DestroyImmediate(skillIconInstance);
        }
        if (skillAnimationInstance != null)
        {
            DestroyImmediate(skillAnimationInstance);
        }
        // �� �ν��Ͻ� ����
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
        float radians = angle * Mathf.Deg2Rad; // ���� �������� ��ȯ
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
