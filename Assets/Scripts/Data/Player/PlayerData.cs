using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core;
using Zenject;
using UniRx;

namespace Data.Player
{
    [Serializable]
    public class PlayerData
    {

        private static PlayerData _instance = null;

        [Inject]
        private GameManager _gameManager;
        private HighScoreDataComparer _highScoreDataComparer = new HighScoreDataComparer();

        public List<HighScoreData> highScores = new List<HighScoreData>();

        public static PlayerData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load();
                    _instance.InitSaving();
                }
                return _instance;
            }
        }

        private PlayerData()
        {

        }

        public void AddNewHighScore(HighScoreData scoreData)
        {
            highScores.Insert(highScores.BinarySearch(scoreData, _highScoreDataComparer), scoreData);
        }

        public void Save()
        {
            Debug.Log("Attempt to save player's data...");
            PlayerPrefs.SetString(GlobalConstants.kPlayerDataKey, JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }

        private static PlayerData Load()
        {
            Debug.Log("Attempt to load player's data...");
            string gameDataJson = PlayerPrefs.GetString(GlobalConstants.kPlayerDataKey, null);
            PlayerData playerData = null;
            if (string.IsNullOrEmpty(gameDataJson))
            {
                playerData = new PlayerData();
                return playerData;
            }

            try
            {
                playerData = JsonUtility.FromJson<PlayerData>(gameDataJson);
                playerData = playerData ?? new PlayerData();
            }
            catch
            {
                playerData = new PlayerData();
                Debug.Log("Player Data: Failed to load player prefs, creating a new instance");
            }
            return playerData;
        }

        private void OnPause(bool paused)
        {
            if (paused)
            {
                Save();
            }
        }

        private void InitSaving()
        {
            EventsHub.OnApplicationPaused.Subscribe(OnPause).AddTo(_gameManager);
        }

    }

    struct JsonDateTime
    {
        public long value;
        public static implicit operator DateTime(JsonDateTime jdt)
        {
            Debug.Log("Converted to time");
            return DateTime.FromFileTimeUtc(jdt.value);
        }
        public static implicit operator JsonDateTime(DateTime dt)
        {
            Debug.Log("Converted to JDT");
            JsonDateTime jdt = new JsonDateTime();
            jdt.value = dt.ToFileTimeUtc();
            return jdt;
        }
    }

}