using Dan.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

using Dan.Models;
using UnityEngine.UI;
public class IntroScene : MonoBehaviour
{
	[Header("Scene Settings")]
	public int sceneIndexToLoad = 1;  // Index of the scene to load next
	public Button button;
	public GameObject loading;

	private void Start()
	{
		Leaderboards.Edmmerse.GetPersonalEntry(OnPersonalEntryLoaded, ErrorCallback);
	}

	public void LoadScen()
	{
		// Start the coroutine to load the next scene after a delay
		SceneManager.LoadScene(sceneIndexToLoad);
	}

	private void OnPersonalEntryLoaded(Entry entry)
	{
		GlobalVariables.score = entry.Score;

		button.interactable =	(true);
		loading.SetActive(false);
	}

	private void ErrorCallback(string error)
	{
		Debug.LogError(error);
		button.interactable = (true);
		loading.SetActive(false);
	}
}
