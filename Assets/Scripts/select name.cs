using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FormValidator : MonoBehaviour
{
    public TMP_InputField[] inputFields;  // Assign all your input fields here in the inspector
    public Button nextButton;             // Assign your Next button
    public TMP_Text warningText;          // Assign a TMP_Text to show warnings

    void Start()
    {
        // Add listener to all input fields
        foreach (var field in inputFields)
        {
            field.onValueChanged.AddListener(delegate { CheckAllFields(); });
        }

        CheckAllFields(); // Initial check
    }

    void CheckAllFields()
    {
        bool allFilled = true;

        foreach (var field in inputFields)
        {
            if (string.IsNullOrWhiteSpace(field.text))
            {
                allFilled = false;
                break;
            }
        }

        nextButton.interactable = allFilled;
        warningText.text = allFilled ? "" : "Please fill all the feilds..";
    }
}
