using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    // 프레임 드랍 시, 이펙트가 제대로 보이지 않음
    // Update() -> Couroutine
    //void Update()
    //{
    //    Color color = image.color;

    //    if (color.a > 0)
    //    {
    //        color.a -= Time.deltaTime;
    //        Debug.Log(color.a);
    //    }
    //    image.color = color;
    //}

    public void ResetEffect()
    {
        Color color = image.color;

        color.a = 1f;

        image.color = color;
    }

    public void StartEffect()
    {
        StartCoroutine(PlayEffect());
    }

    IEnumerator PlayEffect()
    {
        yield return null;
        Color color = image.color;

        for(int i = 0; i <= 50; i ++)
        {
            color.a = 1f - (0.02f * i);
            image.color = color;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
