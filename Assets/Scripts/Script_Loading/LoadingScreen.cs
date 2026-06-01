using UnityEngine;
using Dan.Main;
using Dan.Models;

public class LoadingScreen : MonoBehaviour
{
	public SceneHandler sceneHandler;

	void Start()
	{
		Leaderboards.EdImmerse.GetPersonalEntry(OnPersonalEntryLoaded, ErrorCallback);
	}
	private void OnPersonalEntryLoaded(Entry entry)
	{
		GlobalVariables.score = entry.Score;
		sceneHandler.GoToScene("MainMenu");
	}

	private void ErrorCallback(string error)
	{
		int score = PlayerPrefs.GetInt("Score", 0);
		GlobalVariables.score = score;
		Debug.LogError(error);
		sceneHandler.GoToScene("MainMenu");
	}
}
