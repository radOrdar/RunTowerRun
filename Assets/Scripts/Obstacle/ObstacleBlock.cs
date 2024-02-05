using System.Collections;
using UnityEngine;

namespace Obstacle
{
    [RequireComponent(typeof(Rigidbody))]
    public class ObstacleBlock : MonoBehaviour
    {
        private Rigidbody _rb;
        
        public void OnCollided(Vector3 velocity)
        {
            _rb = GetComponent<Rigidbody>();
            _rb.velocity = velocity;
            _rb.useGravity = true;

            // StartCoroutine(CheckGrounded());
        }

        private IEnumerator CheckGrounded()
        {
            while (!Mathf.Approximately(0, _rb.velocity.sqrMagnitude))
            {
                yield return null;
            }

            Destroy(_rb);
        }
    }
}