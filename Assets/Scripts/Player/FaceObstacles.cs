using System;
using UnityEngine;

namespace Player
{
    public class FaceObstacles : MonoBehaviour
    {
        public event Action WallCollided;
        
        private Collider _collider;

        public void Construct(Collider myCollider)
        {
            _collider = myCollider;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            var other = collision.collider;
            if (!other.CompareTag("Obstacle"))
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
        }
    }
}
