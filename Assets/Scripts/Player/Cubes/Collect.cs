using Player.Cubes.Container;
using UnityEngine;

namespace Player.Cubes
{
    public class Collect : MonoBehaviour
    {
        private CubeContainer _container;
        
        public void Construct(CubeContainer container)
        {
            _container = container;
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
            
            _container.AddCube();
        }
    }
}
