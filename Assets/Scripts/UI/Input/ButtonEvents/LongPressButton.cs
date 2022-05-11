using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Input
{
    public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Min(0)]
        [SerializeField] private float pressDelay = 1;
        
        public event Action LongPressed;

        private float _holdTimeLeft;
        private bool _isHolding;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _holdTimeLeft = pressDelay;
            _isHolding = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isHolding = false;
        }

        private void Update()
        {
            if (!_isHolding)
            {
                return;
            }
            
            if (_holdTimeLeft > 0)
            {
                _holdTimeLeft -= Time.deltaTime;
                return;
            }
            
            LongPressed?.Invoke();
            _isHolding = false;
        }
    }
}