using System;
using GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Input
{
    public class NextLevelButton : MonoBehaviour
    {
        public void Construct(Button button, GameEvents gameEvents)
        {
            button.onClick.AddListener(gameEvents.OnNextLevel);
        }
    }
}