using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private Image bHp;       // 체력바 이미지 
    [SerializeField] private Text bHpText; // 체력 텍스트

    private float b_currentHp;
    private float b_maxHp = 100f;

    private void Start()
    {
        b_currentHp = b_maxHp;
        UpdateBossHpUI();
        //HpPercent.text = Hp.fillAmount.ToString();

    }

    public void BossUIHpBar()
    {
        Image bHpBarImg = GameObject.FindGameObjectWithTag("BossUI").GetComponent<Image>();

        if (bHpBarImg != null)
        {
            float fillAmount = 40.0f / 100.0f; // 임시 값으로 설정
            bHpBarImg.fillAmount = fillAmount; // 체력에 맞게 fillAmount 설정

            // 체력 텍스트 업데이트
            bHpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    // 체력 UI 업데이트
    public void UpdateBossHpUI()
    {
        float fillAmount = b_currentHp / b_maxHp;
        bHp.fillAmount = fillAmount;
        bHpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }


}