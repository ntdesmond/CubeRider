using System;
using System.Collections.Generic;
using Player.Movement;
using UI;
using UnityEngine;

namespace GameFlow
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _levels;
        
        private GameGlobalState _gameGlobalState;
        private GameObject _currentLevel;

        private int _currentLevelIndex;

        public void Construct(GameGlobalState gameGlobalState)
        {
            _gameGlobalState = gameGlobalState;
        }
        
        private void Awake()
        {
            // Restore the index from save and load the level
            _currentLevelIndex = _gameGlobalState.Level;
            LoadCurrentLevel();
        }

        private void LoadNextLevel()
        {
            Destroy(_currentLevel);

            _currentLevelIndex = GetNextLevelIndex();
            _gameGlobalState.Level = _currentLevelIndex;
            
            LoadCurrentLevel();
        }

        private void LoadCurrentLevel()
        {
            _currentLevel = Instantiate(_levels[_currentLevelIndex]);
        }
        
        /// <summary>
        /// Get the index of the next level.
        /// Level 0 is considered to be a tutorial, so it will be skipped after the level chain loops.
        /// </summary>
        private int GetNextLevelIndex()
        {
            var nextIndex = (_currentLevelIndex + 1) % _levels.Count;
            if (nextIndex == 0)
            {
                nextIndex++;
            }

            return nextIndex;
        }
    }
}