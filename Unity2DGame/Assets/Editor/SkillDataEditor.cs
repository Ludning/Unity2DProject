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
        base.OnInspectorGUI();  // 기본 인스펙터 UI를 그립니다.

        GUILayout.Space(10);

        SkillDataGenerator dataGenerator = (SkillDataGenerator)target;

        EditorGUI.BeginChangeCheck();
        dataGenerator.skillIcon = EditorGUILayout.ObjectField("TempSprite", dataGenerator.skillIcon, typeof(Sprite), false) as Sprite;
        dataGenerator.skillPrefab = EditorGUILayout.ObjectField("TempPrefab", dataGenerator.skillPrefab, typeof(GameObject), false) as GameObject;

        // EditorGUI.EndChangeCheck()는 변화가 있었는지를 체크합니다.
        if (EditorGUI.EndChangeCheck())
        {
            // 프리팹 인스턴스를 생성하거나 업데이트하는 메서드 호출
            dataGenerator.InstantiateOrUpdatePrefab();
            Undo.RecordObject(dataGenerator, "Prefab Change"); // Undo 기능 지원
        }
        if (GUILayout.Button("Spawn Sprite And SkillAnimation Instance"))
        {
            dataGenerator.InstantiateOrUpdatePrefab();
        }
        if (GUILayout.Button("Create New Data Instance"))
        {
            // 새로운 ScriptableObject 인스턴스 생성
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


            // 사용자가 저장할 파일 위치를 지정할 수 있는 대화상자를 엽니다.
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

            // 지정된 경로에 파일 저장
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