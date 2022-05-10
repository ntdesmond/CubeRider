using System;
using Player;
using Player.Cubes.Container;
using Player.Movement;
using UnityEngine;

namespace GameFlow
{
    public class GameFlow : MonoBehaviour
    {
        public event Action GameOver, LevelFailed, LevelCompleted;
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
            
            playerForwardMovement.FinishReached += OnFinishReached;
            playerObstacles.WallCollided += InvokeLevelFailed;
        }

        private void Awake()
        {
            _cubeContainer.NoCubesLeft += InvokeLevelFailed;
        }

        private void OnFinishReached()
        {
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
    }
}