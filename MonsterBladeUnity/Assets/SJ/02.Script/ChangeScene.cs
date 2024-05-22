using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        LoadingSceneManager.LoadScene("GameLobby");
    //    }
    //}
    public void SceneChange()
    {
        //LoadingSceneManager.LoadScene("GameLobby");
        MonsterBlade.Manager.LoadManager.Instance.LoadLobby();


    }
}
