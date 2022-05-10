using System;
using Player.Cubes.Container;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CubeCounter : MonoBehaviour
    {
        private CubeContainer _container;
        private TMP_Text _textField;

        public void Construct(CubeContainer container, TMP_Text textField, GameFlow.GameFlow gameFlow)
        {
            _container = container;
            _textField = textField;
            gameFlow.FinishReached += DisableCounter;
        }

        private void OnEnable()
        {
            _container.CubeCountChanged += SetCubeCountText;
        }

        private void OnDisable()
        {
            _container.CubeCountChanged -= SetCubeCountText;
        }

        private void DisableCounter()
        {
            enabled = false;
        }

        private void SetCubeCountText()
        {
            _textField.text = _container.CubeCount.ToString();
        }
    }
}