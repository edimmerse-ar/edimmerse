using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerCountText;
    public Button joinButton;

    private string roomName;

    public void Setup(RoomInfo info, System.Action<string> onJoin)
    {
        roomName = info.Name;

        roomNameText.text = info.Name;
        playerCountText.text = $"{info.PlayerCount} / {info.MaxPlayers}";

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(() => onJoin(roomName));
    }
}