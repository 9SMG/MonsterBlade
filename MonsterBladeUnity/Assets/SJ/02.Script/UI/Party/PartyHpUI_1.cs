using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyHpUI_1 : MonoBehaviour
{
    [SerializeField] private Image _pHp;
    [SerializeField] private Text _pHpText;

    private float _pCurrentHp;
    private float _pMaxHp = 100f;

    public void PartyHpBar()
    {
        Image _partyImg = GameObject.FindGameObjectsWithTag("PartyHp")[0].GetComponent<Image>();

        if(_partyImg != null)
        {
            float fillAmount = 45.0f / 100.0f;
            _partyImg.fillAmount = fillAmount;

            _pHpText.text = Mathf.RoundToInt(fillAmount * 100.0f) + "%";
        }
    }

    public void UpdateMpUI()
    {
        float fillAmount = _pCurrentHp / _pMaxHp;
        _pHp.fillAmount = fillAmount;
        _pHpText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }
}
