using System;
using UnityEngine;

namespace GameFlow
{
    public class GameGlobalState : MonoBehaviour
    {
        public event Action GemCountChanged, LevelChanged;
        public int GemCount
        {
            get => _gemCount;
            set => SaveGemCount(value);
        }

        public int Level
        {
            get => _level;
            set => SaveLevel(value);
        }

        public void ResetGameState()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private int _gemCount;
        private int _level;

        private const string _levelKey = "level";
        private const string _gemsKey = "gems";
        
        private void Awake()
        {
            _gemCount = PlayerPrefs.GetInt(_gemsKey, 0);
            _level = PlayerPrefs.GetInt(_levelKey, 0);
        }

        private void SaveGemCount(int gemCount)
        {
            _gemCount = gemCount;
            GemCountChanged?.Invoke();
            PlayerPrefs.SetInt(_gemsKey, gemCount);
            PlayerPrefs.Save();
        }

        private void SaveLevel(int level)
        {
            _level = level;
            LevelChanged?.Invoke();
            PlayerPrefs.SetInt(_levelKey, level);
            PlayerPrefs.Save();
        }
    }
}