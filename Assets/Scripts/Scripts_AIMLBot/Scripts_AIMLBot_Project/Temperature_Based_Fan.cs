using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add this!

public class Temperature_Based_Fan : MonoBehaviour
{
    public Dropdown dropdown;
    public TMP_InputField InputBox; // Use TMP_InputField

    private string[] questions = {
        "Why we use a temperature-controlled fan",
        "What does the DHT11 sensor do",
	"What happens if the room temperature goes above the set level",
	"How can this system be improved",
	"How should the DHT11 sensor and DC motor (fan) be wired to the Arduino in the Temperature-Based Fan Controller project"

    };

    void Awake()
    {
        if (dropdown == null)
        {
            Debug.LogError("Dropdown reference is not set in the Inspector!");
        }
        if (InputBox == null)
        {
            Debug.LogError("InputBox reference is not set in the Inspector!");
        }
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
        if (InputBox != null)
        {
            InputBox.text = QuestionData.selectedQuestion;
        }
        Debug.Log("Selected question stored: " + QuestionData.selectedQuestion);
    }

    public void GoToInputScene()
    {
        if (dropdown == null) return;
        QuestionData.selectedQuestion = dropdown.options[dropdown.value].text;
        SceneManager.LoadScene("InputScene");
    }
}