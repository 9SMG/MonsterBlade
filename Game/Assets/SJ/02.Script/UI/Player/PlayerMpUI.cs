using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMpUI : MonoBehaviour
{
    // 마나
    private float _Mp;

    public void PlayerMpBar()
    {
        Image MpBarImg = GameObject.FindGameObjectsWithTag("State")[1].GetComponent<Image>();

        if (MpBarImg != null)
        {
            _Mp = 30.0f; // 체력을 40으로 설정
            MpBarImg.fillAmount = _Mp / 100.0f; // 체력에 맞게 fillAmount 설정
        }
    }
}
