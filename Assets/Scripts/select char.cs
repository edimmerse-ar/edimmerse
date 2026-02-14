using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageSelectorTMP : MonoBehaviour
{
    public Image optionA;
    public Image optionB;
    public Button nextButton;
    public TMP_Text warningText;

    private bool isSelected = false;
    private Image selectedOption;

    void Start()
    {
        nextButton.interactable = false;
        warningText.text = "Please select one option.";

        optionA.GetComponent<Button>().onClick.AddListener(() => SelectOption(optionA));
        optionB.GetComponent<Button>().onClick.AddListener(() => SelectOption(optionB));
    }

    void SelectOption(Image selected)
    {
        isSelected = true;
        selectedOption = selected;
        nextButton.interactable = true;
        warningText.text = "";

    }
}
