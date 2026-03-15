using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Experiment
{
	public string experimentName;

	// components required
	public List<string> requiredComponents = new List<string>();

	// message to show
	public string message;

	// prevent repeating
	public bool triggered = false;
}

public class LearnController : MonoBehaviour
{
	// List of all components to be discovered
	private List<string> components = new List<string>();

	// Categories
	private string[] categories = { "Brain", "Circuits", "Senses", "Actions", "Power" };
	public Image[] categoryImages;

	public Image targetImage; // Assign this UI Image in the Inspector

	// Mapping from component → category
	private Dictionary<string, string> componentCategoryMap = new Dictionary<string, string>();

	public Image progressBar;
	public TextMeshProUGUI progressBarText;
	private int percent;

	public GameObject toast;
	public TextMeshProUGUI toastMsg;
	public GameObject experimentObject;
	public TextMeshProUGUI experimentTitle;
	public TextMeshProUGUI experimentText;
	public TextMeshProUGUI experimentComponent;

	private int totalComponents;
	private HashSet<string> foundComponents = new HashSet<string>();

	private Dictionary<string, string> toastMessages;

	public GameObject completePanel;

	public UpdateScore updateScore;

	private List<Experiment> experiments = new List<Experiment>();

	public GameObject scanningText;

	void Start()
	{
		// Example: Fill all components
		components.AddRange(new string[] {
			"Arduino", "Raspberry",
			"Breadboard", "Wires", "Resistor", "Potentiometer", "Battery",
			"Temprature", "UltrasonicSensor", "PIR", "LDRModule", "IR",
			"DCMotor", "ServoMotor",
			"LCD", "LED", "Button", "Buzzer"
		});

		// Map each component to a category
		componentCategoryMap["Arduino"] = "Brain";//1
		componentCategoryMap["Raspberry"] = "Brain";//2

		componentCategoryMap["Breadboard"] = "Circuits";//3
		componentCategoryMap["Wires"] = "Circuits";//4
		componentCategoryMap["Resistor"] = "Circuits";//5
		componentCategoryMap["Potentiometer"] = "Circuits";//6
		componentCategoryMap["Battery"] = "Power";//7

		componentCategoryMap["Temprature"] = "Senses";//8
		componentCategoryMap["UltrasonicSensor"] = "Senses";//9
		componentCategoryMap["PIR"] = "Senses";//10
		componentCategoryMap["LDRModule"] = "Senses";//11
		componentCategoryMap["IR"] = "Senses";//12

		componentCategoryMap["DCMotor"] = "Actions";//13
		componentCategoryMap["ServoMotor"] = "Actions";//14

		componentCategoryMap["LCD"] = "Actions";//15
		componentCategoryMap["LED"] = "Actions";//16
		componentCategoryMap["Button"] = "Actions";//17
		componentCategoryMap["Buzzer"] = "Actions";//18

		totalComponents = components.Count;

		progressBar.fillAmount = 0f;
		progressBarText.text = "0%";
		toast.SetActive(false);


		toastMessages = new Dictionary<string, string>()
		{
            // 🧠 Controllers & Boards
            { "Arduino", "Arduino Uno (The Brain):\n Unlocked the Brain – your robot can now think and process signals!" },
			{ "Raspberry", "Raspberry Pi (The Super Brain):\n Super Brain added – advanced computing unlocked!" },
			{ "Breadboard", "Breadboard (The Circuit Playground):\n Circuit Playground ready – time to prototype connections." },
			{ "Wires", "Connecting Wires (The Robot’s Nerves):\nNerves connected – signals can now flow." },

            // ⚡ Circuit Components
            { "Resistors", "Resistors (The Circuit’s Brakes):\n Brakes installed – current is now under control." },
			{ "Potentiometer", "Potentiometer (The Adjustable Dial):\n Resistance tuned – smoothly controlling voltage or brightness!" },
			{ "Battery", "9V Battery (The Robot’s Food):\n Power online – your robot just got energy!" },

            // 🌡️ Sensors
            { "Temperature", "Temperature Sensor (DHT11 – The Thermometer):\n Thermometer added – your robot can sense heat & cold." },
			{ "UltrasonicSensor", "Ultrasonic Sensor (The Distance Checker):\n Distance checker unlocked – now it can measure range." },
			{ "PIR", "PIR Sensor (The Motion Detector):\n Motion detector ready – movement can now be sensed." },
			{ "LDRModule", "LDR Module (The Light Sensor):\n Light sensor installed – your robot can see brightness." },
			{ "IR", "IR Sensor (The Invisible Eye):\n Infrared eye active – invisible beams detected." },

            // 🎭 Outputs & Actions
            { "LCD", "16x2 LCD (The Robot’s Face):\n Display online – your robot can now show data." },
			{ "DCMotor", "DC Motor (The Spinning Powerhouse):\n Powerhouse ready – wheels can now spin." },
			{ "ServoMotor", "Servo Motor (The Robot’s Arm):\n Arm active – precise movements enabled." },
			{ "LED", "LEDs (The Robot’s Signals):\n Signals unlocked – lights can now blink & warn." },
			{ "Button", "Push Button (The Robot’s Switch):\n Switch connected – you can now give commands." },
			{ "Buzzer", "Buzzer (The Robot’s Voice):\n Voice online – beep alerts are now possible." }
		};

		InitializeExperiments();
	}

