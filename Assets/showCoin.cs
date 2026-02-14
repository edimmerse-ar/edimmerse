using UnityEngine;
using UnityEngine.UI;

public class showCoin : MonoBehaviour
{
    public Text displayText; 

    void Start()
    {
        int Coins = GlobalVariables.score;
        displayText.text = Coins.ToString();
    }
}
