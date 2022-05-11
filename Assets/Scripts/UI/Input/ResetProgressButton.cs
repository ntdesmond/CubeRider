using System;
using GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Input
{
    public class ResetProgressButton : MonoBehaviour
    {
        public void Construct(LongPressButton button, GameGlobalState gameGlobalState)
        {
            button.LongPressed += gameGlobalState.ResetGameState;
        }
    }
}