using UnityEngine;
using TMPro;
public class ProfileStatsShowScript : MonoBehaviour
{
    public TMP_Text PlayerName;
    public TMP_Text PlayerAge;
    public TMP_Text PlayerMail;

    void Start()
    {
        PlayerName.text = GlobalVariables.PlayerName;
        PlayerAge.text = "AGE:"+GlobalVariables.PlayerAge;
        PlayerMail.text = "EMAIL:" + GlobalVariables.PlayerMail;
    }
}
