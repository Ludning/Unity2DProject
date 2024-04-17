using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillButton : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    int skillIndex = 0;

    public SkillSlotType skillSlotType;
    public int SkillSlotIndex;

    //Ŭ���̺�Ʈ ����
    public void OnClick()
    {
        Debug.Log($"{skillSlotType} is click");
        switch (skillSlotType)
        {
            case SkillSlotType.SkillEquip:
                OnClickEquipment();
                break;
            case SkillSlotType.SkillTree:
                OnClickInventory();
                break;
        }
    }

    //��� ����(�κ��丮�� á�� ��� �Ұ�)
    public void OnClickEquipment()
    {
        //Ŭ���� ��ü�� ������ Ȯ���ϰ�
        //GameManager.Instance.UserData.Unequip(skillSlotType);
    }
    //��� ����
    public void OnClickInventory()
    {
        //Ŭ���� ��ü�� ������ Ȯ���ϰ�
        //GameManager.Instance.UserData.EquipItem(skillSlotType);
    }

    public void ChangeSkill(int skillIndex)
    {

    }
    /*public void ChangeItem(int skillIndex)
    {
        if (skillIndex == 0)
        {
            EmptyItem();
            return;
        }
        this.skillIndex = skillIndex;
        image.sprite = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData").items.Find(x => x.id == itemIndex).itemIcon;
        textComponent.enabled = false;
    }
    public void EmptyItem()
    {
        itemIndex = 0;
        //�� ��� �̹��� ���
        image.sprite = null;
        textComponent.enabled = true;
        textComponent.text = "Empty";
    }*/
}
