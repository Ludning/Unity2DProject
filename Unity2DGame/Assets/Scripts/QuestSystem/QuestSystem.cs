using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    Quest testQuest;
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;

    //수락 퀘스트
    List<Quest> AcceptedQuest = new List<Quest>();

    public void AcceptQuest(Quest quest)
    {
        //퀘스트 수락가능 체크해야함
        AcceptedQuest.Add(quest);
    }

    public void TestAcceptQuest()
    {
        //퀘스트 수락가능 체크해야함
        AcceptedQuest.Add(testQuest);
    }
    private void FixedUpdate()
    {
        if (AcceptedQuest == null)
            return;
        string text = "";
        AcceptedQuest.ForEach(q => q.GetRewardContextList().ForEach(rT => text += $"{rT}\n"));
        textMeshProUGUI.text = text;
    }
}
