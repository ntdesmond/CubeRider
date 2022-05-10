using System;
using GameFlow;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GemCounter : MonoBehaviour
    {
        private int _gemCount;
        private GameGlobalState _gameGlobalState;
        private TMP_Text _textField;

        public void Construct(GameGlobalState gameGlobalState, TMP_Text textField)
        {
            _gameGlobalState = gameGlobalState;
            _textField = textField;
        }

        private void OnEnable()
        {
            _gameGlobalState.GemCountChanged += UpdateGemCount;
            UpdateGemCount();
        }

        private void OnDisable()
        {
            _gameGlobalState.GemCountChanged -= UpdateGemCount;
        }

        public void UpdateGemCount()
        {
            _gemCount = _gameGlobalState.GemCount;
        }


        public void Increment(int count = 1)
        {
            _gemCount += count;
            SetGemCountText();
        }

        private void SetGemCountText()
        {
            _textField.text = _gemCount.ToString();
        }
    }
}