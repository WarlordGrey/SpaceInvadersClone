using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Screens;

namespace Core {

    public class AppInstaller : MonoInstaller
    {

        [SerializeField]
        private GameManager _gameManagerPrefab;
        [SerializeField]
        private ScreenManager _screenManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInNewPrefab(_gameManagerPrefab).AsSingle().NonLazy();
            Container.Bind<ScreenManager>().FromComponentInNewPrefab(_screenManagerPrefab).AsSingle().NonLazy();
        }

    }

}