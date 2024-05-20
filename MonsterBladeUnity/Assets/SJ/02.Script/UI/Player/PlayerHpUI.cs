using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    [SerializeField] private Image Hp;       // 체력바 이미지 
    [SerializeField] private Text HpText; // 체력 텍스트

    private float currentHp;
    private float maxHp = 100f;

    private void Start()
    {
        currentHp = maxHp;
        UpdateHpUI();
        //HpPercent.text = Hp.fillAmount.ToString();
    }

    public void PlayerHpBar()
    {
        Image HpBarImg = GameObject.FindGameObjectsWithTag("State")[0].GetComponent<Image>();

        if (HpBarImg != null)
        {
            float fillAmount = 40.0f / 100.0f; // 임시 값으로 설정
            HpBarImg.fillAmount = fillAmount; // 체력에 맞게 fillAmount 설정

            // 체력 텍스트 업데이트
            HpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    // 체력 UI 업데이트
    public void UpdateHpUI()
    {
        float fillAmount = currentHp / maxHp;
        Hp.fillAmount = fillAmount;
        HpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }


}