using System;
using Player;
using Player.Cubes.Container;
using Player.Movement;
using UI.Input;
using UnityEngine;

namespace GameFlow
{
    public class GameEvents : MonoBehaviour
    {
        public event Action LevelStarted, GameOver, LevelFailed, LevelCompleted, NextLevel, LevelRetry, MainMenuEntered, FinishReached, BonusBrickHit;
        private CubeContainer _cubeContainer;
        
        public void Construct(
            CubeContainer container,
            FaceObstacles playerObstacles,
            PlayerForwardMovement playerForwardMovement
        )
        {
            _cubeContainer = container;
            
            LevelFailed += InvokeGameOver;
            LevelCompleted += InvokeGameOver;
            LevelRetry += InvokeLevelStarted;
            NextLevel += InvokeLevelStarted;
            
            playerForwardMovement.FinishReached += OnFinishReached;
            playerObstacles.WallCollided += InvokeLevelFailed;
        }

        /// <summary>
        /// Callback to be called in the player cubes when they bump into the bonus brick.
        /// </summary>
        public void OnBonusBrickHit() => BonusBrickHit?.Invoke();

        /// <summary>
        /// Callback to be called in the player cubes when they reach the final bonus brick.
        /// </summary>
        public void OnVeryEndReached() => LevelCompleted?.Invoke();

        /// <summary>
        /// Callback to be called by the UI.
        /// </summary>
        public void OnNextLevel() => NextLevel?.Invoke();

        /// <summary>
        /// Callback to be called by the UI.
        /// </summary>
        public void OnLevelRetry() => LevelRetry?.Invoke();

        /// <summary>
        /// Callback to be called by the UI.
        /// </summary>
        public void OnLevelStart() => LevelStarted?.Invoke();

        /// <summary>
        /// Callback to be called by the UI.
        /// </summary>
        public void OnMainMenu() => MainMenuEntered?.Invoke();

        private void Awake()
        {
            _cubeContainer.NoCubesLeft += InvokeLevelFailed;
        }

        private void OnFinishReached()
        {
            FinishReached?.Invoke();
            _cubeContainer.NoCubesLeft -= InvokeLevelFailed;
            _cubeContainer.NoCubesLeft += InvokeLevelCompleted;
        }

        private void InvokeGameOver()
        {
            GameOver?.Invoke();
        }

        private void InvokeLevelCompleted()
        {
            LevelCompleted?.Invoke();
        }

        private void InvokeLevelFailed()
        {
            LevelFailed?.Invoke();
        }

        private void InvokeLevelStarted()
        {
            LevelStarted?.Invoke();
        }
    }
}