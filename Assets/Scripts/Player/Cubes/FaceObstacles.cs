using System;
using Player.Animation;
using UnityEngine;

namespace Player.Cubes
{
    public class FaceObstacles : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag) {
                case "Obstacle":
                    HandleWallCollision(other.transform);
                    break;
                case "Lava":
                    HandleLavaCollision();
                    break;
            }
        }

        private void HandleWallCollision(Transform wall)
        {
            var myTransform = transform;
            var collisionAngle = Vector3.Angle(
                myTransform.forward,
                wall.position - myTransform.position
            );
            
            if (collisionAngle > 40)
            {
                return;
            }
            
            myTransform.parent = null;
            Destroy(this, 5);
        }

        private void HandleLavaCollision()
        {
            Destroy(gameObject);
        }
    }
}
