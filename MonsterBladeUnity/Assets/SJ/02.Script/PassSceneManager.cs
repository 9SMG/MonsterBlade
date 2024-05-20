using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PassSceneManager : MonoBehaviour
{
    public static string next2Scene;

    [SerializeField]
    Image progressBar;

    //private float time = 0f;
    public static void LoadScene(string sceneName)
    {
        next2Scene = sceneName;
        SceneManager.LoadScene("MapLoading");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneProcess1());
    }

    IEnumerator LoadSceneProcess1()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(next2Scene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (timer < 5f)
        {
            // 현재 로딩 진행 상황을 계산합니다.
            float progress = Mathf.Clamp01(timer / 5f);

            // 로딩 진행 상황을 표시합니다.
            progressBar.fillAmount = progress;

            timer += Time.deltaTime;
            yield return null;
        }
        op.allowSceneActivation = true;
    }
}