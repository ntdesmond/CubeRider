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

        private void OnCollisionEnter(Collision collision)
        {
            var other = collision.collider;
            if (!(
                other.CompareTag("VeryEnd") || 
                other.CompareTag("Obstacle") || 
                other.CompareTag("Bonus")
            ))
            {
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
            
            if (wall.CompareTag("Bonus"))
            {
                _gameEvents.OnBonusBrickHit();
            }
            else if (wall.CompareTag("VeryEnd"))
            {
                _gameEvents.OnBonusBrickHit();
                _gameEvents.OnVeryEndReached();
            }
        }
    }
}