	void InitializeExperiments()
	{
		experiments.Add(new Experiment()
		{
			experimentName = "Glow LED with Arduino",
			requiredComponents = new List<string> { "Arduino", "LED", "Resistor", "Wires" },
			message = "Experiment Unlocked!\nYou can now build: Glow LED with Arduino."
		});

		experiments.Add(new Experiment()
		{
			experimentName = "Human Detection Alarm",
			requiredComponents = new List<string> { "Arduino", "PIR", "Buzzer", "Wires" },
			message = "Experiment Unlocked!\nBuild a Human Detection Alarm using PIR."
		});

		experiments.Add(new Experiment()
		{
			experimentName = "Distance Measurement",
			requiredComponents = new List<string> { "Arduino", "UltrasonicSensor", "LCD", "Wires" },
			message = "Experiment Unlocked!\nDisplay distance on LCD."
		});

		experiments.Add(new Experiment()
		{
			experimentName = "Light Detection",
			requiredComponents = new List<string> { "Arduino", "LDRModule", "LED", "Resistor" },
			message = "Experiment Unlocked!\nCreate an Automatic Light System."
		});

		experiments.Add(new Experiment()
		{
			experimentName = "Servo Door System",
			requiredComponents = new List<string> { "Arduino", "ServoMotor", "Button", "Wires" },
			message = "Experiment Unlocked!\nCreate a Servo Door Control."
		});
	}

	public void OnMarkerFound(string component)
	{
		scanningText.SetActive(false);

		if (!components.Contains(component))
		{
			Debug.LogWarning("Unknown component: " + component);
			return;
		}

		LoadImage(component);
		targetImage.enabled = true;

		if (!foundComponents.Contains(component))
		{
			foundComponents.Add(component);
			CheckExperiments();

			Debug.Log(foundComponents.Count +" /"+ totalComponents);
			float progress = (float)foundComponents.Count / totalComponents;
			progressBar.fillAmount = progress;

			percent = Mathf.RoundToInt(progress * 100f);
			progressBarText.text = percent + "%";
		}

		HighlightCategory(componentCategoryMap[component]);

		string category = componentCategoryMap[component];
		if (toastMessages.ContainsKey(component))
		{
			StopAllCoroutines();
			toastMsg.text = toastMessages[component];
			toast.SetActive(true);
			StartCoroutine(ShowToast());
		}
	}

	public void onMarkerLost()
	{
		HighlightCategory("");
		targetImage.enabled =false;
		scanningText.SetActive(true);
	}

	void CheckExperiments()
	{
		foreach (Experiment exp in experiments)
		{
			if (exp.triggered)
				continue;

			bool allFound = true;

			foreach (string comp in exp.requiredComponents)
			{
				if (!foundComponents.Contains(comp))
				{
					allFound = false;
					break;
				}
			}

			if (allFound)
			{
				exp.triggered = true;

				toastMsg.text = exp.message;
				experimentObject.SetActive(true);
				experimentTitle.text = exp.experimentName;
				experimentText.text = exp.message;
				experimentComponent.text = "Required Components:\n- " + string.Join(",", exp.requiredComponents);

				Debug.Log("Experiment unlocked: " + exp.experimentName);
			}
		}
	}

	private void HighlightCategory(string category)
	{
		for (int i = 0; i < categories.Length; i++)
		{
			if (i < categoryImages.Length)
			{
				if (categories[i] == category)
				{
					categoryImages[i].color = Color.green;
				}
				else
				{
					categoryImages[i].color = Color.white;
				}
			}
		}
	}

	private IEnumerator ShowToast()
	{
		toast.SetActive(true);

		if (percent > 95)
		{
			yield return new WaitForSeconds(1f);
			toast.SetActive(false);
			completePanel.SetActive(true);
			updateScoreInLeaderboard();
		}

		yield return new WaitForSeconds(5f);
		toast.SetActive(false);
	}

	public void LoadImage(string componentName)
	{
		Sprite sprite = Resources.Load<Sprite>("Learn_Components/" + componentName);
		if (sprite != null)
		{
			targetImage.sprite = sprite;
			targetImage.preserveAspect = true; // keep correct proportions
		}
		else
		{
			Debug.LogWarning("❌ Image not found for: " + componentName);
		}
	}

	public void updateScoreInLeaderboard()
	{
        // percent is an int (0-100). Calculate score as percent/10 with proper rounding
        int score = Mathf.RoundToInt(percent / 10f);
		updateScore.Submit(score);
	}
}
