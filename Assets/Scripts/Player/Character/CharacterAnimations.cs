using GameFlow;
using Player.Cubes.Container;
using Player.Movement;
using UnityEngine;

namespace Player.Character
{
    public class CharacterAnimations : MonoBehaviour
    {
        private Animator _animator;
        
        private readonly int _fall = Animator.StringToHash("Fall");
        private readonly int _fallBackwards = Animator.StringToHash("Fall Backwards");
        private readonly int _dance = Animator.StringToHash("Dance");
        

        public void Construct(Animator animator, GameEvents gameEvents)
        {
            gameEvents.LevelFailed += OnLevelFailed;
            gameEvents.LevelCompleted += OnLevelCompleted;
            _animator = animator;
        }
        
        private void OnLevelFailed()
        {
            var myTransform = transform;
            
            var isObstacle = Physics.Raycast(
                myTransform.position + Vector3.up / 2,
                myTransform.forward,
                1.0f,
                LayerMask.GetMask("Obstacle")
            );
            
            _animator.SetTrigger(isObstacle ? _fallBackwards : _fall);
        }
        
        private void OnLevelCompleted()
        {
            _animator.SetBool(_dance, true);
        }
    }
}
