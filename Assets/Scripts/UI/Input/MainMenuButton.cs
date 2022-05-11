using System;
using GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Input
{
    public class MainMenuButton : MonoBehaviour
    {
        public void Construct(Button button, GameEvents gameEvents)
        {
            button.onClick.AddListener(gameEvents.OnMainMenu);
        }
    }
}