using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    void Start()
    {
       LoadUIScene();
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    void LoadUIScene()
    {
        // 현재 "UI" 씬을 로드했는지 확인 후, 이미 로드된 상태라면 추가적으로 로드하지 않음
        if (!SceneManager.GetSceneByName("UI").isLoaded)
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }
}
