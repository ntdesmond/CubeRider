using UnityEngine;

namespace Player
{
    public class Debounce : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public void Construct(Rigidbody body)
        {
            _rigidbody = body;
        }
        
        
        private void FixedUpdate()
        {
            if (_rigidbody.velocity.y <= 0)
            {
                return;
            }
            _rigidbody.velocity = Vector3.zero;
        }
    }
}