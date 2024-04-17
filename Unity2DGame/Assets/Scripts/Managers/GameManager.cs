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

    public UserData UserData 
    {
        get { return userData; }
        set { userData = value; }
    }


    private Dictionary<int, Monster> monsterDic = new Dictionary<int, Monster>();

    private Dictionary<int, SkillHandler> skillHandlerDic = new Dictionary<int, SkillHandler>();

    /*private void Awake()
    {
        gameData = DataManager.Instance.LoadObject<GameData>("GameData");
        userData = DataManager.Instance.LoadJsonData<UserData>("UserData");
        if (userData == null)
        {
            MakeFirstUserData();
        }

        TestUserData(userData, gameData);
    }*/

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


    /*#region 게임실행 초기화
    //테스트용 유저 데이터 생성
    static void TestUserData(UserData userData, GameData gameData)
    {
        userData.inventoryItem.Add(1);
        userData.inventoryItem.Add(2);
    }

    // 첫 유저 데이터 생성
    private static UserData MakeFirstUserData()
    {
        UserData userData = new UserData();

        userData.playerStatus = new Status();
        userData.gold = 100;

        var json = JsonConvert.SerializeObject(userData);
        DataManager.Instance.SaveJsonData(typeof(UserData).Name, json);

        return userData;
    }
    #endregion*/
}
