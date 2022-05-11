using GameFlow;
using UnityEngine;

namespace UI.GemReward
{
    public class TotalLevelReward : MonoBehaviour
    {
        private GemTweening _gem;
        private LevelGemCounter _levelCounter;

        public void Construct(LevelGemCounter levelCounter, GameEvents gameEvents, GemTweening gem)
        {
            _levelCounter = levelCounter;
            _gem = gem;
            gameEvents.LevelCompleted += AddGems;
        }

        private void AddGems()
        {
            _gem.MoveToCounter(_levelCounter.TotalReward);
        }
    }
}