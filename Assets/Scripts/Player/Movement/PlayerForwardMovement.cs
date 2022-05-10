using System;
using Field;
using GameFlow;
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
        
        public void Construct(GameEvents gameEvents)
        {
            gameEvents.GameOver += StopMovement;
            gameEvents.LevelStarted += OnLevelStarted;
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

        private void OnLevelStarted()
        {
            var myTransform = transform;
            myTransform.localPosition = new Vector3(0, myTransform.localPosition.y, 0);
            myTransform.localRotation = Quaternion.Euler(Vector3.zero);
            enabled = true;
        }

        private void StopMovement()
        {
            enabled = false;
        }

        private void MoveStraight()
        {
            var myTransform = transform;
            myTransform.position += movementSpeed * Time.deltaTime * myTransform.forward;
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

            var fieldPart = hit.collider;
            
            if (fieldPart.CompareTag("Finish"))
            {
                OnFinishReached();
                return;
            }
            
            if (!fieldPart.TryGetComponent<Turn>(out var turn))
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
            var myTransform = transform;
            myTransform.rotation = Quaternion.Euler(0, _initialRotationY + (_isTurningRight ? 90 : -90), 0);
                
            // Round the position as well
            var originalPosition = myTransform.position;
            myTransform.position = new Vector3(
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
            var myTransform = transform;
            
            _turnCenter = turnTransform.TransformPoint(Vector3.up / 2);
            _turnRadius = (_turnCenter - myTransform.position).magnitude;

            _initialRotationY = myTransform.rotation.eulerAngles.y;
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
