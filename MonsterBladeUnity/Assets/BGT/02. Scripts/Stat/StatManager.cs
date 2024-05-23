using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    public static bool IsStatDialogEnable { private set; get; } = false;

    [SerializeField] private StatInfo statInfo;
    [SerializeField] private TextMeshProUGUI mLvLabel;
    [SerializeField] private TextMeshProUGUI mStatPtLabel;
    [SerializeField] private TextMeshProUGUI mHpCurrentLabel;
    [SerializeField] private TextMeshProUGUI mMpCurrentLabel;
    [SerializeField] private TextMeshProUGUI mStaminaCurrentLabel;
    [SerializeField] private TextMeshProUGUI mAttackCurrentLabel;
    [SerializeField] private TextMeshProUGUI mSpeedCurrentLabel;
    [SerializeField] private TextMeshProUGUI mDefenseCurrentLabel;
    [SerializeField] private TextMeshProUGUI mHpMaxLabel;
    [SerializeField] private TextMeshProUGUI mMpMaxLabel;
    [SerializeField] private TextMeshProUGUI mStaminaMaxLabel;
    [SerializeField] private GameObject[] mStatButtons;
    [SerializeField] private GameObject mStatUiGo;

    private int CurrentStatPoint = 1;

    private void Awake()
    {
        IsStatDialogEnable = false;
        statInfo.InitStatData();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            ToggleStatMenu();
        }
    }

    public void LevelUp()
    {
        statInfo.UpgradeBaseStat(StatType.LEVEL);
        CurrentStatPoint += 2;
        UpdateStatTexts();
    }

    public void ToggleStatMenu()
    {
        if (mStatUiGo.activeSelf)
            TryCloseStatDialog();
        else
            TryOpenStatDialog();
    }

    private void TryOpenStatDialog()
    {
        Cursor.lockState = CursorLockMode.Confined;
        mStatUiGo.SetActive(true);
        IsStatDialogEnable = true;
        transform.SetAsLastSibling();
        UpdateStatTexts();
    }

    private void TryCloseStatDialog()
    {
        IsStatDialogEnable = false;
        Cursor.lockState = CursorLockMode.Locked;
        mStatUiGo.SetActive(false);
    }

    public void UpdateStatTexts()
    {
        foreach (GameObject go in mStatButtons)
            go.SetActive(CurrentStatPoint > 0);

        mLvLabel.text = statInfo.level.ToString();
        mStatPtLabel.text = CurrentStatPoint.ToString();
        mHpCurrentLabel.text = statInfo._curHP.ToString();
        mMpCurrentLabel.text = statInfo._curMP.ToString();
        mStaminaCurrentLabel.text = statInfo._curStamina.ToString();
        mHpMaxLabel.text = $"/{statInfo.hpMax}";
        mMpMaxLabel.text = $"/{statInfo.mpMax}";
        mStaminaMaxLabel.text = $"/{statInfo.staminaMax}";
        mAttackCurrentLabel.text = statInfo.baseAttack.ToString();
        mSpeedCurrentLabel.text = statInfo.baseMovementSpeed.ToString();
        mDefenseCurrentLabel.text = statInfo.baseDefense.ToString();
    }

    public void BTN_UpgradeStat(int statIndex)
    {
        if (CurrentStatPoint <= 0) return;
        CurrentStatPoint--;
        statInfo.UpgradeBaseStat((StatType)statIndex);
        UpdateStatTexts();
    }
}