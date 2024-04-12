using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    Player player;
    PlayerController playerController;
    public WeaponController weaponController;
    private void Awake()
    {
        player = GetComponent<Player>();
        playerController = GetComponent<PlayerController>();
    }
    public void EquipmentSkill(SkillData data, SkillEquipmentType type, int index = 0)
    {
        if (type != SkillEquipmentType.Special && (index < 0 || index > 2))
            return;
        switch (type)
        {
            case SkillEquipmentType.Attack:
                player.AttackData[index] = data;
                break;
            case SkillEquipmentType.Skill:
                player.SkillData[index] = data;
                break;
            case SkillEquipmentType.Special:
                player.SpecialData = data;
                break;
        }
    }
}
