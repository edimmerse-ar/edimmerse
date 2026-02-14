using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add this!

public class DropDown_Manager : MonoBehaviour
{
    public Dropdown dropdown;
    //public TMP_InputField InputBox; // Use TMP_InputField

    private string[] questions = {
        "Why do plants need a consistent water supply, and what could go wrong if they are overwatered or underwatered?",
        "What do we expect to happen if we put the sensor in dry soil?"
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