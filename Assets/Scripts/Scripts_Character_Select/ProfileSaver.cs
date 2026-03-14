using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ProfileSaver : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField nameInput;
	[SerializeField] private TMP_InputField ageInput;
	[SerializeField] private TMP_InputField mailInput;

	public Button nextButtonInfo;
	public Button nextButtonChar;
	public TMP_Text warningText;          

	public SceneHandler sceneHandler;

	[Header("Groups")]
	public GameObject group1Obj1;
	public GameObject group1Obj2;

	public GameObject group2Obj1;
	public GameObject group2Obj2;

	void Start()
	{
		nameInput.onValueChanged.AddListener(_ => CheckAllFields(nameInput));
		ageInput.onValueChanged.AddListener(_ => CheckAllFields(ageInput));
		mailInput.onValueChanged.AddListener(_ => CheckAllFields(mailInput));
		CheckAllFields(null);
	}

    private TMP_InputField _lastFocused;

    private void Update()
    {
        TMP_InputField focused = null;
        if (nameInput != null && nameInput.isFocused) focused = nameInput;
        else if (ageInput != null && ageInput.isFocused) focused = ageInput;
        else if (mailInput != null && mailInput.isFocused) focused = mailInput;

        if (focused != _lastFocused)
        {
            _lastFocused = focused;
            CheckAllFields(_lastFocused);
        }
    }
	public void changeCharacter(int character)
	{
		GlobalVariables.character = character;
		PlayerPrefs.SetInt("character", character);
		PlayerPrefs.Save();
		nextButtonChar.interactable = true;
	}

	public void SaveInfo()
	{
		string name = nameInput.text.Trim();
		string age = ageInput.text.Trim();
		string mail = mailInput.text.Trim();

		if (string.IsNullOrEmpty(name))
		{
			return;
		}

		if (!int.TryParse(age, out int ageValue) || ageValue <= 0)
		{
			return;
		}

		if (!IsValidEmail(mail))
		{
			return;
		}

		PlayerPrefs.SetString("username", name);
		PlayerPrefs.SetString("age", age);
		PlayerPrefs.SetString("email", mail);
		PlayerPrefs.Save();

		GlobalVariables.PlayerName = name;
		GlobalVariables.PlayerAge = age;
		GlobalVariables.PlayerMail = mail;

		sceneHandler.GoToScene("ModeMenu");
	}

    void CheckAllFields(TMP_InputField focusedField)
    {
        var username = nameInput != null ? nameInput.text.Trim() : string.Empty;
        var ageText = ageInput != null ? ageInput.text.Trim() : string.Empty;
        var email = mailInput != null ? mailInput.text.Trim() : string.Empty;

        bool validUsername = !string.IsNullOrEmpty(username) && username.Length >= 2;
        bool validAge = int.TryParse(ageText, out var ageValue) && ageValue > 0;
        bool validEmail = IsValidEmail(email);

        // Determine which field to show warnings for. Use the supplied focusedField (from onValueChanged)
        // so typing in a field only shows warnings for that field. If focusedField is null, fall back to _lastFocused.
        var fieldToCheck = focusedField ?? _lastFocused;

        string warning = string.Empty;

        if (fieldToCheck == nameInput)
        {
            if (string.IsNullOrEmpty(username)) warning = "Enter username.";
            else if (username.Length < 2) warning = "Username too short.";
        }
        else if (fieldToCheck == ageInput)
        {
            if (!int.TryParse(ageText, out _) || ageValue <= 0) warning = "Enter a valid age.";
        }
        else if (fieldToCheck == mailInput)
        {
            if (!validEmail) warning = "Enter a valid email.";
        }
        else
        {
            // No focused field specified - show first failing message
            if (!validUsername) warning = string.IsNullOrEmpty(username) ? "Enter username." : "Username too short.";
            else if (!validAge) warning = "Enter a valid age.";
            else if (!validEmail) warning = "Enter a valid email.";
        }

        nextButtonInfo.interactable = validUsername && validAge && validEmail;
        warningText.text = warning;
    }

	private bool IsValidEmail(string email)
	{
		return email.Contains("@") && email.Contains(".");
	}

	public void ActivateGroup1()
	{
		StartCoroutine(EnableAndDisableAfterDelay(group1Obj1, group1Obj2));
	}

	public void ActivateGroup2()
	{
		StartCoroutine(EnableAndDisableAfterDelay(group2Obj1, group2Obj2));
	}

	private IEnumerator EnableAndDisableAfterDelay(GameObject obj1, GameObject obj2)
	{
		obj1.SetActive(true);
		obj2.SetActive(true);

		yield return new WaitForSeconds(5f);

		obj1.SetActive(false);
		obj2.SetActive(false);
	}
}
