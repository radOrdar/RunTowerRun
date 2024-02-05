using UnityEngine;

namespace Obstacle
{
    public class GatePass : MonoBehaviour
    {
        private void Awake()
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.center = Vector3.up * 0.5f;
            boxCollider.size = new Vector3(5, 50, 1);
            gameObject.layer = LayerMask.NameToLayer("Gates");
        }
    }
}