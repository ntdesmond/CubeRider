using System;
using GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Input
{
    public class RetryButton : MonoBehaviour
    {
        public void Construct(Button button, GameEvents gameEvents)
        {
            button.onClick.AddListener(gameEvents.OnLevelRetry);
        }
    }
}