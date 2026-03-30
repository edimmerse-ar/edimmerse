using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine;
using static ComponentManager;

public class PlayerListUI : MonoBehaviourPunCallbacks
{
	public GameObject playerItemPrefab;
	public Transform contentParent;

	public TextMeshProUGUI ranked;

	private void Start()
	{
		
	}

	public override void OnEnable()
	{
		base.OnEnable(); // VERY IMPORTANT

		if (ComponentManager.Instance != null)
			ComponentManager.Instance.onComponentPlaced += OnComponentPlaced;
	}

	public override void OnDisable()
	{
		base.OnDisable(); // VERY IMPORTANT

		if (ComponentManager.Instance != null)
			ComponentManager.Instance.onComponentPlaced -= OnComponentPlaced;
	}

	private void OnComponentPlaced()
	{
		RefreshList();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RefreshList();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshList();
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		RefreshList();
	}


	public void RefreshList()
	{
		Player localPlayer = PhotonNetwork.LocalPlayer;

		// clear old
		foreach (Transform child in contentParent)
		{
			Destroy(child.gameObject);
		}

		// SORT players by DropCount (highest first)
		var sortedPlayers = PhotonNetwork.PlayerList
			.OrderByDescending(player => PlayerStats.GetDropCount(player))
			.ToList();

		int i = 0;
		// rebuild list
		foreach (Player player in sortedPlayers)
		{
			GameObject item = Instantiate(playerItemPrefab, contentParent);

			PlayerItemUI ui = item.GetComponent<PlayerItemUI>();

			ui.SetData(
				player.NickName,
				PlayerStats.GetDropCount(player)
			);

			i++;
			if (player.NickName == localPlayer.NickName)
			{
				ranked.text = "Ranked : " + i;
			}
		}
	}
}