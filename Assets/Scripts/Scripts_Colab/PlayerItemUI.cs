using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemUI : MonoBehaviour
{
	public TMP_Text nameText;
	public TMP_Text countText;

	public Image image;
	public void SetData(string playerName, int count)
	{
		nameText.text = playerName;

		countText.text = "" + count;

		setImageColor();
	}

	public void setImageColor()
	{
		image = GetComponent<Image>();
		if (Photon.Pun.PhotonNetwork.LocalPlayer.NickName == nameText.text)
		{
			image.color = Color.green;
		}
	}
}