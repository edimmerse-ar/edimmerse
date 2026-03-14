using UnityEngine;

public class ClampYMovement : MonoBehaviour
{
    public float fixedY = 0.061f; // Set this to the desired Y position

    void Update()
    {
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }
}
