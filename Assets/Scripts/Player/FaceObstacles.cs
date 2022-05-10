using System;
using GameFlow;
using UnityEngine;

namespace Player
{
    public class FaceObstacles : MonoBehaviour
    {
        public event Action WallCollided;
        
        private Collider _collider;
        private GameEvents _gameEvents;

        public void Construct(Collider myCollider, GameEvents gameEvents)
        {
            _collider = myCollider;
            _gameEvents = gameEvents;
        }

        private void Awake()
        {
            _gameEvents.FinishReached += StartBonusCounting;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var other = collision.collider;

            switch (other.tag)
            {
                case "VeryEnd":
                    WallCollided += _gameEvents.OnVeryEndReached;
                    break;
                case "Obstacle":
                    break;
                default:
                    // Return if not Obstacle or VeryEnd
                    return;
            }
            
            HandleWallCollision(other);
        }

        private void HandleWallCollision(Collider wall)
        {
            var myTransform = transform;
            
            if (!Physics.Raycast(
                _collider.bounds.center,
                myTransform.forward,
                out var hit,
                0.6f,
                LayerMask.GetMask("Obstacle")
            ) || hit.collider != wall)
            {
                return;
            }
            
            WallCollided?.Invoke();
        }

        private void StartBonusCounting()
        {
            WallCollided += _gameEvents.OnBonusBrickHit;
        }
    }
}
