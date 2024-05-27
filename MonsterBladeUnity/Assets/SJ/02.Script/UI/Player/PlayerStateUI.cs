using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    // ����ü��, �ִ�ü��
    public float curHpUI;
    public float maxHpUI = 100.0f;

    // ���縶��, �ִ븶��
    public float curMpUI;
    public float maxMpUI;

    // ���� Sp, �ִ�Sp
    public float curSpUI;
    public float maxSpUI;

    // ���� ����ġ, �ִ� ������
    public float curExpUI;
    public float maxExpUI;

    // �÷��̾� ����
    public float playerLevel;

    private Image[] stateBars;
    private Text[] stateTexts;

    private void Start()
    {
        curHpUI = maxHpUI;
        curMpUI = maxMpUI;
        curSpUI = maxSpUI;
        curExpUI = 0;  // ����ġ�� ���� 0���� �����մϴ�.

        Debug.Log($"Initial HP: {curHpUI}/{maxHpUI}");
        Debug.Log($"Initial MP: {curMpUI}/{maxMpUI}");
        Debug.Log($"Initial SP: {curSpUI}/{maxSpUI}");
        Debug.Log($"Initial EXP: {curExpUI}/{maxExpUI}");

        stateBars = GameObject.FindGameObjectsWithTag("State").Select(go => go.GetComponent<Image>()).ToArray();
        stateTexts = GameObject.FindGameObjectsWithTag("pUIText").Select(go => go.GetComponent<Text>()).ToArray();

        stateBars = new Image[4];
        stateBars[0] = GameObject.Find("HpUI").GetComponent<Image>(); // ��: HpBar ������Ʈ
        stateBars[1] = GameObject.Find("MpUI").GetComponent<Image>(); // ��: MpBar ������Ʈ
        stateBars[2] = GameObject.Find("SpUI").GetComponent<Image>(); // ��: SpBar ������Ʈ
        stateBars[3] = GameObject.Find("ExpUI").GetComponent<Image>(); // ��: ExpBar ������Ʈ

        stateTexts = new Text[2];
        stateTexts[0] = GameObject.Find("HpPercent").GetComponent<Text>(); // ��: HpText ������Ʈ
        stateTexts[1] = GameObject.Find("MpPercent").GetComponent<Text>(); // ��: MpText ������Ʈ
        //stateTexts[2] = GameObject.Find("ExpText").GetComponent<Text>(); // ��: ExpText ������Ʈ

        UpdateAllUI();
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

            Debug.Log($"HP Fill Amount: {fillAmount}"); // ����� �α� �߰�
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
            Debug.Log($"Exp Fill Amount: {fillAmount}");  // ����� �α� �߰�
        }
    }

    private void UpdateAllUI()
    {
        UpdateHpUI();
        UpdateMpUI();
        UpdateSpUI();
        UpdateExpUI();
    }

    // ���ݽ� ü�°���
    public void TakeDamage(float damage)
    {
        curHpUI -= damage;
        curHpUI = Mathf.Clamp(curHpUI, 0, maxHpUI);
        UpdateHpUI();
    }

    // ������ �Ҹ��ϴ� ��ų ��� �޼���
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
        Debug.Log($"Gained Experience: {exp}, Current Exp: {curExpUI}/{maxExpUI}"); // ����� �α� �߰�
        if (curExpUI >= maxExpUI)
        {
            curExpUI -= maxExpUI;
            PlayerLevelUp();
        }
        UpdateExpUI();
    }

    private void PlayerLevelUp()
    {
        // �÷��̾� ���� ����
        playerLevel++;
        // ���� ����ġ �ʱ�ȭ
        curExpUI = 0;
        // �ִ� ü�� ����
        maxHpUI += 20;
        curHpUI = maxHpUI;
        SetHp(curHpUI, maxHpUI);
        // �ִ� ����ġ ����
        maxExpUI *= 1.5f;
        // ����ġ �ؽ�Ʈ ������Ʈ
        Debug.Log("Level Up! New Level: " + playerLevel + ", New Max HP: " + maxHpUI);
    }
}
