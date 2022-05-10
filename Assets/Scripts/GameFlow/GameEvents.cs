using System;
using Player;
using Player.Cubes.Container;
using Player.Movement;
using UI;
using UnityEngine;

namespace GameFlow
{
    public class GameEvents : MonoBehaviour
    {
        public event Action LevelStarted, GameOver, LevelFailed, LevelCompleted, FinishReached, BonusBrickHit;
        private CubeContainer _cubeContainer;
        
        public void Construct(
            CubeContainer container,
            FaceObstacles playerObstacles,
            PlayerForwardMovement playerForwardMovement,
            MainMenuTouchHandler touchHandler
        )
        {
            _cubeContainer = container;
            
            LevelFailed += InvokeGameOver;
            LevelCompleted += InvokeGameOver;
            
            playerForwardMovement.FinishReached += OnFinishReached;
            playerObstacles.WallCollided += InvokeLevelFailed;
            touchHandler.UserTouched += InvokeLevelStarted;
        }

        /// <summary>
        /// Callback to be called in the player cubes when they bump into the bonus brick.
        /// </summary>
        public void OnBonusBrickHit()
        {
            BonusBrickHit?.Invoke();
        }

        /// <summary>
        /// Callback to be called in the player cubes when they reach the final bonus brick.
        /// </summary>
        public void OnVeryEndReached()
        {
            InvokeLevelCompleted();
        }
        
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