using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
	void Start()
	{
		PhotonNetwork.NickName = GlobalVariables.PlayerName;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinOrCreateRoom(
			"ExperimentRoom",
			new RoomOptions { MaxPlayers = 20 },
			TypedLobby.Default
		);
	}

	public override void OnJoinedRoom()
	{

	}
}