using Player.Animation;
using UnityEngine;

namespace Player.Cubes
{
    public class Collect : MonoBehaviour
    {
        private PlayerAnimations _player;
        
        public void Construct(PlayerAnimations player)
        {
            _player = player;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("PlayerCube"))
            {
                return;
            }

            var cubeParent = other.transform.parent;
            if (cubeParent.CompareTag("PlayerCube"))
            {
                PickManyCubes(cubeParent);
                return;
            }

            PickCube(other);
        }

        private void PickManyCubes(Transform cubesParent)
        {
            foreach (Transform cube in cubesParent)
            {
                PickCube(cube);
            }
            Destroy(cubesParent.gameObject);
        }

        private void PickCube(Component cube)
        {
            // Deactivate and destroy the cube in the level
            // Deactivation is needed to prevent repetitive pickups of the cube
            var cubeGameObject = cube.gameObject;
            cubeGameObject.SetActive(false);
            Destroy(cubeGameObject);
            
            var playerTransform = _player.transform;
            var playerPosition = playerTransform.position;
            
            // Spawn a copy of player's cube
            var newCube = Instantiate(this, transform.parent);
            newCube.Construct(_player);
            newCube.transform.position = playerPosition;
            
            // Move player up
            playerPosition += Vector3.up;
            playerTransform.position = playerPosition;
        }
    }
}
