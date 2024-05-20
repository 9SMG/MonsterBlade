using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyHpUI_2 : MonoBehaviour
{
    private float _Php;

    public void PartyHpBar()
    {
        Image PartyImg = GameObject.FindGameObjectsWithTag("PartyHp")[1].GetComponent<Image>();

        if (PartyImg != null)
        {
            _Php = 45.0f;
            PartyImg.fillAmount = _Php / 100.0f;
        }
    }

}
