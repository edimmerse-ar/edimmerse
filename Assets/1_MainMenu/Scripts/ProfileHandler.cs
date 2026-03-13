using UnityEngine;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
	public Sprite[] Profile;
	public Image uiImage;
	public Text scoreText;

	private void Start()
	{
		uiImage.sprite = Profile[GlobalVariables.character];
		int Coins = GlobalVariables.score;
		scoreText.text = Coins.ToString();
	}
}
