using UnityEngine;

namespace Player
{
    public class Debounce : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public void Awake()
        {
            // Not using DI since a global resolver is used
            _rigidbody = GetComponent<Rigidbody>();
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