using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class initials : MonoBehaviour
{
	public string characterSelectScene;
	public ChangeScene changeScene;
	// Start is called before the first frame update
	void Start()
    {
        PlayerPrefs.SetInt("unlock", 1);
        PlayerPrefs.SetInt("coinData", 0);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void LoadInfo()
	{
		string name = PlayerPrefs.GetString("username", "");
		string age = PlayerPrefs.GetString("age", "");
		string mail = PlayerPrefs.GetString("email", "");
		int character = PlayerPrefs.GetInt("char", 1);

		if (string.IsNullOrEmpty(name))
		{
			changeScene.GoToScene(characterSelectScene);
			return;
		}

		if (!int.TryParse(age, out int ageValue) || ageValue <= 0)
		{
			changeScene.GoToScene(characterSelectScene);
			return;
		}

		if (!IsValidEmail(mail))
		{
			changeScene.GoToScene(characterSelectScene);
			return;
		}

		changeScene.GoToScene("ModeMenu");

		GlobalVariables.PlayerName = name;
		GlobalVariables.PlayerAge = age;
		GlobalVariables.PlayerMail = mail;
		GlobalVariables.character = character;
	}

	private bool IsValidEmail(string email)
	{
		return email.Contains("@") && email.Contains(".");
	}

	public void resetData()
	{
		PlayerPrefs.DeleteAll();
	}
}
