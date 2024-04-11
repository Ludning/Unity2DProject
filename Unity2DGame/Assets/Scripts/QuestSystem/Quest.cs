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

    //����Ʈ �Ϸ�����
    public List<QuestCondition> questConditions;

    //����Ʈ ��������
    public List<Quest> RequestQuests;

    //����Ʈ ����
    public List<Reward> rewards;


    //����Ʈ ��������Ȯ��
    public bool IsCanAccepted
    {
        get 
        {
            if (RequestQuests == null || RequestQuests.Count == 0)
                return true;
            return RequestQuests.All(x => x.IsComplete == true); 
        }
    }

    //����Ʈ �Ϸ�Ǿ�����
    public bool IsComplete
    {
        get { return true; }
    }

    //���� ȹ��
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
