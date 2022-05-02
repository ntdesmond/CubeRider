using UnityEngine;

namespace Player
{
    public class PlayerHorizontalMovement : MonoBehaviour
    {
        private float _lastMousePos = -1;
        private float _fieldToScreenRatio;

        private float _maxHorizontalOffset;
        
        public void Construct(Camera camera)
        {
            // Get the X bounds of the player and set them as constraint
            var myTransform = transform;
            _maxHorizontalOffset = 0.5f - myTransform.localScale.x / 2;
            
            // Calculate the ratio for player horizontal movement
            var myLeft = transform.TransformPoint(Vector3.left / 2);
            var myRight = transform.TransformPoint(Vector3.right / 2);
            
            var screenLeft = camera.WorldToScreenPoint(myLeft);
            var screenRight = camera.WorldToScreenPoint(myRight);

            _fieldToScreenRatio = transform.localScale.x * (myRight.x - myLeft.x) / (screenRight.x - screenLeft.x);
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
    }
}
