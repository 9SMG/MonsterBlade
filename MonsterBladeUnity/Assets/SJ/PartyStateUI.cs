using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyStateUI : MonoBehaviour
{
    [SerializeField] private Image mHp;
    [SerializeField] private Text mText;

    public float mCurHpUI;
    public float mMaxHpUI;
    void Start()
    {
        mCurHpUI = mMaxHpUI;
        mHp = GameObject.FindGameObjectWithTag("mPartyHp").GetComponent<Image>();
        mText = GameObject.FindGameObjectWithTag("mPartyHpText").GetComponent<Text>(); ;
    }
    public void mPartyHpUI()
    {
        Image _mPartyHp = GameObject.FindGameObjectWithTag("mPartyHp").GetComponent<Image>();

        if (_mPartyHp != null)
        {
            float fillAmount = mCurHpUI / mMaxHpUI;
            _mPartyHp.fillAmount = fillAmount;

            mText.text = Mathf.RoundToInt(fillAmount * 100.0f) + "%";
        }
    }
    public void PartyTakeDamage(float damage)
    {
        mCurHpUI -= damage;
        mCurHpUI = Mathf.Clamp(mCurHpUI, 0, mMaxHpUI);
        UpdatePartyHpUI();
    }
    public void UpdatePartyHpUI()
    {
        float fillAmount = mCurHpUI / mMaxHpUI;
        mHp.fillAmount = fillAmount;
        mText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }
    public void PartySetHp(float mCurHpUI, float mMaxHpUI)
    {
        this.mCurHpUI = mCurHpUI;
        this.mMaxHpUI = mMaxHpUI;
        UpdatePartyHpUI();
    }
}
