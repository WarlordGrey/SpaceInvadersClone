using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core {

    public class AppInstaller : MonoInstaller
    {

        [SerializeField]
        private GameManager gameManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInNewPrefab(gameManagerPrefab).AsSingle().NonLazy();
        }

    }

}