using System;
using System.Collections.Generic;
using GameFlow;
using UnityEngine;

namespace UI
{
    public class CanvasSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuCanvas;
        [SerializeField] private GameObject _inLevelCanvas;
        [SerializeField] private GameObject _levelCompletedCanvas;
        [SerializeField] private GameObject _levelFailedCanvas;

        private List<GameObject> _canvases;
        
        public void Construct(GameEvents gameEvents)
        {
            gameEvents.LevelStarted += OnLevelStarted;
            gameEvents.LevelFailed += OnLevelFailed;
            gameEvents.LevelCompleted += OnLevelCompleted;
        }

        private void Awake()
        {
            _canvases = new List<GameObject>
            {
                _mainMenuCanvas,
                _inLevelCanvas,
                _levelCompletedCanvas,
                _levelFailedCanvas
            };
        }

        private void DisableAllCanvases()
        {
            foreach (var canvas in _canvases)
            {
                canvas.SetActive(false);
            }
        }

        private void OnLevelStarted()
        {
            DisableAllCanvases();
            _inLevelCanvas.SetActive(true);
        }

        private void OnLevelFailed()
        {
            DisableAllCanvases();
            _levelFailedCanvas.SetActive(true);
        }

        private void OnLevelCompleted()
        {
            DisableAllCanvases();
            _levelCompletedCanvas.SetActive(true);
        }
    }
}