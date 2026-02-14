using UnityEngine;

public class SCROLLSCR : MonoBehaviour
{
    public float moveAmount = 50f; // Adjust as needed
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public void MoveUp()
    {
        transform.position += new Vector3(0, moveAmount, 0);
    }

    public void MoveDown()
    {
        transform.position = startPosition; // Moves back to the original position
    }
}
