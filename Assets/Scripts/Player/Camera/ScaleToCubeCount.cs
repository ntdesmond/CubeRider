using Player.Character;
using Player.Cubes.Container;
using UnityEngine;

namespace Player.Camera
{
    public class ScaleToCubeCount : MonoBehaviour
    {
        [Range(1, 179)]
        public float minAngle = 60, maxAngle = 100;
        
        [Range(1, 40)]
        public float fOVChangeSpeed = 30;
        
        private UnityEngine.Camera _camera;
        private Transform _character;
        private float _targetFOV;
        
        public void Construct(CharacterAnimations character, UnityEngine.Camera playerCamera, CubeContainer container)
        {
            _character = character.transform;
            _camera = playerCamera;
            container.CubeCountChanged += OnCubeCountChanged;
        }

        private void Awake()
        {
            _targetFOV = _camera.fieldOfView;
        }

        private void OnCubeCountChanged()
        {
            var myTransform = transform;
            var playerTop = _character.position + Vector3.up * 2;
            var angleToPlayer = Vector3.Angle(
                myTransform.forward,
                playerTop - myTransform.position
            );
            
            _targetFOV = Mathf.Clamp(angleToPlayer * 2, minAngle, maxAngle);
        }

        private void Update()
        {
            if (Mathf.Approximately(_camera.fieldOfView, _targetFOV))
            {
                return;
            }

            _camera.fieldOfView = Mathf.MoveTowards(
                _camera.fieldOfView,
                _targetFOV,
                fOVChangeSpeed * Time.deltaTime
            );
        }
    }
}