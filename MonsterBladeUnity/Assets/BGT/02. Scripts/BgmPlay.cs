using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlay : MonoBehaviour
{
    float soundLength;
    bool bgmEnd;

    void Awake()
    {
        bgmEnd = false;
    }

    void Start()
    {
        soundLength = SoundManager.Instance.GetClip("BGM_In a good place 01").length;
        StartCoroutine(BgmStart());
    }

    IEnumerator BgmStart()
    {
        yield return new WaitUntil(() => soundLength > 0f); // soundLength가 설정될 때까지 대기

        while (true)
        {
            if (!bgmEnd)
            {
                bgmEnd = true;
                SoundManager.Instance.PlaySound2D("BGM_In a good place " + SoundManager.Range(1, 1, true));
            }

            yield return new WaitForSeconds(soundLength);

            bgmEnd = false;
        }
    }
}
