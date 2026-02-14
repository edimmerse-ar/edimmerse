using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreGen : MonoBehaviour
{
    public Image[] uiImage;
    public Color newColor = Color.yellow;
    public GameObject ScoreCard,Loose,Poppers;
    public Text ScoreText, ErrorText, TimerText;
    public int TotalError = 0;
    public int TotalScore = 0;
    public int flimit = 4;
    public int llimit = 7;
    

    private float timer;
    private bool isRunning;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            TimerText.text = Mathf.FloorToInt(timer / 60).ToString() + " min : "+ Mathf.FloorToInt(timer % 60) + " s";
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }


    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0f;
        isRunning = false;
    }

    public void ShowScore()
    {
        if(TotalScore - TotalError==0||TotalScore-TotalError<0){
                    Loose.SetActive(true);
        }else{
        int currentUnlock = PlayerPrefs.GetInt("unlock", 1);
        currentUnlock++;
        PlayerPrefs.SetInt("unlock", currentUnlock);
        int currentCoin = PlayerPrefs.GetInt("coinData", 0);
        currentCoin=currentCoin+10;
                PlayerPrefs.SetInt("coinData", currentCoin);

        PlayerPrefs.Save();
        ScoreCard.SetActive(true);
        Poppers.SetActive(true);
        int ScoreD = 0;
        if (TotalError > TotalScore)
        {
            ScoreD = 0;
        }
        else
        {
            ScoreD = TotalScore - TotalError;
        }
		GlobalVariables.updateScore(ScoreD);
		ScoreText.text = ScoreD.ToString();
        ErrorText.text = TotalError.ToString();
        if (ScoreD < flimit && ScoreD != 0)
        {
            uiImage[0].color = newColor;
        }
        else if (ScoreD < llimit)
        {
            uiImage[0].color = newColor;
            uiImage[1].color = newColor;

        }
        else
        {
            uiImage[0].color = newColor;
            uiImage[1].color = newColor;
            uiImage[2].color = newColor;

        }
        StopTimer();
            TimerText.text = Mathf.FloorToInt(timer / 60).ToString() + " min : "+ Mathf.FloorToInt(timer % 60) + " s";
        ResetTimer();}
    }
}
