using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class QuestCondition : ScriptableObject
{
    //몬스터 죽이기,
    //아이템 획득하기,
    //다음 스테이지 넘어가기,
    //특정 기믹 완료하기
    public abstract bool IsCompleted();
}
