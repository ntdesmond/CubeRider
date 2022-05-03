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
            // Destroy the cube in the level
            Destroy(cube.gameObject);
            
            var myTransform = transform;
            var playerTransform = _player.transform;
            var playerPosition = playerTransform.position;
            
            // Spawn a copy of player's cube and move player accordingly
            var newCube = Instantiate(myTransform, myTransform.parent);
            newCube.position = playerPosition;
            playerPosition += Vector3.up;
            playerTransform.position = playerPosition;
        }
    }
}
