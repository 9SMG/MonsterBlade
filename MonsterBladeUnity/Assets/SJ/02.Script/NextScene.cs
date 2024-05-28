using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using LoadManager = MonsterBlade.Manager.LoadManager;

public class NextScene : MonoBehaviour
{

    public void SceneChange()
    {
        //PassSceneManager.LoadScene("Map");
        LoadManager.Instance.LoadStage();
    }

    public void BackScene()
    {
        LoadManager.Instance.LoadTitle();
    }
}
