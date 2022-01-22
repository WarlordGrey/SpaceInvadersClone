using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI.Screens;
using UniRx;

namespace Core
{

    public class StartScript : MonoBehaviour
    {

        [SerializeField]
        private LoadingScreen loadingScreen = null;
        [SerializeField]
        private float startTimeout = 0.1f;

        private bool isLoadingStarted = false;

        // Start is called before the first frame update
        private void Awake()
        {
            Application.targetFrameRate = 60;
            SceneManager.sceneLoaded += OnSceneLoaded;
            loadingScreen.ForceWork();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Equals("Main"))
            {
                SceneManager.UnloadSceneAsync("Start");
                EventsHub.InitCompleted.OnNext(Unit.Default);
            }
        }

        private void Update()
        {
            if (startTimeout > 0)
            {
                startTimeout -= Time.deltaTime;
            }
            else if (!isLoadingStarted)
            {
                SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
                isLoadingStarted = true;
            }
        }

    }

}