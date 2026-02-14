using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGameObjectWithDelay : MonoBehaviour
{
    public ScoreGen ScoreScript;
    public Dropdown[] dd; // Array of Dropdowns
    public GameObject objectToToggle; // GameObject to be toggled
    public GameObject Error,finishLoose,finishWin,codeBtn; // GameObject to be toggled
    public Dropdown dropdown; // Dropdown to control delay
    private float delay = 1f; // Default delay
    private Coroutine toggleCoroutine; // Store the running coroutine
    public string p,p1,p2;

    void Start()
    {
        // Setup the delay change listener
        dropdown.onValueChanged.AddListener(DropdownValueChanged);
    }

    // Run code based on dropdown selections
    public void RunCode()
    {
        if (CheckConditions())
        {
            ScoreScript.TotalScore+=9;
            Debug.Log("Conditions matched, toggling object.");

            // Stop any running coroutine to avoid multiple coroutines toggling
            if (toggleCoroutine != null) StopCoroutine(toggleCoroutine);

            // Start toggling the object
            toggleCoroutine = StartCoroutine(ToggleObject());
            codeBtn.SetActive(false);
            finishWin.SetActive(true);
            finishLoose.SetActive(false);
        }
        else
        {
            ScoreScript.TotalError+=1;
            Error.SetActive(true);
            finishWin.SetActive(false);
            finishLoose.SetActive(true);
            codeBtn.SetActive(true);



            // Log dropdown values to help debug
            for (int i = 0; i < dd.Length; i++)  // Fixed loop syntax
            {
                Debug.Log($"Dropdown {i}: {dd[i].options[dd[i].value].text}");
            }
        }
    }

    // Verify if dropdown selections meet the conditions
    private bool CheckConditions()
    {
        return
            dd[0].options[dd[0].value].text == p &&
            dd[1].options[dd[1].value].text == "OUTPUT" &&
            dd[2].options[dd[2].value].text == "digitalWrite()" &&
            dd[3].options[dd[3].value].text == p1 &&
            dd[4].options[dd[4].value].text == "HIGH" &&
            dd[5].options[dd[5].value].text == "digitalWrite()" &&
            dd[6].options[dd[6].value].text == p2 &&
            dd[7].options[dd[7].value].text == "LOW";
    }

    // Update delay based on dropdown value
    private void DropdownValueChanged(int value)
    {
        delay = value switch
        {
            0 => 1f,
            1 => 1f,
            2 => 2f,
            _ => 1f // Default case
        };
    }

    // Coroutine to toggle the GameObject state
    private IEnumerator ToggleObject()
    {
        while (true)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
            yield return new WaitForSeconds(delay);
        }
    }
}
