using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatusText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textUI;

    public TextMeshProUGUI TextUI
    {
        get { return textUI; }
    }

    //action을 불러오기
    public void StatusTextChange(StatusTextEvent statusText)
    {
        textUI.text = statusText.statusText;
    }
}
