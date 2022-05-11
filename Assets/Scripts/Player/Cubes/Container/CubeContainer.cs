using System;
using GameFlow;
using LevelTrash;
using Player.Character;
using UI.Input;
using UnityEngine;

namespace Player.Cubes.Container
{
    public class CubeContainer : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _initialCubeCount = 1;
        
        public event Action CubeAdded, CubeRemoved, CubeCountChanged, NoCubesLeft;
        public int CubeCount => transform.childCount;

        private Transform _character;
        private Transform _cubePrefab;
        private Transform _trash;

        public void Construct(
            PrefabData prefabs,
            CharacterAnimations character,
            GameEvents gameEvents,
            Trash trash
        )
        {
            _cubePrefab = prefabs.playerCubePrefab;
            _character = character.transform;
            _trash = trash.transform;
            gameEvents.LevelRetry += AddInitialCubes;
            gameEvents.LevelStarted += AddInitialCubes;
            gameEvents.MainMenuEntered += AddInitialCubes;
            gameEvents.GameOver += RemoveAllCubes;
        }
        
        private void Awake()
        {
            CubeAdded += OnCubeCountChanged;
            CubeRemoved += OnCubeCountChanged;
            AddInitialCubes();
        }

        private void AddInitialCubes()
        {
            if (CubeCount > 0)
            {
                return;
            }
            
            _character.localPosition = Vector3.zero;
            for (var i = 0; i < _initialCubeCount; i++)
            {
                AddCube();
            }
        }

        /// <summary>
        /// Spawn a new cube from the prefab as a child of the container.
        /// </summary>
        public void AddCube()
        {
            var characterPosition = _character.localPosition;
            
            // Spawn a copy of player's cube
            var newCube = Instantiate(_cubePrefab, transform);
            newCube.localPosition = characterPosition;
            CubeAdded?.Invoke();
            
            // Move character up
            characterPosition += Vector3.up;
            _character.localPosition = characterPosition;
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
            
            cube.SetParent(_trash);
            CubeRemoved?.Invoke();
        }
        public void RemoveAllCubes()
        {
            foreach (Transform cube in transform)
            {
                cube.SetParent(_trash);
                CubeRemoved?.Invoke();
            }
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
