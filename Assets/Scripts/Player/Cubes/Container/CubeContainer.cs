using System;
using Player.Character;
using UnityEngine;

namespace Player.Cubes.Container
{
    public class CubeContainer : MonoBehaviour
    {
        public event Action CubeAdded, CubeRemoved, CubeCountChanged, NoCubesLeft;
        public int CubeCount => transform.childCount;

        private Transform _character;
        private Transform _cubePrefab;
        
        public void Construct(PrefabData prefabs, CharacterAnimations character)
        {
            _cubePrefab = prefabs.playerCubePrefab;
            _character = character.transform;
        }
        
        private void Awake()
        {
            CubeAdded += OnCubeCountChanged;
            CubeRemoved += OnCubeCountChanged;
        }

        /// <summary>
        /// Spawn a new cube from the prefab as a child of the container.
        /// </summary>
        public void AddCube()
        {
            var characterPosition = _character.position;
            
            // Spawn a copy of player's cube
            var newCube = Instantiate(_cubePrefab, transform);
            newCube.position = characterPosition;
            CubeAdded?.Invoke();
            
            // Move character up
            characterPosition += Vector3.up;
            _character.position = characterPosition;
        }

        /// <summary>
        /// Removes the cube from the children list.
        /// Note that cube's gameObject won't be destroyed.
        /// </summary>
        /// <param name="cube">The child transform of the cube</param>
        public void RemoveCube(Transform cube)
        {
            if (!cube.IsChildOf(transform))
            {
                return;
            }
            
            cube.parent = null;
            CubeRemoved?.Invoke();
        }

        private void OnCubeCountChanged()
        {
            CubeCountChanged?.Invoke();
            if (CubeCount == 0)
            {
                NoCubesLeft?.Invoke();
            }
        }
    }
}
