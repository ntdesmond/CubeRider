using GameFlow;
using TMPro;
using UnityEngine;

namespace UI.GemReward
{
    public class BaseGemReward : MonoBehaviour
    {
        private int _gemCount;
        private TMP_Text _textField;

        public void Construct(LevelGemCounter counter, TMP_Text textField)
        {
            _gemCount = counter.BaseReward;
            _textField = textField;
        }

        private void Awake()
        {
            SetGemCountText();
        }

        private void SetGemCountText()
        {
            _textField.text = _gemCount.ToString();
        }
    }
}