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

    // ������ ��� ��, ����Ʈ�� ����� ������ ����
    //void Update()
    //{
    //    Color color = image.color;

    //    if( color.a > 0)
    //    {
    //        color.a -= Time.deltaTime;
    //    }
    //    image.color = color;
    //}

    public void StartEffect()
    {
        StartCoroutine(PlayEffect());
    }

    IEnumerator PlayEffect()
    {
        yield return null;
        Color color = image.color;

        for (int i = 0; i <= 50; i++)
        {
            color.a = 1f - (0.02f * i);
            image.color = color;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
