using UnityEngine;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
	public Sprite[] Profile;
	public Image uiImage;
	public Text scoreText;

	private void Start()
	{
		// Load saved character selection from PlayerPrefs in case the static value was reset
		int savedCharacter = PlayerPrefs.GetInt("character", GlobalVariables.character);
		GlobalVariables.character = savedCharacter;
		Debug.Log("Character: " + GlobalVariables.character);

		if (Profile != null && Profile.Length > GlobalVariables.character && uiImage != null)
		{
			uiImage.sprite = Profile[GlobalVariables.character];
		}
		else
		{
			Debug.LogWarning($"ProfileHandler: profile sprite for index {GlobalVariables.character} not found.");
		}

		int Coins = GlobalVariables.score;
		scoreText.text = Coins.ToString();
	}
}
