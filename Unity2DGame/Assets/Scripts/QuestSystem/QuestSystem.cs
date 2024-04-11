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

    //���� ����Ʈ
    List<Quest> AcceptedQuest = new List<Quest>();

    public void AcceptQuest(Quest quest)
    {
        //����Ʈ �������� üũ�ؾ���
        AcceptedQuest.Add(quest);
    }

    public void TestAcceptQuest()
    {
        //����Ʈ �������� üũ�ؾ���
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
