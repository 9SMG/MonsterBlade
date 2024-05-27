using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    public float curHpUI;
    public float maxHpUI = 100.0f;

    public float curMpUI;
    public float maxMpUI;

    public float curSpUI;
    public float maxSpUI;

    public float curExpUI;
    public float maxExpUI;

    private Image[] stateBars;
    private Text[] stateTexts;

    private void Start()
    {
        Debug.Log($"Initial HP: {curHpUI}/{maxHpUI}");
        Debug.Log($"Initial MP: {curMpUI}/{maxMpUI}");
        Debug.Log($"Initial SP: {curSpUI}/{maxSpUI}");
        Debug.Log($"Initial EXP: {curExpUI}/{maxExpUI}");

        stateBars = GameObject.FindGameObjectsWithTag("State").Select(go => go.GetComponent<Image>()).ToArray();
        stateTexts = GameObject.FindGameObjectsWithTag("pUIText").Select(go => go.GetComponent<Text>()).ToArray();

        stateBars = new Image[4];
        stateBars[0] = GameObject.Find("HpUI").GetComponent<Image>(); // 예: HpBar 오브젝트
        stateBars[1] = GameObject.Find("MpUI").GetComponent<Image>(); // 예: MpBar 오브젝트
        stateBars[2] = GameObject.Find("SpUI").GetComponent<Image>(); // 예: SpBar 오브젝트
        stateBars[3] = GameObject.Find("ExpUI").GetComponent<Image>(); // 예: ExpBar 오브젝트

        stateTexts = new Text[2];
        stateTexts[0] = GameObject.Find("HpPercent").GetComponent<Text>(); // 예: HpText 오브젝트
        stateTexts[1] = GameObject.Find("MpPercent").GetComponent<Text>(); // 예: MpText 오브젝트
        //stateTexts[2] = GameObject.Find("ExpText").GetComponent<Text>(); // 예: ExpText 오브젝트

        UpdateAllUI();
    }

    // 공격시 체력감소
    public void TakeDamage(float damage)
    {
        curHpUI -= damage;
        curHpUI = Mathf.Clamp(curHpUI, 0, maxHpUI);
        UpdateHpUI();
    }

    public void SetHp(float curHpUI, float maxHpUI)
    {
        this.curHpUI = curHpUI;
        this.maxHpUI = maxHpUI;
        UpdateHpUI();
    }

    public void SetMp(float curMpUI, float maxMpUI)
    {
        this.curMpUI = curMpUI;
        this.maxMpUI = maxMpUI;
        UpdateMpUI();
    }

    public void SetSp(float curSpUI, float maxSpUI)
    {
        this.curSpUI = curSpUI;
        this.maxSpUI = maxSpUI;
        UpdateSpUI();
    }

    public void SetExp(float curExpUI, float maxExpUI)
    {
        this.curExpUI = curExpUI;
        this.maxExpUI = maxExpUI;
        UpdateExpUI();
    }

    private void UpdateHpUI()
    {
        if (stateBars.Length > 0 && stateTexts.Length > 0)
        {

            float fillAmount = curHpUI / maxHpUI;
            stateBars[0].fillAmount = fillAmount;
            stateTexts[0].text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    private void UpdateMpUI()
    {
        if (stateBars.Length > 1 && stateTexts.Length > 1)
        {
            float fillAmount = curMpUI / maxMpUI;
            stateBars[1].fillAmount = fillAmount;
            stateTexts[1].text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    private void UpdateSpUI()
    {
        if (stateBars.Length > 2)
        {
            float fillAmount = curSpUI / maxSpUI;
            stateBars[2].fillAmount = fillAmount;
        }
    }

    private void UpdateExpUI()
    {
        if (stateBars.Length > 3)
        {
            float fillAmount = curExpUI / maxExpUI;
            stateBars[3].fillAmount = fillAmount;
            //stateTexts[2].text = Mathf.RoundToInt(fillAmount * 100f) + "%";
        }
    }

    private void UpdateAllUI()
    {
        UpdateHpUI();
        UpdateMpUI();
        UpdateSpUI();
        UpdateExpUI();
    }

    // 마나만 소모하는 스킬 사용 메서드
    public void UseSkill(float manaCost)
    {
        if (curMpUI >= manaCost)
        {
            curMpUI -= (int)manaCost;
            SetMp(curMpUI, maxMpUI);
            Debug.Log("Skill used, remaining mana: " + curMpUI);
        }
        else
        {
            Debug.Log("Not enough mana to use skill.");
        }
    }

    public void GainExperience(float exp)
    {
        curExpUI += exp;
        if (curExpUI >= maxExpUI)
        {
            curExpUI -= maxExpUI;
            // 레벨업 처리 (레벨업 시 필요한 추가 로직을 여기에 작성)
            LevelUp();
        }
        UpdateExpUI();
    }

    private void LevelUp()
    {
        // 레벨업 시 필요한 로직 (예: 플레이어의 레벨 증가, 최대 경험치 증가 등)
        // 예시:
        maxExpUI *= 1.1f; // 다음 레벨업에 필요한 경험치를 증가시킴
                          // 레벨업 애니메이션이나 사운드 효과 추가 가능
    }
}
