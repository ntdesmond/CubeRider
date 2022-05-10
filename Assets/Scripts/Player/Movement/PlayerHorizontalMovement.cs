using Player.Character;
using Player.Cubes.Container;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerHorizontalMovement : MonoBehaviour
    {
        private float _lastMousePos = -1;
        private float _fieldToScreenRatio;

        private float _maxHorizontalOffset;
        
        public void Construct(UnityEngine.Camera camera, GameFlow.GameFlow gameFlow)
        {
            // Get the X bounds of the player and set them as constraint
            var myTransform = transform;
            _maxHorizontalOffset = 0.5f - myTransform.localScale.x / 2;
            
            // Calculate the ratio for player horizontal movement
            var myLeft = transform.TransformPoint(Vector3.left / 2);
            var myRight = transform.TransformPoint(Vector3.right / 2);
            
            var screenLeft = camera.WorldToScreenPoint(myLeft);
            var screenRight = camera.WorldToScreenPoint(myRight);

            _fieldToScreenRatio = transform.localScale.x / (screenRight.x - screenLeft.x);

            gameFlow.GameOver += DisableInput;
        }
        
        private void Update()
        {
            // Check if mouse/finger is held
            if (!Input.GetMouseButton(0))
            {
                _lastMousePos = -1;
                return;
            }

            // Find the mouse offset
            var mousePos = Input.mousePosition.x;
            var mouseDeltaX = _lastMousePos > 0 ? mousePos - _lastMousePos : 0;
            _lastMousePos = mousePos;

            // Move horizontallу
            var position = transform.localPosition;
            
            position.x = Mathf.Clamp(
                position.x + mouseDeltaX * _fieldToScreenRatio,
                -_maxHorizontalOffset, 
                _maxHorizontalOffset
            );
            transform.localPosition = position;
        }

        private void DisableInput()
        {
            enabled = false;
        }
    }
}
