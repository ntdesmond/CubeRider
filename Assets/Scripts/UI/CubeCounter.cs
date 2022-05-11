using System;
using GameFlow;
using Player.Cubes.Container;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CubeCounter : MonoBehaviour
    {
        private CubeContainer _container;
        private TMP_Text _textField;

        public void Construct(CubeContainer container, TMP_Text textField)
        {
            _container = container;
            _textField = textField;
        }

        private void Awake()
        {
            _container.CubeCountChanged += SetCubeCountText;
        }

        private void SetCubeCountText()
        {
            _textField.text = _container.CubeCount.ToString();
        }
    }
}