using System;
using GameFlow;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Input
{
    public class MainMenuTouchHandler : MonoBehaviour, IPointerDownHandler
    {
        private GameEvents _gameEvents;
        
        public void Construct(GameEvents gameEvents)
        {
            _gameEvents = gameEvents;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _gameEvents.OnLevelStart();
        }
    }
}