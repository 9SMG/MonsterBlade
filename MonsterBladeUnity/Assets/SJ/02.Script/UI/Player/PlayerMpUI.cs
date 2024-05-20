using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMpUI : MonoBehaviour
{
    [SerializeField] private Image Mp;
    [SerializeField] private Text MpText;

    // 마나
    private float currentMp;
    private float maxMp = 100f;

    public void PlayerMpBar()
    {
        Image MpBarImg = GameObject.FindGameObjectsWithTag("State")[1].GetComponent<Image>();

        if (MpBarImg != null)
        {
            float fillAmount = 30.0f / 100.0f; // 임시 값으로 설정
            MpBarImg.fillAmount = fillAmount; // 체력에 맞게 fillAmount 설정

            // 마나 텍스트 업데이트
            MpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    // 체력 UI 업데이트
    public void UpdateMpUI()
    {
        float fillAmount = currentMp / maxMp;
        Mp.fillAmount = fillAmount;
        MpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }
}
