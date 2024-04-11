using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest", order = 0)]
public class Quest : ScriptableObject
{
    public int id;
    public string questName;
    public string questDiscription;

    //퀘스트 완료조건
    public List<QuestCondition> questConditions;

    //퀘스트 수락조건
    public List<Quest> RequestQuests;

    //퀘스트 보상
    public List<Reward> rewards;


    //퀘스트 수락가능확인
    public bool IsCanAccepted
    {
        get 
        {
            if (RequestQuests == null || RequestQuests.Count == 0)
                return true;
            return RequestQuests.All(x => x.IsComplete == true); 
        }
    }

    //퀘스트 완료되었는지
    public bool IsComplete
    {
        get { return true; }
    }

    //보상 획득
    public void GetReward()
    {
        rewards.ForEach(x => x.GetReward());
    }

    public List<string> GetRewardContextList()
    {
        List<string> contextList = new List<string>();
        rewards.ForEach(r => contextList.Add(r.GetString()));
        return contextList;
    }
}
