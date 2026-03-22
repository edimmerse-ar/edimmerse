using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerStats : MonoBehaviourPunCallbacks
{
	private const string DROP_KEY = "DropCount";

	void Start()
	{
		// initialize drop count = 0
		Hashtable props = new Hashtable();
		props[DROP_KEY] = 0;

		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}

	public static void IncrementDrop()
	{
		var player = PhotonNetwork.LocalPlayer;

		int current = 0;

		if (player.CustomProperties.ContainsKey(DROP_KEY))
			current = (int)player.CustomProperties[DROP_KEY];

		Hashtable props = new Hashtable();
		props[DROP_KEY] = current + 1;

		player.SetCustomProperties(props);
	}

	public static int GetDropCount(Photon.Realtime.Player player)
	{
		if (player.CustomProperties.ContainsKey(DROP_KEY))
			return (int)player.CustomProperties[DROP_KEY];

		return 0;
	}
}