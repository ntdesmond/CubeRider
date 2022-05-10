using Player.Cubes.Container;
using UnityEngine;

namespace Player.Cubes
{
    public class SelfRemove : MonoBehaviour
    {
        [Min(0)] [SerializeField] private float _fallDestroyDelay = 0.5f;
        [Min(0)] [SerializeField] private float _fallDistanceThreshold = 0.3f;
        
        private CubeContainer _container;

        public void Construct(FaceObstacles cubeObstacles, CubeContainer container)
        {
            _container = container;
            cubeObstacles.WallCollided += OnWallCollided;
        }

        private void OnWallCollided()
        {
            _container.RemoveCube(transform); 
        }
        
        private void FixedUpdate()
        {
            // Destroy the cube if it falls below the player's bounds
            var myTransform = transform;
            var yDiff = _container.transform.position.y - myTransform.position.y;
            
            if (yDiff <= _fallDistanceThreshold)
            {
                return;
            }

            _container.RemoveCube(transform);
            Destroy(gameObject, _fallDestroyDelay);
            enabled = false;
        }
    }
}