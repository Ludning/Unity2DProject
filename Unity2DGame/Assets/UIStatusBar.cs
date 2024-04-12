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

    public void Init(string name, float hpRatio)
    {
        nameField.text = name;
        HPBarRefresh(hpRatio);
    }
    public void HPBarRefresh(float hpRatio)
    {
        hpBarSlider.value = hpRatio;
    }
}
