using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    // 플레이어 체럭
    public float curHpUI;
    public float maxHpUI;

    // 플레이어 마나
    public float curMpUI;
    public float maxMpUI;
    // 플레이어 기력
    public float curSpUI;
    public float maxSpUI;
    // 플레이어 경험치
    public float curExpUI;
    public float maxExpUI;

    private void Start()
    {
        curHpUI  = maxHpUI;
        curMpUI  = maxMpUI;
        curSpUI  = maxSpUI;
        curExpUI = maxExpUI;
    }

    private void Update()
    {
        SetHp(90,100);
        SetMp(60, 100);
        SetSp(10, 100);
        SetExp(10, 100);
    }
    public void TakeDamage(float damage)
    {
        curHpUI -= damage;
        curHpUI = Mathf.Clamp(curHpUI, 100, maxHpUI);
    }

    // 플레이어 체력 관리
    public void SetHp(float curHpUI, float maxHpUI)
    {
        this.curHpUI = curHpUI;
        this.maxHpUI = maxHpUI;
        PlayerHpBar();
    }
    public void SetMp(float curMpUI, float maxMpUI)
    {
        this.curMpUI = curMpUI;
        this.maxMpUI = maxMpUI;
        PlayerMpBar();
    }
    public void SetSp(float curSpUI, float maxSpUI)
    {
        this.curSpUI = curSpUI;
        this.maxSpUI = maxSpUI;
        PlayerSpBar();
    }
    public void SetExp(float curExpUI, float maxExpUI)
    {
        this.curExpUI = curExpUI;
        this.maxExpUI = maxExpUI;
        PlayerExpBar();
    }
    public void PlayerHpBar()
    {
        Image HpBarImg = GameObject.FindGameObjectsWithTag("State")[0].GetComponent<Image>();
        Text HpText = GameObject.FindGameObjectsWithTag("pUIText")[0].GetComponent<Text>();
        if (HpBarImg != null)
        {
            float fillAmount = curHpUI / maxHpUI;
            HpBarImg.fillAmount = fillAmount; // 체력에 맞게 fillAmount 설정

            // 체력 텍스트 업데이트
            HpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    public void PlayerMpBar()
    {
        Image MpBarImg = GameObject.FindGameObjectsWithTag("State")[1].GetComponent<Image>();
        Text MpText = GameObject.FindGameObjectsWithTag("pUIText")[1].GetComponent<Text>();
        if (MpBarImg != null)
        {
            float fillAmount = curMpUI / maxMpUI;
            MpBarImg.fillAmount = fillAmount; // 마나에 맞게 fillAmount 설정

            // 마나 텍스트 업데이트
            MpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    public void PlayerSpBar()
    {
        Image SpBarImg = GameObject.FindGameObjectsWithTag("State")[2].GetComponent<Image>();
        if (SpBarImg != null)
        {
            float fillAmount = curSpUI / maxSpUI;
            SpBarImg.fillAmount = fillAmount; // 기력에 맞게 fillAmount 설정
        }
    }

    public void PlayerExpBar()
    {
        Image ExpBarImg = GameObject.FindGameObjectsWithTag("State")[3].GetComponent<Image>();
        Text ExpText = GameObject.FindGameObjectsWithTag("pUIText")[2].GetComponent<Text>();
        if (ExpBarImg != null)
        {
            float fillAmount = curExpUI / maxExpUI;
            ExpBarImg.fillAmount = fillAmount; // 경험치에 맞게 fillAmount 설정

            // 경험치 텍스트 업데이트
            ExpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }
}