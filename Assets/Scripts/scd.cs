using UnityEngine;
using UnityEngine.UI;

public class scd : MonoBehaviour
{
    public ScoreGen ScoreScript;
    private bool placed = false;   // Tracks if the LED is placed
    public GameObject led, pannels, defpannels, errorBox, placementBTN;         // The LED object that will stop dragging
    public string TagName;

    public GameObject doneStep, nextStep;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Stick collided");

        if (other.gameObject.CompareTag(TagName))
        {
            Debug.Log("Found it!");
            placed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagName))
        {
            Debug.Log("Found it!");
            placed = false;
        }
    }

    // This function will disable the drag component if placed is true
    public void SetPosition()
    {
        if (placed)
        {
            doneStep.SetActive(false);
            nextStep.SetActive(true);
            // Check if the led has a LeanDragTranslate component
            var dragComponent = led.GetComponent<Lean.Touch.LeanDragTranslate>();
            if (dragComponent != null)
            {
                Destroy(dragComponent);  // Disable by removing
                Debug.Log("Drag disabled");
                pannels.SetActive(false);
                defpannels.SetActive(false);
                placementBTN.SetActive(false);
                ScoreScript.TotalScore += 2;

            }
            else
            {
                Debug.LogWarning("LeanDragTranslate not found on the LED object.");
            }
        }
        else
        {
            errorBox.SetActive(true);
            ScoreScript.TotalError += 1;
        }
    }

}
