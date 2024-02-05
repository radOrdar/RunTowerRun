using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
   

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
    }
}
