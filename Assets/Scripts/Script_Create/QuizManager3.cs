using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For legacy Text

public class QuizManager3 : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] options = new string[3];
        public int correctOptionIndex;
        public Sprite questionImage;
    }

    [Header("UI References")]
    public Text questionText;
    public Text FinalText;    
    public Button[] optionButtons;
    public GameObject correctPanel;
    public GameObject tryAgainPanel;
    public Image questionImage;
    public Text timerText; // CHANGED
    public Text scoreText; // CHANGED
    public GameObject QuizCanvas;
    public GameObject ScoreCanvas;
    

    [Header("Audio and Particles")]
    public AudioSource correctAudioSource;
    public ParticleSystem confetti1;
    public ParticleSystem confetti2;

    [Header("Question Images")]
    public Sprite image1;
    public Sprite image2;
    public Sprite image3;
    public Sprite resisterImage;
    public Sprite breadBoardImage;
    public Sprite ledImage;


	private List<Question> questions = new List<Question>();
    private int currentQuestionIndex = 0;
    private int score = 0;

    private float totalTime = 60f;
    private float remainingTime;

    public UpdateScore updateScore;

    public bool isQuizEnded = false;

	void Start()
    {
        remainingTime = totalTime;
        UpdateScoreUI();
        LoadQuestions();
        ShowQuestion();

        if (confetti1 != null) confetti1.gameObject.SetActive(false);
        if (confetti2 != null) confetti2.gameObject.SetActive(false);

        if (tryAgainPanel != null) tryAgainPanel.SetActive(false);
    }

    void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = Mathf.Ceil(remainingTime).ToString() + "s";
        }
        else if(!isQuizEnded)
		{
            EndQuiz();
            isQuizEnded = true;
		}
    }

    void LoadQuestions()
    {
        questions.Add(new Question
        {
            questionText = "What does Arduino Do?",
            options = new string[] { "Power Like Battery", "Acts as Brain", "Only Wire" },
            correctOptionIndex = 1,
            questionImage = image1
        });

        questions.Add(new Question
        {
            questionText = "What does an IR Sensor Do?",
            options = new string[] { "Sense Invisible Infrared Light", "Acts as Brain", "Only Wire" },
            correctOptionIndex = 0,
            questionImage = image2
        });

        questions.Add(new Question
        {
            questionText = "Why do we use a resistor with an LED?",
            options = new string[] { "To make the LED change color", "To protect LED", "To increase the voltage" },
            correctOptionIndex = 1,
            questionImage = resisterImage
        });

		questions.Add(new Question
		{
			questionText = "What is an LED?",
			options = new string[] { "A sensor that feels light", "A light that can turn on and off", "A motor that spins around" },
			correctOptionIndex = 1,
			questionImage = ledImage
		});

		questions.Add(new Question
		{
			questionText = "Why we write a program (code) for the Arduino?",
			options = new string[] { "Set LED color", "Control LED on/off", "Change wires" },
			correctOptionIndex = 1,
			questionImage = image1
		});

		questions.Add(new Question
		{
			questionText = "Which function is used to continuously run code on the Arduino?",
			options = new string[] { "setup()", "loop()", "start()" },
			correctOptionIndex = 1,
			questionImage = image1
		});

		questions.Add(new Question
		{
			questionText = "What does the delay(1000); command do?",
			options = new string[] { "Turns off the LED", "Stops the program", "Pauses the program for 1 second" },
			correctOptionIndex = 2,
			questionImage = image1
		});

		questions.Add(new Question
		{
			questionText = "How does the IR sensor detect an object?",
			options = new string[] { "By receiving reflected infrared light ", "By sensing the object’s color", "By detecting sound" },
			correctOptionIndex = 0,
			questionImage = image3
		});

		questions.Add(new Question
		{
			questionText = "When the IR sensor detects moving object, what does the Arduino do?",
			options = new string[] { "Stops all sensors ", "Follows the object", "Activates an alarm" },
			correctOptionIndex = 1,
			questionImage = image3
		});

        questions.Add(new Question
        {
            questionText = "Why we use a breadboard in electronic projects?",
            options = new string[] { "Project look pretty", "No soldering needed", "Charges the Arduino faster" },
            correctOptionIndex = 1,
            questionImage = breadBoardImage
		});
	}

    void ShowQuestion()
    {
        correctPanel.SetActive(false);

        if (currentQuestionIndex >= questions.Count)
        {
            EndQuiz();
            return;
        }

        Question current = questions[currentQuestionIndex];
        questionText.text = current.questionText;
        questionImage.sprite = current.questionImage;
        questionImage.enabled = true;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = current.options[i]; // CHANGED
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == questions[currentQuestionIndex].correctOptionIndex)
        {
            score += 3;
            UpdateScoreUI();
            StartCoroutine(ShowCorrectThenNext());
        }
        else
        {
            Debug.Log("Wrong answer! Try again.");
            StartCoroutine(ShowTryAgainPanel());
        }
    }

    IEnumerator ShowTryAgainPanel()
    {
        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (tryAgainPanel != null)
            tryAgainPanel.SetActive(false);
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    IEnumerator ShowCorrectThenNext()
    {
        correctPanel.SetActive(true);

        if (correctAudioSource != null && PlayerPrefs.GetInt("MusicEnabled") == 1)
            correctAudioSource.Play();

        if (confetti1 != null)
        {
            confetti1.gameObject.SetActive(true);
            confetti1.Play();
        }
        if (confetti2 != null)
        {
            confetti2.gameObject.SetActive(true);
            confetti2.Play();
        }

        yield return new WaitForSeconds(3f);

        if (correctAudioSource != null && correctAudioSource.isPlaying)
            correctAudioSource.Stop();

        if (confetti1 != null)
        {
            confetti1.Stop();
            confetti1.gameObject.SetActive(false);
        }
        if (confetti2 != null)
        {
            confetti2.Stop();
            confetti2.gameObject.SetActive(false);
        }

        correctPanel.SetActive(false);
        NextQuestion();
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
        ShowQuestion();
    }

    void EndQuiz()
    {
        FinalText.text = "Total Score : "+score;

        updateScore.Submit(score);

		questionImage.enabled = false;

        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
        }

        timerText.gameObject.SetActive(false);

        QuizCanvas.SetActive(false);
        ScoreCanvas.SetActive(true);
    }
}
