using Player.Character;
using UnityEngine;

namespace Player.Trail
{
    public class StopAboveLava : MonoBehaviour
    {
        private Transform _parent;
        private Vector3 _position;
        private Transform _trailPrefab;

        
        public void Construct(PrefabData prefabs)
        {
            _trailPrefab = prefabs.playerTrailPrefab;
        }
        
        private void Awake()
        {
            var myTransform = transform;
            _parent = myTransform.parent;
            _position = myTransform.localPosition;
        }

        private void Update()
        {
            if (Physics.Raycast(
                _parent.TransformPoint(Vector3.up),
                Vector3.down,
                float.PositiveInfinity,
                LayerMask.GetMask("Field")
            ))
            {
                SpawnNewTrail();
                return;
            }
            
            // Stop the trail movement
            transform.parent = null;
        }

        private void SpawnNewTrail()
        {
            // Proceed only if the current trail is over
            if (transform.parent != null)
            {
                return;
            }
            
            // Instantiate the new trail and disable the current component
            var newTrail = Instantiate(_trailPrefab, _parent);
            newTrail.transform.localPosition = _position;
            
            enabled = false;
        }
    }
}
