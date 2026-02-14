using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform leaderboardContainer;
	[SerializeField] private GameObject leaderboardEntryPrefab;
	
	private List<PlayerData> playerDataList = new List<PlayerData>
	{
		new PlayerData("Alice", 120),
		new PlayerData("Bob", 95),
		new PlayerData("Charlie", 200),
		new PlayerData("Diana", 150)
	};

	void Start()
	{
		PopulateLeaderboard(playerDataList);
	}

	public void PopulateLeaderboard(List<PlayerData> data)
	{
		foreach (Transform child in leaderboardContainer)
		{
			Destroy(child.gameObject);
		}

		data.Sort((a, b) => b.score.CompareTo(a.score));

		foreach (var player in data)
		{
			GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

			TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();
			if (texts.Length >= 2)
			{
				texts[0].text = player.username;
				texts[0].enabled = true;
				texts[1].text = player.score.ToString();
				texts[1].enabled = true;
			}

			Image bg = entry.GetComponent<Image>();
			if (bg != null)
			{
				bg.color = player == data[0]
					? new Color(1f, 0.9f, 0.5f, 1f)
					: new Color(1f, 1f, 1f, 0.85f);
			}
		}
	}
}

[System.Serializable]
public class PlayerData
{
	public string username;
	public int score;

	public PlayerData(string username, int score)
	{
		this.username = username;
		this.score = score;
	}
}
