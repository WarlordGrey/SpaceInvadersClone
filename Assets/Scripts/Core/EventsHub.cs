using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Data;

namespace Core {

    public class EventsHub
    {

        public static readonly Subject<Unit> InitCompleted = new Subject<Unit>();
        public static readonly Subject<Unit> StartGame = new Subject<Unit>();
        public static readonly Subject<Unit> LoadLevel = new Subject<Unit>();
        public static readonly Subject<Unit> LevelLoaded = new Subject<Unit>();
        public static readonly Subject<LevelFinishedData> FinishLevel = new Subject<LevelFinishedData>();

        public static readonly Subject<bool> OnApplicationPaused = new Subject<bool>();

        public static readonly Subject<Unit> PauseGame = new Subject<Unit>();
        public static readonly Subject<Unit> UnPauseGame = new Subject<Unit>();

        public static readonly Subject<Unit> GoToMainMenu = new Subject<Unit>();
        public static readonly Subject<Unit> GoToHighScores = new Subject<Unit>();

        public static readonly Subject<DeadEnemyData> EnemyDied = new Subject<DeadEnemyData>();
        public static readonly Subject<Unit> PlayerDamaged = new Subject<Unit>();
        public static readonly Subject<int> UpdateScore = new Subject<int>();

    }

}