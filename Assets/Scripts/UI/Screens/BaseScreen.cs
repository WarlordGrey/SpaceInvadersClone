using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Core;
using UniRx;

namespace UI.Screens
{

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseScreen : MonoBehaviour
    {

        [Inject]
        private ScreenManager _screenManager;

        [SerializeField]
        private CanvasGroup group = null;
        [SerializeField]
        private bool pauseTimescaleOnShow = true;

        private Canvas canvas = null;

        public bool CanBeShown
        {
            get;
            protected set;
        } = true;

        public virtual void Close(ScreenManager.AfterShowHideAction callback = null)
        {
            BaseScreen active = _screenManager.ActiveScreen;
            if ((active != null) && (active.Equals(this)))
            {
                _screenManager.Close(callback);
            }
        }

        public CanvasGroup CanvasGroup
        {
            get
            {
                return group;
            }
        }

        public Canvas Canvas
        {
            get
            {
                if (canvas == null)
                {
                    canvas = GetComponent<Canvas>();
                }
                return canvas;
            }
        }

        public virtual void OnHide()
        {
            if (pauseTimescaleOnShow)
            {
                EventsHub.PauseGame.OnNext(Unit.Default);
            }
        }

        public virtual void OnShow()
        {
            if (pauseTimescaleOnShow)
            {
                EventsHub.UnPauseGame.OnNext(Unit.Default);
            }
        }

        public void ActionAfterStateChanged(bool isShown)
        {
            if (isShown)
            {
                AfterShow();
            }
            else
            {
                AfterHide();
            }
        }

        protected virtual void AfterShow()
        {

        }

        protected virtual void AfterHide()
        {

        }

    }

}