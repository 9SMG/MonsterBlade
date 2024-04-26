using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayNightTimeCheck : MonoBehaviour
{
    //[Header("스카이 박스3개, Light 넣기")]
    [SerializeField] Material skybox1;
    [SerializeField] Material skybox2;
    [SerializeField] Material skybox3;
    [SerializeField] GameObject Lamp;

    [SerializeField] GameObject directionalLight;
    int hours;

    // Start is called before the first frame update
    void Start()
    {
        hours = DateTime.Now.Hour;
        StartCoroutine("HourCheck");
    }
    
    IEnumerator HourCheck()
    {
        bool first = true;
        int minus_Time = 60 - DateTime.Now.Minute;
        while(true)
        {
            hours = DateTime.Now.Hour;
            Check_Enviroment();
            if(first)
            {
                yield return new WaitForSeconds(minus_Time * 60);
                first = false;
            }
            yield return new WaitForSeconds(3600f);
        }
    }
    void Check_Enviroment()
    {
        //18시~ 4시까지 가로등 on
        if (hours >= 18 || hours <= 4)
        {
            Lamp.SetActive(true);
        }   
        else
        {
            Lamp.SetActive(false);
        }

        // 아침 // 확인하고 18시로 변경
        if(hours >= 6 && hours <=15)
        {
            directionalLight.SetActive(true);
            RenderSettings.skybox = skybox1;
        }
        // 새벽, 해지기 전 확인후 앞에 시간 18로 변경
        else if((hours >= 16 && hours <= 21) || (hours >= 4 && hours <= 6))
        {
            directionalLight.SetActive(false);
            RenderSettings.skybox = skybox2;
        }
        // 밤
        else if (hours >= 21 || hours <= 4)
        {
            directionalLight.SetActive(false);
            RenderSettings.skybox = skybox3;
        }
    }
}
