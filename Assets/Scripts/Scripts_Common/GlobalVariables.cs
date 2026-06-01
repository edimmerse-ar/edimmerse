using Dan.Main;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static int character = 1; // Global variable
    public static string PlayerName;
    public static string PlayerAge;
    public static string PlayerProfilePhoto;
    public static string PlayerMail;
    public static int sessionScore = 0;
	public static int score = 0;

	public static void updateScore(int scoreAug)
	{
		GlobalVariables.sessionScore+=scoreAug;
		Debug.Log("sessionScore" + sessionScore);
		Debug.Log("scoreAug" + scoreAug);
		Debug.Log("score" + GlobalVariables.score);
		if (GlobalVariables.sessionScore > GlobalVariables.score && GlobalVariables.sessionScore >=0)
		{
			Debug.Log("update leaderboard score" + GlobalVariables.sessionScore);
			Leaderboards.EdImmerse.UploadNewEntry(PlayerName, GlobalVariables.sessionScore, GlobalVariables.Callback, GlobalVariables.ErrorCallback);
			PlayerPrefs.SetInt("Score", GlobalVariables.sessionScore);
			PlayerPrefs.Save();
			GlobalVariables.score = GlobalVariables.sessionScore;
		}
	}

	static void Callback(bool success)
	{

	}

	static void ErrorCallback(string error)
	{
		Debug.LogError(error);
	}

}