using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField nameInput;
	[SerializeField] private TMP_InputField ageInput;
	[SerializeField] private TMP_InputField mailInput;

	public ChangeScene changeScene;

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

		changeScene.GoToScene("ModeMenu");
	}

	private bool IsValidEmail(string email)
	{
		// Basic email check (not regex-heavy)
		return email.Contains("@") && email.Contains(".");
	}
}
