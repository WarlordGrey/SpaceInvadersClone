using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using Core;
using Data;

namespace UI.Screens
{

    public class ScreenManager : MonoBehaviour
    {

        public delegate void AfterShowHideAction();

        //[SerializeField]
        //private float animDuration = 1;

        [SerializeField]
        private LoadingScreen _loadingScreen;
        [SerializeField]
        private MainMenuScreen _mainMenuScreen;
        [SerializeField]
        private HighScoreScreen _highScoreScreen;
        [SerializeField]
        private GameplayScreen _gameplayScreen;
        [SerializeField]
        private ResultScreen _resultScreen;

        private Stack<BaseScreen> activeScreens = new Stack<BaseScreen>();
        private List<BaseScreen> screens = new List<BaseScreen>();
        private List<System.IDisposable> disposables = new List<System.IDisposable>();

        private void Awake()
        {
            foreach (Transform cur in transform)
            {
                if (cur.TryGetComponent(out BaseScreen screen))
                {
                    screens.Add(screen);
                }
            }
            foreach (var cur in screens)
            {
                cur.CanvasGroup.alpha = 0;
                cur.CanvasGroup.gameObject.SetActive(true);
                cur.CanvasGroup.gameObject.SetActive(false);
            }
            Show(_loadingScreen);
            disposables.Add(EventsHub.InitCompleted.Subscribe(OnInitCompleted).AddTo(this));
            disposables.Add(EventsHub.FinishLevel.Subscribe(OnFinishLevel).AddTo(this));
            disposables.Add(EventsHub.StartGame.Subscribe(OnStartGame).AddTo(this));
            disposables.Add(EventsHub.GoToMainMenu.Subscribe(OnMainMenuShow).AddTo(this));
            disposables.Add(EventsHub.GoToHighScores.Subscribe(OnHighScoresShow).AddTo(this));
        }

        private void OnInitCompleted(Unit unit)
        {
            //if ((ActiveScreen != null) && !(ActiveScreen.Equals(_mainMenuScreen)))
            //{
            //    return;
            //}
            Show(_mainMenuScreen);
        }

        private void OnDestroy()
        {
            foreach (var cur in disposables)
            {
                cur.Dispose();
            }
        }

        public BaseScreen ActiveScreen
        {
            get
            {
                if (activeScreens.Count > 0)
                {
                    return activeScreens.Peek();
                }
                return null;
            }
        }

        private void OnFinishLevel(LevelFinishedData data)
        {
            Show(_resultScreen);
        }

        private void OnStartGame(Unit unit)
        {
            Show(_gameplayScreen);
        }

        private void OnMainMenuShow(Unit unit)
        {
            Show(_mainMenuScreen);
        }

        private void OnHighScoresShow(Unit unit)
        {
            Show(_highScoreScreen);
        }

        private void SimpleShow(BaseScreen screen, AfterShowHideAction callback, bool animated = false)
        {
            if (screen != null)
            {
                screen.OnShow();
                ChangeScreenState(screen, callback, animated, true);
            }
            else
            {
                callback?.Invoke();
            }
        }

        private void SimpleHide(AfterShowHideAction callback, bool animated = false)
        {
            if (activeScreens.Count > 0)
            {
                BaseScreen screen = activeScreens.Peek();
                screen.OnHide();
                ChangeScreenState(screen, callback, animated, false);
            }
            else
            {
                callback?.Invoke();
            }
        }

        private void SimpleClose(AfterShowHideAction callback, bool animated = false)
        {
            if (activeScreens.Count > 0)
            {
                BaseScreen screen = activeScreens.Pop();
                screen.OnHide();
                ChangeScreenState(screen, callback, animated, false);
            }
            else
            {
                callback?.Invoke();
            }
        }

