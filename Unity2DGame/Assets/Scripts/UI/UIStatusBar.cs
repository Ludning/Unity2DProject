using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusBar : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameField;
    [SerializeField]
    Slider hpBarSlider;
    [SerializeField]
    Transform StatusEffectIMGContainer;
    Dictionary<string, GameObject> StatusEffectIMGList = new Dictionary<string, GameObject>();

    private void Start()
    {
        //nameField.transform.SetAsFirstSibling();  // �ؽ�Ʈ�� ���� ����
        //hpBarSlider.transform.SetSiblingIndex(1);  // ü�¹ٸ� �� ��°��
        //StatusEffectIMGContainer.SetAsLastSibling();  // �������� ���� ����������
    }
    //�ʱ�ȭ
    public void Init(string name, float hpRatio)
    {
        nameField.text = name;
        HPBarRefresh(hpRatio);
    }


    //Hp������ ����
    public void HPBarRefresh(float hpRatio)
    {
        hpBarSlider.value = hpRatio;
    }

    //�����̻� ������ �߰�
    public void AddStatusEffect(GameObject sprite)
    {
        GameObject img = ObjectPool.Instance.GetGameObject(sprite);
        img.transform.parent = StatusEffectIMGContainer;
        img.transform.localScale = Vector3.one;
        StatusEffectIMGList.Add(sprite.name, img);
    }

    //�����̻� ������ ����
    public void RemoveStatusEffect(GameObject sprite)
    {
        if (StatusEffectIMGList.TryGetValue(sprite.name, out GameObject img))
        {
            StatusEffectIMGList.Remove(sprite.name);
            ObjectPool.Instance.ReturnToPool(img);
        }
    }
}
