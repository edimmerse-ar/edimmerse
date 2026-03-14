using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    // Public GameObject to be set in the Inspector
    public GameObject targetObject;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (targetObject != null)
        {
            // Optionally set the initial state of the targetObject
            targetObject.SetActive(isActive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Optionally perform actions based on isActive
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hanger"))
        {
            // Toggle the active state
            isActive = !isActive;

            // Set the active state of the targetObject
            if (targetObject != null)
            {
                targetObject.SetActive(isActive);
            }
        }
    }
}
