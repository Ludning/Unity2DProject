using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(SkillDataGenerator))]
public class SkillDataEditor : Editor
{
    GameObject skillAnimation;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  // �⺻ �ν����� UI�� �׸��ϴ�.

        GUILayout.Space(10);

        SkillDataGenerator dataGenerator = (SkillDataGenerator)target;

        EditorGUI.BeginChangeCheck();
        dataGenerator.skillIcon = EditorGUILayout.ObjectField("TempSprite", dataGenerator.skillIcon, typeof(Sprite), false) as Sprite;
        dataGenerator.skillPrefab = EditorGUILayout.ObjectField("TempPrefab", dataGenerator.skillPrefab, typeof(GameObject), false) as GameObject;

        // EditorGUI.EndChangeCheck()�� ��ȭ�� �־������� üũ�մϴ�.
        if (EditorGUI.EndChangeCheck())
        {
            // ������ �ν��Ͻ��� �����ϰų� ������Ʈ�ϴ� �޼��� ȣ��
            dataGenerator.InstantiateOrUpdatePrefab();
            Undo.RecordObject(dataGenerator, "Prefab Change"); // Undo ��� ����
        }
        if (GUILayout.Button("Spawn Sprite And SkillAnimation Instance"))
        {
            dataGenerator.InstantiateOrUpdatePrefab();
        }
        if (GUILayout.Button("Create New Data Instance"))
        {
            // ���ο� ScriptableObject �ν��Ͻ� ����
            SkillData newData = ScriptableObject.CreateInstance<SkillData>();


            newData.skillId = dataGenerator.skillId;
            newData.skillName = dataGenerator.skillName;
            newData.skillDiscription = dataGenerator.skillDiscription;
            newData.skillIcon = dataGenerator.skillIcon;
            newData.skillEquipmentType = dataGenerator.skillEquipmentType;
            newData.skillOneShotDatas = dataGenerator.skillOneShotDatas;
            newData.coolTime = dataGenerator.coolTime;
            newData.manaCost = dataGenerator.manaCost;
            newData.skillPrefab = dataGenerator.skillPrefab;


            // ����ڰ� ������ ���� ��ġ�� ������ �� �ִ� ��ȭ���ڸ� ���ϴ�.
            string path = "";
            switch (dataGenerator.skillEquipmentType)
            {
                case SkillEquipmentType.Attack:
                    path = $"Assets/Resource/ScriptableObject/Skill/AttackData/{dataGenerator.skillName}.asset";
                    break;
                case SkillEquipmentType.Skill:
                    path = $"Assets/Resource/ScriptableObject/Skill/SkillData/{dataGenerator.skillName}.asset";
                    break;
                case SkillEquipmentType.Special:
                    path = $"Assets/Resource/ScriptableObject/Skill/SpecialData/{dataGenerator.skillName}.asset";
                    break;
                default:
                    break;
            }

            // ������ ��ο� ���� ����
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(newData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newData;
            }
        }
    }
}
#endif