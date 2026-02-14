using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add this!

public class Day_Night_Light_Control : MonoBehaviour
{
    public Dropdown dropdown;
    //public TMP_InputField InputBox; // Use TMP_InputField

    private string[] questions = {
        "What is the purpose of the Smart Day-Night Light Controller",
        "How LDR (Light Dependent Resistor) works when the surroundings become dark",
        "How LDR (Light Dependent Resistor) works when there is bright daylight",
        "How can this system be improved",
        "How should the components (LDR, LED etc.) be wired for this Smart Day-Night Light Controller"
    };

    void Awake()
    {
        if (dropdown == null)
        {
            Debug.LogError("Dropdown reference is not set in the Inspector!");
        }
        //if (InputBox == null)
        //{
        //    Debug.LogError("InputBox reference is not set in the Inspector!");
        //}
    }

    void Start()
    {
        if (dropdown == null) return;

        dropdown.ClearOptions();

        var options = new System.Collections.Generic.List<Dropdown.OptionData>();
        foreach (string q in questions)
        {
            options.Add(new Dropdown.OptionData(q));
        }
        dropdown.AddOptions(options);

        dropdown.RefreshShownValue();

        OnDropdownChanged(dropdown);
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChanged(dropdown); });

        QuestionData.selectedQuestion = dropdown.options[dropdown.value].text;
    }

    void OnDropdownChanged(Dropdown dropdown)
    {
        int index = dropdown.value;
        QuestionData.selectedQuestion = dropdown.options[index].text;
        //if (InputBox != null)
        //{
        //    InputBox.text = QuestionData.selectedQuestion;
        //}
        Debug.Log("Selected question stored: " + QuestionData.selectedQuestion);
    }

    public void GoToInputScene()
    {
        if (dropdown == null) return;
        QuestionData.selectedQuestion = dropdown.options[dropdown.value].text;
        SceneManager.LoadScene("InputScene");
    }
}