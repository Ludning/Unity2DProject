using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    [SerializeField]
    public RewardType rewardType;
    [SerializeField]
    public int rewardValue;
    public void GetReward()
    {
        switch (rewardType)
        {
            case RewardType.Gold:
                break;
            case RewardType.Equipment:
                break;
            case RewardType.Exp:
                break;
            case RewardType.SkillPoint:
                break;
        }
    }

    public string GetString()
    {
        return $"{rewardType.ToString()} : {rewardValue}";
    }
}
