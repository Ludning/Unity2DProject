using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class QuestCondition : ScriptableObject
{
    //���� ���̱�,
    //������ ȹ���ϱ�,
    //���� �������� �Ѿ��,
    //Ư�� ��� �Ϸ��ϱ�
    public abstract bool IsCompleted();
}
