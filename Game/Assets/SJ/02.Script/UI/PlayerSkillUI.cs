using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    private float playerSkiUI;

    public void PlayerSkiUIBar()
    {
        Image PlayerSkiUIimg = GameObject.FindGameObjectsWithTag("PlayerSki")[0].GetComponent<Image>();
        
        if (PlayerSkiUIimg != null)
        {
            playerSkiUI = 35.0f;
            PlayerSkiUIimg.fillAmount = playerSkiUI / 100.0f;
        }
    }
}
