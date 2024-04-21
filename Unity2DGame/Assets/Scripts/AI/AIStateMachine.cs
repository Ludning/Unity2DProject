using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine<T>
{
    private T m_sender;

    //현재 상태를 담는 프로퍼티
    public IState<T> CurState { get; set; }


    //기본 상태를 생성시에 설정
    public AIStateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    //State변경
    public void SetState(IState<T> state)
    {
        //Debug.Log("SetState : " + state);

        // null에러출력
        if (m_sender == null)
        {
            Debug.LogError("m_sender ERROR");
            return;
        }

        if (CurState == state)
        {
            Debug.LogWarningFormat("Same state : ", state);
            return;
        }

        if (CurState != null)
            CurState.OperateExit(m_sender);

        //상태 교체.
        CurState = state;

        //새 상태의 Enter를 호출한다.
        if (CurState != null)
            CurState.OperateEnter(m_sender);

        //Debug.Log("SetNextState : " + state);

    }

    //Update
    public void DoOperateUpdate()
    {
        if (m_sender == null)
        {
            //Debug.LogError("invalid m_sener");
            return;
        }
        CurState.OperateUpdate(m_sender);
    }
}
