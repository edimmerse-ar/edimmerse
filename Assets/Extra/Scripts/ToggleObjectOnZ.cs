using UnityEngine;
using System.Collections;

public class ToggleObjectOnZ : MonoBehaviour
{
    public GameObject objectToToggle; // Assign the GameObject to toggle in Inspector
    public float delay = 1f;          // Time delay for toggling
    private float minZ = 0.05f;       // Lower Z boundary
    private float maxZ = 0.9f;        // Upper Z boundary
    private bool isToggling = false;

    void Update()
    {
        float zPos = transform.position.z;
        Debug.Log(zPos);

        if (zPos > minZ && zPos < maxZ && !isToggling)
        {
            StartCoroutine(ToggleObject());
        }
        else if (zPos <= minZ || zPos >= maxZ)
        {
            StopCoroutine(ToggleObject());
            isToggling = false;
        }
    }

    private IEnumerator ToggleObject()
    {
        isToggling = true;
        while (transform.position.z > minZ && transform.position.z < maxZ)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
            yield return new WaitForSeconds(delay);
        }
        isToggling = false;
    }
}
