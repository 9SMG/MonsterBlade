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
        //const string scnPhotonLobby;

        // Loading Canvas
        public Canvas loadingCanvas;
        public Text gameLogo;
        public Image progressBar;

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

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Loading(IEnumerator loadRoutine)
        {
            //if(nowLoading)
            //{
            //    Debug.Log("Using Loading: " + loadRoutine.ToString());
            //    yield break;
            //}
            //nowLoading = true;

            Debug.Log("Start Loading(IEnumerator loadRoutine)");

            // Loading Canvas Init
            ResetLoadingCanvas();

            // Visible Loading Canvas
            ShowLoadingCanavas(true);

            //Load Start
            SetProgressBar(0f);

            UnloadAll();
            yield return loadRoutine;

            // Load End
            SetProgressBar(1f);
            //nowLoading = false;

            ShowLoadingCanavas(false);

            Debug.Log("End Loading(IEnumerator loadRoutine)");
        }

        void UnloadAll()
        {
            UnloadTitle();
            UnloadLobby();
            UnloadStage();
        }

        public void LoadTitle()
        {
            SceneManager.LoadScene(scnStart, LoadSceneMode.Additive);
        }

        void UnloadTitle()
        {
            if(SceneManager.GetSceneByName(scnStart).isLoaded)
                SceneManager.UnloadSceneAsync(scnStart);
        }

        public void LoadLobby()
        {
            Debug.Log("LoadLobby()");
            StartCoroutine(Loading(LoadLobbyCoroutine()));
        }

        void UnloadLobby()
        {
            if (SceneManager.GetSceneByName(scnGameLobby).isLoaded)
                SceneManager.UnloadSceneAsync(scnGameLobby);
        }
        IEnumerator LoadLobbyCoroutine()
        {
            Debug.Log("Start LoadLobbyCoroutine()");
            //for (int i = 0; i < 100; i++)
            //{
            //    yield return new WaitForSeconds(0.1f);
            //    SetProgressBar(i * 0.01f);
            //}
            AsyncOperation ao = SceneManager.LoadSceneAsync(scnGameLobby, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                SetProgressBar(ao.progress);
                yield return null;
            }

            Debug.Log("End LoadLobbyCoroutine()");
        }

        public void LoadStage()
        {
            // Loading
            StartCoroutine(Loading(LoadStageCoroutine()));
        }

        void UnloadStage()
        {
            if (SceneManager.GetSceneByName(scnMap).isLoaded)
                SceneManager.UnloadSceneAsync(scnMap);
            if (SceneManager.GetSceneByName(scnMap).isLoaded)
                SceneManager.UnloadSceneAsync(scnUI);
        }

        IEnumerator LoadStageCoroutine()
        {
            AsyncOperation ao;
            if ( !SceneManager.GetSceneByName(scnMap).isLoaded )
            {
                ao = SceneManager.LoadSceneAsync(scnMap, LoadSceneMode.Additive);
                while (!ao.isDone)
                {
                    SetProgressBar(ao.progress / 2f);
                    yield return null;
                }
            }
            

            ao = SceneManager.LoadSceneAsync(scnUI, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                SetProgressBar((ao.progress + 1f) / 2f);
                yield return null;
            }
        }

        


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

