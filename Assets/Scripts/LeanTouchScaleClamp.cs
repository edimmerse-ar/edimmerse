using UnityEngine;
using Lean.Touch;

public class LeanTouchScaleClamp : MonoBehaviour
{
    public float minScale = 1f; // Minimum scale
    public float maxScale = 3f; // Maximum scale
    public float sensitivity = 0.1f; // Controls how fast scaling happens

    private void Update()
    {
        // Detect pinch gesture and scale accordingly
        if (LeanTouch.Fingers.Count == 2) // Checks if two fingers are touching the screen
        {
            float pinchScale = LeanGesture.GetPinchScale(); // Get pinch scale factor

            // Apply scaling with sensitivity
            Vector3 newScale = transform.localScale * (1 + (pinchScale - 1) * sensitivity);

            // Clamp the scale between min and max
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            // Apply the new clamped scale to the object
            transform.localScale = newScale;
        }
    }
}
