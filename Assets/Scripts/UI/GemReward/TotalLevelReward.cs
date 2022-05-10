using GameFlow;
using TMPro;
using UnityEngine;

namespace UI.GemReward
{
    public class TotalLevelReward : MonoBehaviour
    {
        private LevelGemCounter _counter;

        public void Construct(LevelGemCounter counter, GameEvents gameEvents)
        {
            _counter = counter;
            gameEvents.LevelCompleted += AddGems;
        }

        private void AddGems()
        {
            Debug.Log($"Total {_counter.TotalReward} gems");
        }
    }
}