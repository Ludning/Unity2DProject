using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelController : UI_Controller
{
    [SerializeField]
    Image attackImage;
    [SerializeField]
    Image skillImage;
    [SerializeField]
    Image specialImage;

    [Space]
    [SerializeField]
    Image attackCooltimeImage;
    [SerializeField]
    Image skillCooltimeImage;
    [SerializeField]
    Image specialCooltimeImage;

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGamePaused)
            return;
        UpdateAttackImage();
        UpdateSkillCoolTimeImageFill();
        UpdateSpecialCoolTimeImageFill();
    }

    public void UpdateAttackImage()
    {
        int skillId = GameManager.Instance.UserData.GetCurrentSkill;
        SkillData skillData = GameManager.Instance.UserData.GetSkillData(skillId);
        attackImage.sprite = skillData.skillIcon;
    }
    public void UpdateSkillCoolTimeImageFill()
    {
        int skillId = GameManager.Instance.UserData.GetCurrentSkill;
        SkillData skillData = GameManager.Instance.UserData.GetSkillData(skillId);
        skillImage.sprite = skillData.skillIcon;
        float currentCoolTime = GameManager.Instance.UserData.GetCurrentSkillCoolTiem;
        skillCooltimeImage.fillAmount = SkillCoolTimeRatio(skillData.coolTime, currentCoolTime);
    }
    public void UpdateSpecialCoolTimeImageFill()
    {
        int skillId = GameManager.Instance.UserData.GetCurrentSpecial;
        SkillData skillData = GameManager.Instance.UserData.GetSkillData(skillId);
        specialImage.sprite = skillData.skillIcon;
        float currentCoolTime = GameManager.Instance.UserData.GetCurrentSpecialCoolTime;
        skillCooltimeImage.fillAmount = SkillCoolTimeRatio(skillData.coolTime, currentCoolTime);
    }
    private float SkillCoolTimeRatio(float maxCoolTime, float currentCooltime)
    {
        // 0���� ������ ���� ����
        if (maxCoolTime == 0)  
        {
            return 0f;
        }
        // ���� ��Ÿ���� �ִ� ��Ÿ������ ������ ���� �ð� ������ ����
        return currentCooltime / maxCoolTime;
    }
}