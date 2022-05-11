using System;
using DG.Tweening;
using GameFlow;
using UnityEngine;

namespace UI.GemReward
{
    public class GemTweening : MonoBehaviour
    {
        
        private GemCounter _counter;
        private Vector3 _counterPosition;
        private Transform _gemTweenPrefab;
        private GameGlobalState _gameState;

        public void Construct(PrefabData prefabs, GemCounter counter, GameGlobalState gameState)
        {
            _gemTweenPrefab = prefabs.gemTweenPrefab;
            _counter = counter;
            _gameState = gameState;
        }

        private void Awake()
        {
            var counterTransform = _counter.transform;
            _counterPosition = counterTransform.TransformPoint(
                _counter.GetComponent<RectTransform>().offsetMin
            );
        }

        public void MoveToCounter(int gemCount)
        {
            var sequence = DOTween.Sequence();
            for (var i = 0; i < gemCount; i++)
            {
                var newGem = Instantiate(_gemTweenPrefab, transform.parent);
                sequence.Append(
                    newGem
                        .DOMove(_counterPosition, 0.1f)
                        .OnComplete(() => OnGemTweened(newGem))
                );
            }

            sequence.Play().OnComplete(() => _gameState.GemCount += gemCount);
        }

        private void OnGemTweened(Transform gem)
        {
            Destroy(gem.gameObject);
            _counter.Increment();
        }
    }
}