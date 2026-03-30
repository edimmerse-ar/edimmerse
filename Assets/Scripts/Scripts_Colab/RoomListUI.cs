using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;

public class RoomListUI : MonoBehaviour
{
	public PhotonLauncher launcher;

	public Transform contentParent;
	public GameObject roomItemPrefab;

	public void UpdateRoomList(List<RoomInfo> rooms)
	{
		// Clear old UI
		foreach (Transform child in contentParent)
		{
			Destroy(child.gameObject);
		}

		// Create new UI items
		foreach (RoomInfo room in rooms)
		{
			GameObject obj = Instantiate(roomItemPrefab, contentParent);

			RoomItemUI item = obj.GetComponent<RoomItemUI>();
			item.Setup(room, launcher.JoinRoom);
		}
	}
}