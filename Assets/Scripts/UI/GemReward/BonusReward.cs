using GameFlow;
using TMPro;
using UnityEngine;

namespace UI.GemReward
{
    public class BonusReward : MonoBehaviour
    {
        private LevelGemCounter _counter;
        private TMP_Text _textField;

        public void Construct(LevelGemCounter counter, TMP_Text textField)
        {
            _counter = counter;
            _textField = textField;
        }

        private void Awake()
        {
            _counter.BonusChanged += SetGemCountText;
            SetGemCountText();
        }

        private void SetGemCountText()
        {
            _textField.text = _counter.BonusMultiplier.ToString();
        }
    }
}