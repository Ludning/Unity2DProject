using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data Bundle", menuName = "Skill System/Skill Data Bundle")]
public class SkillDataBundle : ScriptableObject
{
    public List<SkillData> data;
}
