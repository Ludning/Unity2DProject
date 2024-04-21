using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    // 렌더링을 비활성화할 최대 거리
    [SerializeField]
    float cullingDistance = 10f;

    Canvas textCanvas;
    Canvas sliderCanvas;
    Canvas iconCanvas;

    public void SetUpHpUI()
    {
        textCanvas = transform.GetChild(0).GetComponent<Canvas>();
        sliderCanvas = transform.GetChild(1).GetComponent<Canvas>();
        iconCanvas = transform.GetChild(2).GetComponent<Canvas>();

        textCanvas.sortingLayerName = "Information_Text";
        sliderCanvas.sortingLayerName = "Information_Slider";
        iconCanvas.sortingLayerName = "Information_Icon";

        StopAllCoroutines();
        StartCoroutine(Culling());
    }

    IEnumerator Culling()
    {
        while (true)
        {
            if (GameManager.Instance.player == null)
            {
                yield return new WaitForSeconds(0.3f);
                continue;
            }
            Vector2 distance = transform.position - GameManager.Instance.player.transform.position;
            if (distance.sqrMagnitude < cullingDistance * cullingDistance)
            {
                textCanvas.enabled = true;
                sliderCanvas.enabled = true;
                iconCanvas.enabled = true;
            }
            else
            {
                textCanvas.enabled = false; ;
                sliderCanvas.enabled = false;
                iconCanvas.enabled = false; ;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
