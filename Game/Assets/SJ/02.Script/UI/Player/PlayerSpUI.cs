using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpUI : MonoBehaviour
{
    // 체력
    private float _Sp;

    public void PlayerSpBar()
    {
        Image SpBarImg = GameObject.FindGameObjectsWithTag("State")[2].GetComponent<Image>();

        if (SpBarImg != null)
        {
            _Sp = 90.0f; // 체력을 40으로 설정
            SpBarImg.fillAmount = _Sp / 100.0f; // 체력에 맞게 fillAmount 설정
        }
    }
}
