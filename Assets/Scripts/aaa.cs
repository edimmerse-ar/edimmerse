using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class aaa : MonoBehaviour
{
    public Dropdown[] dd; // Array of Dropdowns
    public GameObject objectToToggle; // GameObject whose material will be toggled
    public GameObject Error; // Error indicator
    public Dropdown dropdown; // Dropdown to control delay
    public Material material1; // First material
    public Material material2; // Second material



    private float delay = 1f; // Default delay
    private Coroutine toggleCoroutine; // Store the running coroutine
    private Renderer objRenderer; // Reference to the Renderer component

    void Start()
    {
        // Setup the delay change listener
        dropdown.onValueChanged.AddListener(DropdownValueChanged);

        // Get the Renderer component of the target object
        objRenderer = objectToToggle.GetComponent<Renderer>();
    }

    // Run code based on dropdown selections
    public void RunCode()
    {
        if (CheckConditions())
        {
            Debug.Log("Conditions matched, toggling material.");

            // Stop any running coroutine to avoid multiple coroutines toggling
            if (toggleCoroutine != null) StopCoroutine(toggleCoroutine);

            // Start toggling the material
            toggleCoroutine = StartCoroutine(ToggleMaterial());
        }
        else
        {
            Error.SetActive(true);

            // Log dropdown values to help debug
            for (int i = 0; i < dd.Length; i++)
            {
                Debug.Log($"Dropdown {i}: {dd[i].options[dd[i].value].text}");
            }
        }
    }

    // Verify if dropdown selections meet the conditions
    private bool CheckConditions()
    {
        return
            dd[0].options[dd[0].value].text == "13" &&
            dd[1].options[dd[1].value].text == "OUTPUT" &&
            dd[2].options[dd[2].value].text == "digitalRead()" &&
            dd[3].options[dd[3].value].text == "0" &&
            dd[4].options[dd[4].value].text == "HIGH" &&
            dd[5].options[dd[5].value].text == "digitalRead()" &&
            dd[6].options[dd[6].value].text == "0" &&
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

    // Coroutine to toggle the material between two options
    private IEnumerator ToggleMaterial()
    {
        bool useFirstMaterial = true;

        while (true)
        {
            objRenderer.material = useFirstMaterial ? material1 : material2;
            useFirstMaterial = !useFirstMaterial;

            yield return new WaitForSeconds(delay);
        }
    }
}
