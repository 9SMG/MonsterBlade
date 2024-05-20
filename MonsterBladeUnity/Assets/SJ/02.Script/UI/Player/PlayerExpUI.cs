using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpUI : MonoBehaviour
{
    // 경험치
    private float _Exp;

    public void PlayerExpBar()
    {
        Image ExpBarImg = GameObject.FindGameObjectWithTag("Exp").GetComponent<Image>();

        if (ExpBarImg != null)
        {
            _Exp = 70.0f; // 체력을 40으로 설정
            ExpBarImg.fillAmount = _Exp / 100.0f; // 체력에 맞게 fillAmount 설정
        }
    }
}
