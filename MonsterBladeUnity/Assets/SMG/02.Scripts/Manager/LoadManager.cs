using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

using MonsterBlade.Manager;

namespace MonsterBlade.Manager
{
    public class LoadManager : MonoSingleton<LoadManager>
    {
        // Manager Scene
        public static readonly string scnManager = "scManager";

        // Title
        public static readonly string scnStart = "Start";

        // Lobby
        public static readonly string scnGameLobby = "GameLobby";

        // In Game
        public static readonly string scnMap = "Map";
        public static readonly string scnUI = "UI";
        public static readonly string scnPhotonLobby = "PhotonLobby";

        // Loading Canvas
        public Canvas loadingCanvas;
        public Text gameLogo;
        public Image progressBar;

        public FadeEffect fadeEffectImage;

        //[SerializeField]
        //bool nowLoading;

        public List<GameObject> backGroundViews = new List<GameObject>();

        public static bool IsLoadscMng
        {
            get { return SceneManager.GetSceneByName(scnManager).isLoaded; }
        }


        // Start is called before the first frame update
        void Start()
        {
            //nowLoading = false;
            if (SceneManager.GetActiveScene().name == scnManager)
            {
                LoadTitle();
                //LoadLobby();
                //LoadStage();
            }
        }

        ///// Loading /////
        IEnumerator Loading(IEnumerator loadRoutine, bool bShowLogo = false)
        {
            //if(nowLoading)
            //{
            //    Debug.Log("Using Loading: " + loadRoutine.ToString());
            //    yield break;
            //}
            //nowLoading = true;
            Debug.Log("Loading(" + loadRoutine.ToString() + ")");

            // Loading Canvas Init
            ResetLoadingCanvas();

            // Visible Loading Canvas
            ShowLoadingCanavas(true);
            ShowGameLogo(bShowLogo);

            // Unload
            UnloadAll();
            yield return new WaitForSeconds(1f);

            // Start LoadSceneAsync

            SetProgressBar(0f);
            yield return loadRoutine;

            // End LoadSceneAsync
            SetProgressBar(1f);
            //nowLoading = false;

            yield return new WaitForSeconds(1f);
            ShowLoadingCanavas(false);
        }

        void UnloadAll()
        {
            UnloadTitle();
            UnloadLobby();
            UnloadStage();
        }

        IEnumerator WaitLoadAndActiveScene(string activeSceneName)
        {
            Scene scene = SceneManager.GetSceneByName(activeSceneName);
            while (!scene.isLoaded)
                yield return null;

            SceneManager.SetActiveScene(scene);
        }

        ///// Title /////
        public void LoadTitle()
        {
            StartCoroutine(LoadTitleCoroutine());

            //SceneManager.LoadScene(scnStart, LoadSceneMode.Additive);

            //while(!SceneManager.SetActiveScene(SceneManager.GetSceneByName(scnStart)));
            //StartCoroutine(DelayLoadScene(scnStart));
        }

        void UnloadTitle()
        {
            if (SceneManager.GetSceneByName(scnStart).isLoaded)
                SceneManager.UnloadSceneAsync(scnStart);
        }

        IEnumerator LoadTitleCoroutine()
        {
            ShowLoadingCanavas(false);
            UnloadAll();
            yield return null;

            SceneManager.LoadScene(scnStart, LoadSceneMode.Additive);

            yield return WaitLoadAndActiveScene(scnStart);
        }

        ///// Lobby /////
        public void LoadLobby()
        {
            StartCoroutine(Loading(LoadLobbyCoroutine(), true));
            //StartCoroutine(DelayLoadScene(scnGameLobby));
        }

        void UnloadLobby()
        {
            if (SceneManager.GetSceneByName(scnGameLobby).isLoaded)
                SceneManager.UnloadSceneAsync(scnGameLobby);
        }

        IEnumerator LoadLobbyCoroutine()
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(scnGameLobby, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                SetProgressBar(ao.progress);
                yield return null;
            }

            yield return WaitLoadAndActiveScene(scnGameLobby);
        }

        ///// Stage /////
        public void LoadStage()
        {
            StartCoroutine(Loading(LoadStageCoroutine()));
            //StartCoroutine(DelayLoadScene(scnMap, scnUI));
        }

        void UnloadStage()
        {
            if (!IsLoadscMng)
                return;
            if (SceneManager.GetSceneByName(scnMap).isLoaded)
                SceneManager.UnloadSceneAsync(scnMap);
            if (SceneManager.GetSceneByName(scnMap).isLoaded)
                SceneManager.UnloadSceneAsync(scnUI);
        }

        IEnumerator LoadStageCoroutine()
        {
            AsyncOperation ao;
            if (!SceneManager.GetSceneByName(scnMap).isLoaded)
            {
                ao = SceneManager.LoadSceneAsync(scnMap, LoadSceneMode.Additive);
                while (!ao.isDone)
                {
                    SetProgressBar(ao.progress / 3f);
                    yield return null;
                }
            }

            yield return WaitLoadAndActiveScene(scnMap);

            ao = SceneManager.LoadSceneAsync(scnUI, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                SetProgressBar((ao.progress + 1f) / 3f);
                yield return null;
            }

            ao = SceneManager.LoadSceneAsync(scnPhotonLobby, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                SetProgressBar((ao.progress + 2f) / 3f);
                yield return null;
            }
        }


        ///// Etc /////
        void ResetLoadingCanvas()
        {
            if (backGroundViews.Count <= 0)
                return;

            //Debug.Log("ResetLoadingCanvas()");

            // BackGround Setting
            foreach (GameObject bg in backGroundViews)
                bg.SetActive(false);

            int _idx = Random.Range(0, backGroundViews.Count);

            backGroundViews[_idx].SetActive(true);


            // ProgressBar Setting
            SetProgressBar(0f);
        }

        void ShowLoadingCanavas(bool bShow)
        {
            if (loadingCanvas == null)
                return;

            if (bShow)
                fadeEffectImage.StartEffect(); // fadeEffectImage.ResetEffect(); //

            loadingCanvas.enabled = bShow;
        }

        void SetProgressBar(float fillAmount)
        {
            if (progressBar == null)
                return;

            progressBar.fillAmount = fillAmount;
        }

        void ShowGameLogo(bool bShow)
        {
            if (gameLogo == null)
                return;

            gameLogo.enabled = bShow;
        }
    }
}

