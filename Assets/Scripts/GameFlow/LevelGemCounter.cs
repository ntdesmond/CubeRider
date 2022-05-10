using System;
using UnityEngine;

namespace GameFlow
{
    public class LevelGemCounter : MonoBehaviour
    {
        public int BonusMultiplier { get; private set; }
        public int BaseReward => _baseGemReward;
        
        public int TotalReward => _baseGemReward * BonusMultiplier;

        public event Action BonusChanged;

        [Range(1, 100)]
        [SerializeField] private int _baseGemReward = 10;

        public void Construct(GameEvents gameEvents)
        {
            gameEvents.LevelStarted += ResetBonusCounter;
            gameEvents.BonusBrickHit += IncrementBonus;
        }
        
        private void IncrementBonus()
        {
            BonusMultiplier++;
            BonusChanged?.Invoke();
        }
        
        private void ResetBonusCounter()
        {
            BonusMultiplier = 0;
            BonusChanged?.Invoke();
        }
        
    }
}