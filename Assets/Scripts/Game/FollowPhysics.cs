using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void FixedUpdate()
    {
        transform.position = _target.position;
        transform.rotation = _target.rotation;
    }

    
}
