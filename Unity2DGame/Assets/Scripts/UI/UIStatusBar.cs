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
        //nameField.transform.SetAsFirstSibling();  // 텍스트를 가장 먼저
        //hpBarSlider.transform.SetSiblingIndex(1);  // 체력바를 두 번째로
        //StatusEffectIMGContainer.SetAsLastSibling();  // 아이콘을 가장 마지막으로
    }
    //초기화
    public void Init(string name, float hpRatio)
    {
        nameField.text = name;
        HPBarRefresh(hpRatio);
    }


    //Hp게이지 갱신
    public void HPBarRefresh(float hpRatio)
    {
        hpBarSlider.value = hpRatio;
    }

    //상태이상 아이콘 추가
    public void AddStatusEffect(GameObject sprite)
    {
        GameObject img = ObjectPool.Instance.GetGameObject(sprite);
        img.transform.parent = StatusEffectIMGContainer;
        img.transform.localScale = Vector3.one;
        StatusEffectIMGList.Add(sprite.name, img);
    }

    //상태이상 아이콘 제거
    public void RemoveStatusEffect(GameObject sprite)
    {
        if (StatusEffectIMGList.TryGetValue(sprite.name, out GameObject img))
        {
            StatusEffectIMGList.Remove(sprite.name);
            ObjectPool.Instance.ReturnToPool(img);
        }
    }
}
