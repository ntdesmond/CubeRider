using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainMenuTouchHandler : MonoBehaviour, IPointerDownHandler
    {
        public event Action UserTouched;
        public void OnPointerDown(PointerEventData eventData)
        {
            UserTouched?.Invoke();
        }
    }
}