using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data Bundle", menuName = "Skill System/Skill Data Bundle")]
public class SkillDataBundle : ScriptableObject
{
    public SkillData nullData;
    public List<SkillData> fire;
    public List<SkillData> frost;
    public List<SkillData> tetanus;

    private List<SkillData> allData;

    public List<SkillData> GetAllData()
    {
        if(allData == null)
            allData = CombineLists(fire, frost, tetanus);
        return allData;
    }
    List<T> CombineLists<T>(List<T> list1, List<T> list2, List<T> list3)
    {
        List<T> result = new List<T>();
        result.AddRange(list1);
        result.AddRange(list2);
        result.AddRange(list3);
        return result;
    }
}
