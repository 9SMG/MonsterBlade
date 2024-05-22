using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BgmPlay : MonoBehaviour
{
    float soundLength;
    bool bgmEnd;
    string currentSceneName;
    Dictionary<string, string> sceneBgmMap;

    void Awake()
    {
        bgmEnd = false;
        sceneBgmMap = new Dictionary<string, string>
        {
            { "Start", "StartBgm" },
            { "GameLobby", "LobbyBgm" },
            { "Map", "MainBgm" }
        };

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        PlayBGMForScene(currentSceneName);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log("Previous Scene: " + previousScene.name + ", New Scene: " + newScene.name);

        if (previousScene.IsValid())
        {
            StopAllCoroutines();

            if (sceneBgmMap.ContainsKey(previousScene.name))
            {
                SoundManager.Instance.StopLoopSound(sceneBgmMap[previousScene.name]);
            }
            else
            {
                Debug.LogWarning("No BGM found for previous scene: " + previousScene.name);
            }
        }

        PlayBGMForScene(newScene.name);
    }

    void PlayBGMForScene(string sceneName)
    {
        if (sceneBgmMap.ContainsKey(sceneName))
        {
            string bgmName = sceneBgmMap[sceneName];
            soundLength = SoundManager.Instance.GetClip(bgmName).length;
            StartCoroutine(BgmStart(bgmName));
        }
        else
        {
            Debug.LogWarning("No BGM found for scene: " + sceneName);
        }
    }

    IEnumerator BgmStart(string bgmName)
    {
        yield return new WaitUntil(() => soundLength > 0f);

        while (true)
        {
            if (!bgmEnd)
            {
                bgmEnd = true;
                SoundManager.Instance.PlaySound2D(bgmName, 0f, true, SoundType.BGM);
            }

            yield return new WaitForSeconds(soundLength);

            bgmEnd = false;
        }
    }
}
