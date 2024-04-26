using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    // 체력
    private float _Hp;

    public void PlayerHpBar()
    {
        Image HpBarImg = GameObject.FindGameObjectsWithTag("State")[0].GetComponent<Image>();

        if (HpBarImg != null)
        {
            _Hp = 40.0f; // 체력을 40으로 설정
            HpBarImg.fillAmount = _Hp / 100.0f; // 체력에 맞게 fillAmount 설정
        }
    }
}
