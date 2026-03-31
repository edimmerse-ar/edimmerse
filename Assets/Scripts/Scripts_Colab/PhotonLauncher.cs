using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
	public List<RoomInfo> roomList = new List<RoomInfo>();
	public RoomListUI roomListUI;

	public TMP_InputField roomNameInput;
	private string roomName = null;

	public GameObject loadingObject;
	public GameObject roomPanel;

	public SceneHandler sceneHandler;
	void Start()
	{
		string playerName = GlobalVariables.PlayerName;

		if (string.IsNullOrEmpty(playerName))
		{
			playerName = GenerateRandomPlayerName();
		}

		PhotonNetwork.NickName = playerName;

		Debug.Log("Player Name: " + playerName);

		roomNameInput.onValueChanged.AddListener(OnRoomNameChanged);

		PhotonNetwork.ConnectUsingSettings();
	}

	string GenerateRandomPlayerName()
	{
		string[] tech = { "Volt", "Amp", "Ohm", "Flux", "Pulse", "Wave" };
		string[] roles = { "Rider", "Hunter", "Master", "Pilot", "Runner" };

		string t = tech[Random.Range(0, tech.Length)];
		string r = roles[Random.Range(0, roles.Length)];
		int number = Random.Range(10, 99);

		return t + r + number;
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		loadingObject.SetActive(false);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomListUpdate)
	{
		Debug.Log("Room list updated. Total rooms: " + roomListUpdate.Count);
		roomList.Clear();

		foreach (RoomInfo room in roomListUpdate)
		{
			if (!room.RemovedFromList)
			{
				roomList.Add(room);
				Debug.Log($"Room: {room.Name}, Players: {room.PlayerCount}/{room.MaxPlayers}");
			}
		}

		roomList.Clear();

		foreach (RoomInfo room in roomListUpdate)
		{
			if (!room.RemovedFromList)
			{
				roomList.Add(room);
			}
		}

		roomListUI.UpdateRoomList(roomList);
	}

	void OnRoomNameChanged(string newName)
	{
		roomName = newName;
	}

	public void CreateRoom()
	{
		// If empty → generate random name
		if (string.IsNullOrEmpty(roomName))
		{
			roomName = GenerateRandomRoomName();
		}

		Debug.Log("Creating Room: " + roomName);

		PhotonNetwork.CreateRoom(
			roomName,
			new RoomOptions { MaxPlayers = 100 },
			TypedLobby.Default
		);
	}

	string GenerateRandomRoomName()
	{
		string[] words =
		{
		"Quantum", "Neutron", "Photon", "Electron", "Flux",
		"Pulse", "Matrix", "Core", "CircuitX", "Nano",
		"Binary", "Logic", "Vector", "Phase", "Spectrum"
	};

		string word = words[Random.Range(0, words.Length)];
		int number = Random.Range(100, 999);

		return word + "_" + number;
	}

	public void JoinRoom(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}

	public override void OnJoinedRoom()
	{
		roomPanel.SetActive(false);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnected from Photon: " + cause);

		loadingObject.SetActive(false);

		sceneHandler.GoToScene("DIY");
	}
}