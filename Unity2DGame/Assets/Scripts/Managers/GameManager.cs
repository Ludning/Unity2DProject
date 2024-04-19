using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Manager<GameManager>
{
    public Player player;
    private UserData userData;

    private int pauseCount = 0;

    public bool IsGamePaused
    {
        get
        {
            return pauseCount > 0;
        }
    }
    public void AddPauseCount()
    {
        pauseCount++;
    }
    public void SubPauseCount()
    {
        pauseCount--;
    }

    public UserData UserData 
    {
        get { return userData; }
        set { userData = value; }
    }


    private Dictionary<int, Monster> monsterDic = new Dictionary<int, Monster>();

    private Dictionary<int, SkillHandler> skillHandlerDic = new Dictionary<int, SkillHandler>();


    private void Update()
    {
        if (IsGamePaused)
            return;
        userData.UpdateCoolTime(Time.deltaTime);
    }

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
