using UnityEngine;
using Dan.Main;
using Dan.Models;

using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public int sceneIndexToLoad;
	public GameObject loading;
	public GameObject robot;

	public SceneHandler sceneHandler;

	void Start()
    {
		Leaderboards.EdImmerse.GetPersonalEntry(OnPersonalEntryLoaded, ErrorCallback);
    }

	public void LoadScen()
	{
		SceneManager.LoadScene(sceneIndexToLoad);
	}

	private void OnPersonalEntryLoaded(Entry entry)
	{
		GlobalVariables.score = entry.Score;
		loading.SetActive(false);
		robot.SetActive(true);
	}

	private void ErrorCallback(string error)
	{
		Debug.LogError(error);
		loading.SetActive(false);
		robot.SetActive(true);
	}
	public void LoadInfo()
	{
		string name = PlayerPrefs.GetString("username", "");
		string age = PlayerPrefs.GetString("age", "");
		string mail = PlayerPrefs.GetString("email", "");
		int character = PlayerPrefs.GetInt("char", 1);

		if (string.IsNullOrEmpty(name))
		{
			sceneHandler.GoToScene("CharacterSelect");
			return;
		}

		if (!int.TryParse(age, out int ageValue) || ageValue <= 0)
		{
			sceneHandler.GoToScene("CharacterSelect");
			return;
		}

		if (!IsValidEmail(mail))
		{
			sceneHandler.GoToScene("CharacterSelect");
			return;
		}

		sceneHandler.GoToScene("ModeMenu");

		GlobalVariables.PlayerName = name;
		GlobalVariables.PlayerAge = age;
		GlobalVariables.PlayerMail = mail;
		GlobalVariables.character = character;
	}

	private bool IsValidEmail(string email)
	{
		return email.Contains("@") && email.Contains(".");
	}
}
