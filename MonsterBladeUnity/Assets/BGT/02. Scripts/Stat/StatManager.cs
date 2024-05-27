using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    public bool StatMenu = false;

    public StatInfo statInfo;
    [SerializeField] private TextMeshProUGUI mLvLabel;
    [SerializeField] private TextMeshProUGUI mStatPtLabel;
    [SerializeField] private TextMeshProUGUI mAttackCurrentLabel;
    [SerializeField] private TextMeshProUGUI mSpeedCurrentLabel;
    [SerializeField] private TextMeshProUGUI mDefenseCurrentLabel;
    [SerializeField] private TextMeshProUGUI mHpMaxLabel;
    [SerializeField] private TextMeshProUGUI mMpMaxLabel;
    [SerializeField] private TextMeshProUGUI mStaminaMaxLabel;
    [SerializeField] private GameObject[] mStatButtons;
    [SerializeField] private GameObject mStatUi;

    Arrow arrow;
    Skill skill;
    int CurrentStatPoint = 1;

    void Awake()
    {
        arrow = GetComponent<Arrow>();
        skill = GetComponent<Skill>();
        statInfo.InitStatData();
    }

    void Update()
    {
        StartCoroutine(OpenStat());
        if (Input.GetKey(KeyCode.F2))
        {
            CurrentStatPoint += 100;
        }
    }

    public void LevelUp()
    {
        statInfo.UpgradeBaseStat(StatType.LEVEL);
        CurrentStatPoint += 2;
        UpdateStatTexts();
    }

    IEnumerator OpenStat()
    {
        if (Input.GetKey(KeyCode.P))
        {
            if (!StatMenu)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                mStatUi.SetActive(true);
                UpdateStatTexts();
                yield return new WaitForSeconds(0.2f);
                StatMenu = true;
            }
            else if(StatMenu)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                mStatUi.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                StatMenu = false;
            }
        }
    }

    public void UpdateStatTexts()
    {
        foreach (GameObject go in mStatButtons)
            go.SetActive(CurrentStatPoint > 0);

        mLvLabel.text = statInfo.level.ToString();
        mStatPtLabel.text = CurrentStatPoint.ToString();
        mHpMaxLabel.text = statInfo.hpMax.ToString();
        mMpMaxLabel.text = statInfo.mpMax.ToString();
        mStaminaMaxLabel.text = statInfo.staminaMax.ToString();
        mAttackCurrentLabel.text = statInfo.Attack.ToString();
        mSpeedCurrentLabel.text = statInfo.MovementSpeed.ToString();
        mDefenseCurrentLabel.text = statInfo.Defense.ToString();
    }

    public void BTN_UpgradeStat(int statIndex)
    {
        if (CurrentStatPoint <= 0) return;
        CurrentStatPoint--;
        statInfo.UpgradeBaseStat((StatType)statIndex);
        UpdateStatTexts();
        arrow.damage = statInfo.Attack;
        skill.damage += statInfo.Attack;
    }
}