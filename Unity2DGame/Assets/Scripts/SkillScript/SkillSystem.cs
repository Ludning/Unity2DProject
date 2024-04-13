using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class SkillSystem : MonoBehaviour
{
    Player player;
    PlayerController playerController;
    private WeaponController weaponController;

    Animation animation = null;

    private readonly Dictionary<SkillRangeType, ISkillRangeTask> SkillRangeTaskDic = new Dictionary<SkillRangeType, ISkillRangeTask>();
    private readonly Dictionary<SkillBuffType, ISkillBuffTask> SkillBuffTaskDic = new Dictionary<SkillBuffType, ISkillBuffTask>();
    private readonly Dictionary<SkillDebuffType, ISkillDebuffTask> SkillDebuffTaskDic = new Dictionary<SkillDebuffType, ISkillDebuffTask>();
    private readonly Dictionary<SkillMovementType, ISkillMovementTask> SkillMovementTaskDic = new Dictionary<SkillMovementType, ISkillMovementTask>();
    private readonly Dictionary<SkillDirectionType, ISkillDirectionTask> SkillDirectionTaskDic = new Dictionary<SkillDirectionType, ISkillDirectionTask>();
    private readonly Dictionary<SkillTargetMovementType, ISkillTargetMovementTask> SkillTargetMovementTaskDic = new Dictionary<SkillTargetMovementType, ISkillTargetMovementTask>();

    LayerMask layerMask = 1 << 7 | 1 << 8 | 1 << 9;


    public SkillData currentSkillData;
    int currentIndex = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerController = GetComponent<PlayerController>();
    }

    public ISkillRangeTask GetSkillRangeTask(SkillRangeType type)
    {
        if (!SkillRangeTaskDic.ContainsKey(type))
        {
            SkillRangeTaskDic.Add(type, SkillTaskFactory.SkillRangeTaskFactory(type));
        }
        return SkillRangeTaskDic[type];
    }
    public ISkillBuffTask GetSkillBuffTask(SkillBuffType type)
    {
        if (!SkillBuffTaskDic.ContainsKey(type))
        {
            SkillBuffTaskDic.Add(type, SkillTaskFactory.SkillBuffTaskFactory(type));
        }
        return SkillBuffTaskDic[type];
    }
    public ISkillDebuffTask GetSkillDebuffTask(SkillDebuffType type)
    {
        if (!SkillDebuffTaskDic.ContainsKey(type))
        {
            SkillDebuffTaskDic.Add(type, SkillTaskFactory.SkillDebuffTaskFactory(type));
        }
        return SkillDebuffTaskDic[type];
    }
    public ISkillMovementTask GetSkillMovementTask(SkillMovementType type)
    {
        if (!SkillMovementTaskDic.ContainsKey(type))
        {
            SkillMovementTaskDic.Add(type, SkillTaskFactory.SkillMovementTaskFactory(type));
        }
        return SkillMovementTaskDic[type];
    }
    public ISkillDirectionTask GetSkillDirectionTask(SkillDirectionType type)
    {
        if (!SkillDirectionTaskDic.ContainsKey(type))
        {
            SkillDirectionTaskDic.Add(type, SkillTaskFactory.SkillDirectionTaskTaskFactory(type));
        }
        return SkillDirectionTaskDic[type];
    }
    public ISkillTargetMovementTask GetSkillTargetMovementTask(SkillTargetMovementType type)
    {
        if (!SkillTargetMovementTaskDic.ContainsKey(type))
        {
            SkillTargetMovementTaskDic.Add(type, SkillTaskFactory.SkillTargetMovementTaskFactory(type));
        }
        return SkillTargetMovementTaskDic[type];
    }


    public void SetWeaponController(WeaponController weaponController)
    {
        this.weaponController = weaponController;
    }
    
    //스킬 장비할 때, 다음 씬 로드할때 호출
    //그냥 DontDestroyOnLoad할까?
    public void SkillInit(SkillData skillData)
    {
        skillData.skillOneShotDatas.ForEach(data => GetSkillRangeTask(data.skillRangeData.skillRangeType));//skillRangeType에 따라  객체 생성
        skillData.skillOneShotDatas.ForEach(data => GetSkillBuffTask(data.selfEffectData.skillBuffType));//skillBuffType 따라  객체 생성
        skillData.skillOneShotDatas.ForEach(data => GetSkillDebuffTask(data.targetEffectData.skillDebuffType));//skillDebuffType 따라  객체 생성
        skillData.skillOneShotDatas.ForEach(data => GetSkillMovementTask(data.playerMovementData.skillMovementType));//skillMovementType 따라  객체 생성
        skillData.skillOneShotDatas.ForEach(data => GetSkillMovementTask(data.bladeMovementData.skillMovementType));//skillMovementType 따라  객체 생성
        skillData.skillOneShotDatas.ForEach(data => GetSkillDirectionTask(data.playerMovementData.skillDirectionType));//skillDirectionType 따라  객체 생성
        skillData.skillOneShotDatas.ForEach(data => GetSkillDirectionTask(data.bladeMovementData.skillDirectionType));//skillDirectionType 따라  객체 생
        skillData.skillOneShotDatas.ForEach(data => GetSkillTargetMovementTask(data.targetMovementData.skillTargetMovementType));//skillTargetMovementType 따라  객체 생성
    }
    public void StartSkillInvoke()
    {
        animation?.Stop();
        currentIndex = 0;

        //애니메이션 생성, 출력
        //skillData.animation;
    }
    public void InvokeNextTask()
    {
        if (currentIndex < currentSkillData.skillOneShotDatas.Count)
        {
            //currentSkillData.skillOneShotDatas[currentIndex]의 정보로 Physics.OverlapSphere으로 주위 오브젝트에게 데미지, 이동,등의 기믹 작동
            SkillOneShotData data = currentSkillData.skillOneShotDatas[currentIndex];

            //타입
            //스킬 버프 타입, 매개변수로 범위에서 받아온 버프 타입과 버프 지속시간 수치를 입력
            GetSkillBuffTask(data.selfEffectData.skillBuffType).Activate(player);

            //범위 종류, 모든 조건에 충족하는 게임오브젝트? 를 반환
            Unit[] unitInRange = GetSkillRangeTask(data.skillRangeData.skillRangeType).Activate(player.GetWeaponController(), data.skillRangeData, layerMask);
            //타겟에 데미지를 줌
            unitInRange.ToList().ForEach(x => x.OnDamaged(data.value));
            //스킬 디버프 타입, 매개변수로 범위에서 받아온 객체들과 디버프 타입과 디버프 지속시간 수치를 입력
            GetSkillDebuffTask(data.targetEffectData.skillDebuffType).Activate(unitInRange, data.targetEffectData);
            //타겟 당기기 또는 밀치기
            GetSkillTargetMovementTask(data.targetMovementData.skillTargetMovementType).Activate(unitInRange, data.targetMovementData.skillMovementValue);

            //플레이어 움직임 방향, 앞 뒤 좌우의 방향 벡터를 가져온다
            Vector2 playerDirection = GetSkillDirectionTask(data.playerMovementData.skillDirectionType).Activate(player.gameObject);
            //플레이어 움직임 타입, 매개변수로 받아온 벡터와 움직일 객체를 넣고 Activate내부에 Rigidbody로 이동하수 호출
            GetSkillMovementTask(data.playerMovementData.skillMovementType).Activate(player.gameObject);

            //블레이드 움직임 방향, 앞 뒤 좌우의 방향 벡터를 가져온다
            Vector2 bladeDirection = GetSkillDirectionTask(data.bladeMovementData.skillDirectionType).Activate(player.GetWeaponObject());
            //블레이드 움직임 타입, 매개변수로 받아온 벡터와 움직일 객체를 넣고 Activate내부에 Rigidbody로 이동하수 호출
            GetSkillMovementTask(data.bladeMovementData.skillMovementType).Activate(player.GetWeaponObject());

            currentIndex++;
        }
    }
}
