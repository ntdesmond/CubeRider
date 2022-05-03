using Player.Animation;
using UnityEngine;

namespace Player.Cubes
{
    public class FaceObstacles : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.CompareTag("Obstacle"))
            {
                return;
            }

            var myTransform = transform;
            var collisionAngle = Vector3.Angle(
                myTransform.forward,
                collision.collider.transform.position - myTransform.position
            );
            
            if (collisionAngle > 40)
            {
                return;
            }
            
            myTransform.parent = null;
            Destroy(this);
        }
    }
}
