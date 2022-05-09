using System;
using Player.Cubes.Container;
using UnityEngine;

namespace Player.Cubes
{
    public class FaceObstacles : MonoBehaviour
    {
        private CubeContainer _container;
        public void Construct(CubeContainer container)
        {
            _container = container;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Obstacle"))
            {
                return;
            }
            
            HandleWallCollision(other.transform);
        }

        private void HandleWallCollision(Transform wall)
        {
            var myTransform = transform;
            var collisionAngle = Vector3.Angle(
                myTransform.forward,
                wall.position - myTransform.position
            );
            
            if (collisionAngle > 40)
            {
                return;
            }
            
            _container.RemoveCube(transform);
        }

        private void FixedUpdate()
        {
            // Destroy the cube if it falls below the player's bounds
            var myTransform = transform;
            var yDiff = myTransform.position.y - _container.transform.position.y;
            var isAlreadyRemoved = myTransform.parent == null;
            
            if (yDiff >= -0.3f || isAlreadyRemoved)
            {
                return;
            }

            _container.RemoveCube(transform);
            Destroy(gameObject, 3);
        }
    }
}