        private void ChangeScreenState(BaseScreen screen, AfterShowHideAction callback, bool animated, bool isShown)
        {
            int fadeState = isShown ? 1 : 0;
            if (screen != null)
            {
                Debug.LogFormat("Screen: <color=green>{0}</color>; IsShown: {1}", screen, isShown);
                if (!screen.gameObject.activeSelf)
                {
                    screen.gameObject.SetActive(true);
                }
                if (isShown)
                {
                    //screen.CanvasGroup.gameObject.SetActive(isShown);
                    screen.Canvas.enabled = isShown;
                }
                if (animated)
                {
                    //screen.CanvasGroup.DOFade(fadeState, animDuration).OnComplete(
                    //    () =>
                    //    {
                    //        callback?.Invoke();
                    //        if (!isShown)
                    //        {
                    //            screen.CanvasGroup.gameObject.SetActive(isShown);
                    //            screen.ActionAfterStateChanged(isShown);
                    //        }
                    //    }    
                    //);
                }
                else
                {
                    screen.CanvasGroup.alpha = fadeState;
                    callback?.Invoke();
                    if (!isShown)
                    {
                        //screen.CanvasGroup.gameObject.SetActive(isShown);
                        screen.Canvas.enabled = isShown;
                    }
                    screen.ActionAfterStateChanged(isShown);
                }
            }
            else
            {
                callback?.Invoke();
            }
        }

        public void Show(BaseScreen screen, AfterShowHideAction callback = null, bool withClose = false)
        {
            if ((ActiveScreen != null) && (ActiveScreen.Equals(screen)))
            {
                return;
            }
            if ((screen != null) && (screen.CanBeShown))
            {
                if (withClose)
                {
                    SimpleClose(() =>
                    {
                        activeScreens.Push(screen);
                        SimpleShow(screen, callback);
                    });
                }
                else
                {
                    SimpleHide(() =>
                    {
                        activeScreens.Push(screen);
                        SimpleShow(screen, callback);
                    });
                }
            }
            else
            {
                callback?.Invoke();
            }
        }

        public void CloseAndShow(BaseScreen screen, AfterShowHideAction callback = null)
        {
            Show(screen, callback, true);
        }

        public void Close(AfterShowHideAction callback = null)
        {
            SimpleClose(() =>
            {
                SimpleShow(ActiveScreen, callback);
            });
        }

        public void CloseAll(AfterShowHideAction callback = null)
        {
            if (activeScreens.Count <= 0)
            {
                callback?.Invoke();
                return;
            }
            SimpleClose(() => { CloseAllRecursion(callback); });
        }

        private void CloseAllRecursion(AfterShowHideAction callback)
        {
            if (activeScreens.Count <= 0)
            {
                callback?.Invoke();
                return;
            }
            SimpleClose(() => { CloseAllRecursion(callback); }, false);
        }

        //private void OnApplicationPause(bool isPaused)
        //{
            //if (!isPaused)
            //{
            //    OnPause(true);
            //}
        //}

        public void ShowOnlyOnGameScreen(BaseScreen screen, AfterShowHideAction callback = null)
        {
            while ((ActiveScreen != null) && !ActiveScreen.Equals(_gameplayScreen))
            {
                Close();
            }
            if (ActiveScreen == null)
            {
                Show(_gameplayScreen);
            }
            Show(screen, callback);
        }

        public void RejectOrShowOnGameScreen(BaseScreen screen, AfterShowHideAction callback = null)
        {
            if ((ActiveScreen != null) && (ActiveScreen.Equals(screen)))
            {
                Show(screen, callback);
            }
        }

        private void OnValidate()
        {
            _loadingScreen = transform.GetComponentInChildren<LoadingScreen>();
            _mainMenuScreen = transform.GetComponentInChildren<MainMenuScreen>();
            _highScoreScreen = transform.GetComponentInChildren<HighScoreScreen>();
            _gameplayScreen = transform.GetComponentInChildren<GameplayScreen>();
            _resultScreen = transform.GetComponentInChildren<ResultScreen>();
        }

    }

}