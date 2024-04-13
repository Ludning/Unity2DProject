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
    
    //��ų ����� ��, ���� �� �ε��Ҷ� ȣ��
    //�׳� DontDestroyOnLoad�ұ�?
    public void SkillInit(SkillData skillData)
    {
        skillData.skillOneShotDatas.ForEach(data => GetSkillRangeTask(data.skillRangeData.skillRangeType));//skillRangeType�� ����  ��ü ����
        skillData.skillOneShotDatas.ForEach(data => GetSkillBuffTask(data.selfEffectData.skillBuffType));//skillBuffType ����  ��ü ����
        skillData.skillOneShotDatas.ForEach(data => GetSkillDebuffTask(data.targetEffectData.skillDebuffType));//skillDebuffType ����  ��ü ����
        skillData.skillOneShotDatas.ForEach(data => GetSkillMovementTask(data.playerMovementData.skillMovementType));//skillMovementType ����  ��ü ����
        skillData.skillOneShotDatas.ForEach(data => GetSkillMovementTask(data.bladeMovementData.skillMovementType));//skillMovementType ����  ��ü ����
        skillData.skillOneShotDatas.ForEach(data => GetSkillDirectionTask(data.playerMovementData.skillDirectionType));//skillDirectionType ����  ��ü ����
        skillData.skillOneShotDatas.ForEach(data => GetSkillDirectionTask(data.bladeMovementData.skillDirectionType));//skillDirectionType ����  ��ü ��
        skillData.skillOneShotDatas.ForEach(data => GetSkillTargetMovementTask(data.targetMovementData.skillTargetMovementType));//skillTargetMovementType ����  ��ü ����
    }
    public void StartSkillInvoke()
    {
        animation?.Stop();
        currentIndex = 0;

        //�ִϸ��̼� ����, ���
        //skillData.animation;
    }
    public void InvokeNextTask()
    {
        if (currentIndex < currentSkillData.skillOneShotDatas.Count)
        {
            //currentSkillData.skillOneShotDatas[currentIndex]�� ������ Physics.OverlapSphere���� ���� ������Ʈ���� ������, �̵�,���� ��� �۵�
            SkillOneShotData data = currentSkillData.skillOneShotDatas[currentIndex];

            //Ÿ��
            //��ų ���� Ÿ��, �Ű������� �������� �޾ƿ� ���� Ÿ�԰� ���� ���ӽð� ��ġ�� �Է�
            GetSkillBuffTask(data.selfEffectData.skillBuffType).Activate(player);

            //���� ����, ��� ���ǿ� �����ϴ� ���ӿ�����Ʈ? �� ��ȯ
            Unit[] unitInRange = GetSkillRangeTask(data.skillRangeData.skillRangeType).Activate(player.GetWeaponController(), data.skillRangeData, layerMask);
            //Ÿ�ٿ� �������� ��
            unitInRange.ToList().ForEach(x => x.OnDamaged(data.value));
            //��ų ����� Ÿ��, �Ű������� �������� �޾ƿ� ��ü��� ����� Ÿ�԰� ����� ���ӽð� ��ġ�� �Է�
            GetSkillDebuffTask(data.targetEffectData.skillDebuffType).Activate(unitInRange, data.targetEffectData);
            //Ÿ�� ���� �Ǵ� ��ġ��
            GetSkillTargetMovementTask(data.targetMovementData.skillTargetMovementType).Activate(unitInRange, data.targetMovementData.skillMovementValue);

            //�÷��̾� ������ ����, �� �� �¿��� ���� ���͸� �����´�
            Vector2 playerDirection = GetSkillDirectionTask(data.playerMovementData.skillDirectionType).Activate(player.gameObject);
            //�÷��̾� ������ Ÿ��, �Ű������� �޾ƿ� ���Ϳ� ������ ��ü�� �ְ� Activate���ο� Rigidbody�� �̵��ϼ� ȣ��
            GetSkillMovementTask(data.playerMovementData.skillMovementType).Activate(player.gameObject);

            //���̵� ������ ����, �� �� �¿��� ���� ���͸� �����´�
            Vector2 bladeDirection = GetSkillDirectionTask(data.bladeMovementData.skillDirectionType).Activate(player.GetWeaponObject());
            //���̵� ������ Ÿ��, �Ű������� �޾ƿ� ���Ϳ� ������ ��ü�� �ְ� Activate���ο� Rigidbody�� �̵��ϼ� ȣ��
            GetSkillMovementTask(data.bladeMovementData.skillMovementType).Activate(player.GetWeaponObject());

            currentIndex++;
        }
    }
}
