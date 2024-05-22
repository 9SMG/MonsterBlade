using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using MonsterBlade.Manager;

public class MoveScene : MonoBehaviour
{
    void Start()
    {
        //LoadUIScene();
        if(!LoadManager.IsLoadscMng)
            LoadManager.Instance.LoadStage();
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    void LoadUIScene()
    {
        // ���� "UI" ���� �ε��ߴ��� Ȯ�� ��, �̹� �ε�� ���¶�� �߰������� �ε����� ����
        if (!SceneManager.GetSceneByName("UI").isLoaded)
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }
}
