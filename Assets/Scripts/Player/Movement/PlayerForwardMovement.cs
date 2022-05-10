using System;
using Field;
using Player.Character;
using Player.Cubes.Container;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerForwardMovement : MonoBehaviour
    {
        [Min(1)]
        public float movementSpeed;

        public event Action FinishReached;

        private bool _finishReached;
        private bool _isTurning;
        
        private Vector3 _turnCenter;
        private float _turnRadius;
        
        private bool _isTurningRight;
        
        private float _initialRotationY;
        
        public void Construct(GameFlow.GameFlow gameFlow)
        {
            gameFlow.GameOver += StopMovement;
        }
        
        private void Update()
        {
            CheckRoad();
            
            if (_isTurning)
            {
                MoveOnTurn();
                return;
            }
            
            MoveStraight();
        }

        private void StopMovement()
        {
            enabled = false;
        }

        private void MoveStraight()
        {
            transform.position += movementSpeed * Time.deltaTime * transform.forward;
        }

        private void MoveOnTurn()
        {
            // Find the rotation angle (s / r == v * t / r)
            var deltaAngle = movementSpeed * Time.deltaTime / _turnRadius;
            
            // Use this angle for rotation
            transform.RotateAround(
                _turnCenter,
                _isTurningRight ? Vector3.up : Vector3.down,
                deltaAngle * Mathf.Rad2Deg
            );
        }

        private void CheckRoad()
        {
            if (_finishReached || !Physics.Raycast(
                transform.TransformPoint(Vector3.up),
                Vector3.down,
                out var hit,
                float.PositiveInfinity,
                LayerMask.GetMask("Field")
            ))
            {
                return;
            }

            var collider = hit.collider;
            
            if (collider.CompareTag("Finish"))
            {
                OnFinishReached();
                return;
            }
            
            if (!collider.TryGetComponent<Turn>(out var turn))
            {
                FinishTurn();
                return;
            }

            StartTurn(turn);
        }

        private void FinishTurn()
        {
            if (!_isTurning)
            {
                return;    
            }
            
            // Fix rotation to one of 90-degree directions
            transform.rotation = Quaternion.Euler(0, _initialRotationY + (_isTurningRight ? 90 : -90), 0);
                
            // Round the position as well
            var originalPosition = transform.position;
            transform.position = new Vector3(
                Mathf.Round(originalPosition.x * 2) / 2,
                originalPosition.y,
                Mathf.Round(originalPosition.z * 2) / 2
            );
                
            _isTurning = false;
        }

        private void StartTurn(Turn turn)
        {
            if (_isTurning)
            {
                return;    
            }
            
            _isTurningRight = turn.direction == Turn.TurnDirection.Right;
            
            // Calculate the center and radius point of the arc
            var turnTransform = turn.transform;
            _turnCenter = turnTransform.TransformPoint(Vector3.up / 2);
            _turnRadius = (_turnCenter - transform.position).magnitude;

            _initialRotationY = transform.rotation.eulerAngles.y;
            _isTurning = true;
        }

        private void OnFinishReached()
        {
            FinishTurn();
            _finishReached = true;
            FinishReached?.Invoke();
        }
    }
}
