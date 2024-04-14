using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    private GameData gamedata;
    private UserData userdata;
    private GameSettingData gameSettingData;

    public Player player;

    private Dictionary<int, Monster> monsterDic = new Dictionary<int, Monster>();

    private Dictionary<int, SkillHandler> skillHandlerDic = new Dictionary<int, SkillHandler>();

    public void AddMonster(Monster monster)
    {
        int instanceID = monster.InteractiveCollider.GetInstanceID();
        monsterDic.Add(instanceID, monster);
    }
    public void RemoveMonster(Monster monster)
    {
        int instanceID = monster.InteractiveCollider.GetInstanceID();
        monsterDic.Remove(instanceID);
    }

    public SkillHandler GetSkillHandler(GameObject skillObject)
    {
        int instanceID = skillObject.GetInstanceID();
        if (!monsterDic.ContainsKey(instanceID))
            AddSkillHandler(skillObject);
        return skillHandlerDic[instanceID];
    }
    public void AddSkillHandler(GameObject skillObject)
    {
        int instanceID = skillObject.GetInstanceID();
        skillHandlerDic.Add(instanceID, skillObject.GetComponent<SkillHandler>());
    }

    public Monster[] GetMonsterByInstanceID(int[] InstanceID)
    {
        return InstanceID.Where(k => monsterDic.ContainsKey(k))
                              .Select(k => monsterDic[k])
                              .ToArray();
    }

    public void GetPlayerData()
    {
        return;
    }
    public void GetMonsterData()
    {
        return;
    }
    public void GetStatusEffectData()
    {
        return;
    }
}
