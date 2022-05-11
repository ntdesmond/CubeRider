using System;
using GameFlow;
using LevelTrash;
using UnityEngine;

namespace Player.Trail
{
    public class Recreate : MonoBehaviour
    {
        private GameEvents _gameEvents;
        
        private Transform _parent;
        private Transform _trailPrefab;
        private Transform _trash;
        
        private Vector3 _position;


        public void Construct(PrefabData prefabs, Trash trash, GameEvents gameEvents)
        {
            _trailPrefab = prefabs.playerTrailPrefab;
            _trash = trash.transform;
            _gameEvents = gameEvents;
        }
        
        private void Awake()
        {
            var myTransform = transform;
            _parent = myTransform.parent;
            _position = myTransform.localPosition;
            
            
            _gameEvents.GameOver += StopAndDisable;
            _gameEvents.LevelStarted += SpawnNewTrail;
        }

        private void OnDestroy()
        {
            _gameEvents.GameOver -= StopAndDisable;
            _gameEvents.LevelStarted -= SpawnNewTrail;
        }

        private void Update()
        {
            // Stop the trail when no field below
            if (!Physics.Raycast(
                _parent.TransformPoint(Vector3.up),
                Vector3.down,
                float.PositiveInfinity,
                LayerMask.GetMask("Field")
            ))
            {
                StopSelf();
                return;
            }
            
            // Restart the trail when is above the field again
            SpawnNewTrail();
        }

        private void SpawnNewTrail()
        {
            // Proceed only if the current trail is over
            if (!transform.IsChildOf(_trash))
            {
                return;
            }
            
            // Instantiate the new trail and disable the current component
            var newTrail = Instantiate(_trailPrefab, _parent);
            newTrail.transform.localPosition = _position;
            
            enabled = false;
        }

        private void StopSelf()
        {
            transform.SetParent(_trash);
        }

        private void StopAndDisable()
        {
            StopSelf();
            enabled = false;
        }
    }
}
