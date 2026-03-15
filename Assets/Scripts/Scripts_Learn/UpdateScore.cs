using TMPro;
using UnityEngine;

    public class UpdateScore : MonoBehaviour
    {
	public Animation scoreAnimation;
    public TextMeshProUGUI scoreText;

		// Start is called before the first frame update
		void Start()
        {
        
        }

        // Update is called once per frame
        public void Submit(int score)
        {
		scoreText.enabled = true;
		scoreAnimation.Play();
            scoreText.text = "SCORE: +" + score.ToString();
		    GlobalVariables.updateScore(score);
		}
	}
