using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            Vector3 directionAway = transform.position - (target.position - transform.position);
            transform.LookAt(directionAway);
        }
    }
}
