using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States.GameStates;
using System;
using UniRx;
using Data;

namespace Core
{
    public class GameManager : MonoBehaviour
    {

        private AGameState _gameState = null;
        private List<IDisposable> _disposables = new List<IDisposable>();
        private int _enemiesCount = -1;
        private int _livesCount = -1;
        private int _score = -1;

        public AGameState GameState
        {
            get
            {
                if (_gameState == null)
                {
                    _gameState = new WaitGameState();
                }
                return _gameState;
            }
            set
            {
                _gameState = value;
                _gameState.Init();
            }
        }

        private void Awake()
        {
            _disposables.Add(EventsHub.StartGame.Subscribe(OnStartGame).AddTo(this));
            _disposables.Add(EventsHub.LevelLoaded.Subscribe(OnLevelLoaded).AddTo(this));
            _disposables.Add(EventsHub.EnemyDied.Subscribe(OnEnemyDied).AddTo(this));
            _disposables.Add(EventsHub.PlayerDamaged.Subscribe(OnPlayerDamaged).AddTo(this));
        }

        void Start()
        {

        }

        void Update()
        {

        }

        private void OnDestroy()
        {
            foreach (var cur in _disposables)
            {
                cur.Dispose();
            }
        }

        private void OnStartGame(Unit unit)
        {
            GameState = new StartState();
            _enemiesCount = GlobalConstants.kBaseEnemiesCount;
            _livesCount = GlobalConstants.kBaseLivesCount;
            _score = 0;
        }

        private void OnLevelLoaded(Unit unit)
        {
            GameState = new GameplayState();
        }

        private void OnEnemyDied(DeadEnemyData data)
        {
            _enemiesCount--;
            _score += data.score;
            if(_enemiesCount <= 0)
            {
                LevelFinishedData lfd;
                lfd.isSuccessful = true;
                lfd.score = _score;
                EventsHub.FinishLevel.OnNext(lfd);
                GameState = new FinishedState();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            EventsHub.OnApplicationPaused.OnNext(pause);
        }

        private void OnPlayerDamaged(Unit unit)
        {
            _livesCount--;
            if(_livesCount <= 0)
            {
                LevelFinishedData lfd;
                lfd.isSuccessful = false;
                lfd.score = _score;
                EventsHub.FinishLevel.OnNext(lfd);
                GameState = new FinishedState();
            }
        }

    }

}