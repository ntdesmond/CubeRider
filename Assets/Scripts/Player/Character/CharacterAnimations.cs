using Player.Cubes.Container;
using UnityEngine;

namespace Player.Character
{
    public class CharacterAnimations : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _body;
        
        private readonly int _fall = Animator.StringToHash("Fall");
        private readonly int _fallBackwards = Animator.StringToHash("Fall Backwards");

        public void Construct(CubeContainer container, Animator animator, Rigidbody body)
        {
            container.NoCubesLeft += OnNoCubesLeft;
            _animator = animator;
            _body = body;
        }
        
        private void OnNoCubesLeft()
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
    }
}
