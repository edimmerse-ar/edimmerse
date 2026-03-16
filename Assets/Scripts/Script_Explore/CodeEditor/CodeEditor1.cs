using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeEditor1 : MonoBehaviour
{
    [Header("Pin Mode Selector UI")]
    public GameObject pinModeSelector;
    public Slider pinSliderPM;
    public TextMeshProUGUI pinTextPM;
    public Toggle highTogglePM;
    public Toggle lowTogglePM;

    [Header("Digital Write Selector UI")]
    public GameObject digitalWriteSelector;
    public Slider pinSliderDW;
    public TextMeshProUGUI pinTextDW;
    public Toggle highToggleDW;
    public Toggle lowToggleDW;

    [Header("Delay Selector UI")]
    public GameObject delaySelector;
    public Slider delaySlider;
    public TextMeshProUGUI delayValueText;

    [Header("Toast Reference")]
    public Explore1 explore1;

    [Header("State")]
    private int activeId = 0;
    private EditMode currentEditMode = EditMode.None;
    private bool hasShownFirstTimeToast = false;

    private List<PinMode> pinModeStates;
    private List<DigitalWrite> digitalWriteStates;
    private List<int> delayStates;

    public  TextMeshProUGUI[] pinModeText;
    public  TextMeshProUGUI[] digitalWriteText;
    public  TextMeshProUGUI[] delayText;
    public  TextMeshProUGUI fullCodeText;
    
    [Header("Light Blinking")]
    public Material lightMaterial;
    public Light lightSource;
    [SerializeField] private float maxLightIntensity = 3f;
    
    private Coroutine blinkCoroutine;

	[Header("Score")]
	public UpdateScore updateScore;
	
    private enum EditMode
    {
        None,
        PinMode,
        DigitalWrite,
        Delay
    }

    void Start()
    {
        InitializeStates();
        SetupListeners();
    }

    void OnDestroy()
    {
        RemoveListeners();
        StopBlinking();
    }

    public void StartCodeEditor()
    {
        if (!hasShownFirstTimeToast)
        {
            hasShownFirstTimeToast = true;
            ShowToast("Start editing the given code. Once done, run the program.");
        }
    }

    public void RunCode()
    {
        // Check if hardware setup is complete before allowing to run
        if (explore1 != null && !explore1.IsHardwareSetupComplete())
        {
            updateScore.Submit(-1);
            ShowToast("Please complete the hardware connection first before running the program.");
            return;
        }

        // Validate pinMode[0]: should be pinMode(13, OUTPUT)
        if (!IsValidIndex(pinModeStates, 0))
        {
			updateScore.Submit(-1);
			ShowToast("Error: pinMode configuration is missing.");
            return;
        }

        PinMode pm0 = pinModeStates[0];
        if (pm0.pin != 13)
        {
			updateScore.Submit(-1);
			ShowToast($"pinMode pin should be 13, not {pm0.pin}. Please fix it.");
            return;
        }
        if (!pm0.high) // high = true means OUTPUT
        {
			updateScore.Submit(-1);
			ShowToast("pinMode mode should be OUTPUT, not INPUT. Please fix it.");
            return;
        }

        // Validate digitalWrite[0]: should be digitalWrite(13, HIGH)
        if (!IsValidIndex(digitalWriteStates, 0))
        {
			updateScore.Submit(-1);
			ShowToast("Error: First digitalWrite configuration is missing.");
            return;
        }

        DigitalWrite dw0 = digitalWriteStates[0];
        if (dw0.pin != 13)
        {
			updateScore.Submit(-1);
			ShowToast($"First digitalWrite pin should be 13, not {dw0.pin}. Please fix it.");
            return;
        }
        if (!dw0.high)
        {
			updateScore.Submit(-1);
			ShowToast("First digitalWrite should be HIGH, not LOW. Please fix it.");
            return;
        }

        // Validate digitalWrite[1]: should be digitalWrite(13, LOW)
        if (!IsValidIndex(digitalWriteStates, 1))
        {
			updateScore.Submit(-1);
			ShowToast("Error: Second digitalWrite configuration is missing.");
            return;
        }

        DigitalWrite dw1 = digitalWriteStates[1];
        if (dw1.pin != 13)
        {
			updateScore.Submit(-1);
			ShowToast($"Second digitalWrite pin should be 13, not {dw1.pin}. Please fix it.");
            return;
        }
        if (dw1.high)
        {
			updateScore.Submit(-1);
			ShowToast("Second digitalWrite should be LOW, not HIGH. Please fix it.");
            return;
        }

		PlayerPrefs.SetInt("unlock", 2);
		updateScore.Submit(10);
		// All validations passed - code is correct!
		ShowToast("Great job! Your code is correct. Running program...");
        
        // Start blinking with current delay values
        StartBlinking();
    }

    #region Light Blinking

    private void StartBlinking()
    {
        // Stop any existing blinking coroutine
        StopBlinking();
        
        // Get delay values (in milliseconds, convert to seconds)
        float onDuration = delayStates[0] / 1000f;
        float offDuration = delayStates[1] / 1000f;
        
        // Start new blinking coroutine
        blinkCoroutine = StartCoroutine(BlinkLightCoroutine(onDuration, offDuration));
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        
        // Turn off light when stopping
        SetLightState(false);
    }

    private IEnumerator BlinkLightCoroutine(float onDuration, float offDuration)
    {
        while (true)
        {
            // Light ON
            SetLightState(true);
            yield return new WaitForSeconds(onDuration);
            
            // Light OFF
            SetLightState(false);
            yield return new WaitForSeconds(offDuration);
        }
    }

    private void SetLightState(bool isOn)
    {
        float alpha = isOn ? 1f : 0f;
        float intensity = isOn ? maxLightIntensity : 0f;
        
        if (lightMaterial != null)
        {
            lightMaterial.SetFloat("_Alpha", alpha);
        }
        
        if (lightSource != null)
        {
            lightSource.intensity = intensity;
        }
    }

    #endregion

    private void ShowToast(string message)
    {
        if (explore1 != null)
        {
            explore1.ShowToastMessage(message);
        }
        else
        {
            Debug.LogWarning("explore1 reference is not set. Toast message: " + message);
        }
    }


    private void InitializeStates()
    {
        pinModeStates = new List<PinMode>
        {
            new PinMode() { pin = 1, high = false },
        };

        digitalWriteStates = new List<DigitalWrite>
        {
            new DigitalWrite() { pin = 1, high = false },
            new DigitalWrite() { pin = 1, high = false }
        };

        delayStates = new List<int>
        {
            1000,
            1000
        };

        UpdateFullCodeText();
    }

    private void SetupListeners()
    {
        // PinMode listeners
        highTogglePM.onValueChanged.AddListener(OnHighTogglePMChanged);
        lowTogglePM.onValueChanged.AddListener(OnLowTogglePMChanged);
        pinSliderPM.onValueChanged.AddListener(OnPinSliderPMChanged);
        
        // DigitalWrite listeners
        highToggleDW.onValueChanged.AddListener(OnHighToggleDWChanged);
        lowToggleDW.onValueChanged.AddListener(OnLowToggleDWChanged);
        pinSliderDW.onValueChanged.AddListener(OnPinSliderDWChanged);
        
        // Delay listener
        delaySlider.onValueChanged.AddListener(OnDelaySliderChanged);
    }

    private void RemoveListeners()
    {
        // PinMode listeners
        if (highTogglePM != null)
            highTogglePM.onValueChanged.RemoveListener(OnHighTogglePMChanged);
        if (lowTogglePM != null)
            lowTogglePM.onValueChanged.RemoveListener(OnLowTogglePMChanged);
        if (pinSliderPM != null)
            pinSliderPM.onValueChanged.RemoveListener(OnPinSliderPMChanged);
        
        // DigitalWrite listeners
        if (highToggleDW != null)
            highToggleDW.onValueChanged.RemoveListener(OnHighToggleDWChanged);
        if (lowToggleDW != null)
            lowToggleDW.onValueChanged.RemoveListener(OnLowToggleDWChanged);
        if (pinSliderDW != null)
            pinSliderDW.onValueChanged.RemoveListener(OnPinSliderDWChanged);
        
        // Delay listener
        if (delaySlider != null)
            delaySlider.onValueChanged.RemoveListener(OnDelaySliderChanged);
    }

    #region PinMode Handlers

    private void OnHighTogglePMChanged(bool isOn)
    {
        if (!isOn || currentEditMode != EditMode.PinMode) return;
        
        if (IsValidIndex(pinModeStates, activeId))
        {
            pinModeStates[activeId].high = true;
            UpdatePinModeText(activeId);
        }
    }

    private void OnLowTogglePMChanged(bool isOn)
    {
        if (!isOn || currentEditMode != EditMode.PinMode) return;
        
        if (IsValidIndex(pinModeStates, activeId))
        {
            pinModeStates[activeId].high = false;
            UpdatePinModeText(activeId);
        }
    }

    private void OnPinSliderPMChanged(float value)
    {
        int intValue = (int)value;
        pinTextPM.text = intValue.ToString();

        if (currentEditMode == EditMode.PinMode && IsValidIndex(pinModeStates, activeId))
        {
            pinModeStates[activeId].pin = intValue;
            UpdatePinModeText(activeId);
        }
    }

    #endregion

    #region DigitalWrite Handlers

    private void OnHighToggleDWChanged(bool isOn)
    {
        if (!isOn || currentEditMode != EditMode.DigitalWrite) return;
        
        if (IsValidIndex(digitalWriteStates, activeId))
        {
            digitalWriteStates[activeId].high = true;
            UpdateDigitalWriteText(activeId);
        }
    }

    private void OnLowToggleDWChanged(bool isOn)
    {
        if (!isOn || currentEditMode != EditMode.DigitalWrite) return;
        
        if (IsValidIndex(digitalWriteStates, activeId))
        {
            digitalWriteStates[activeId].high = false;
            UpdateDigitalWriteText(activeId);
        }
    }

    private void OnPinSliderDWChanged(float value)
    {
        int intValue = (int)value;
        pinTextDW.text = intValue.ToString();

        if (currentEditMode == EditMode.DigitalWrite && IsValidIndex(digitalWriteStates, activeId))
        {
            digitalWriteStates[activeId].pin = intValue;
            UpdateDigitalWriteText(activeId);
        }
    }

    #endregion

    #region Delay Handler

    private void OnDelaySliderChanged(float value)
    {
        int intValue = (int)value;
        delayValueText.text = intValue.ToString();

        if (currentEditMode == EditMode.Delay && IsValidIndex(delayStates, activeId))
        {
            delayStates[activeId] = intValue;
            UpdateDelayText(activeId);
        }
    }

    #endregion

    private void UpdatePinModeText(int id)
    {
        if (!IsValidIndex(pinModeStates, id))
            return;

        PinMode state = pinModeStates[id];
        string mode = state.high ? "OUTPUT" : "INPUT";
        
        // Update individual pinMode text if available
        if (pinModeText != null && id < pinModeText.Length && pinModeText[id] != null)
        {
            pinModeText[id].text = $"<b>pinMode (<color=\"blue\"> {state.pin} </color>,<color=\"green\"> {mode} </color>)";
        }
        
        // Always update the full code text
        UpdateFullCodeText();
    }

    private void UpdateDigitalWriteText(int id)
    {
        if (!IsValidIndex(digitalWriteStates, id))
            return;

        DigitalWrite state = digitalWriteStates[id];
        string mode = state.high ? "HIGH" : "LOW";
        
        // Update individual digitalWrite text if available
        if (digitalWriteText != null && id < digitalWriteText.Length && digitalWriteText[id] != null)
        {
            digitalWriteText[id].text = $"<b>digitalWrite (<color=\"blue\"> {state.pin} </color>,<color=\"green\"> {mode} </color>)";
        }
        
        // Always update the full code text
        UpdateFullCodeText();
    }

    private void UpdateDelayText(int id)
    {
        if (!IsValidIndex(delayStates, id))
            return;

        // Update individual delay text if available
        if (delayText != null && id < delayText.Length && delayText[id] != null)
        {
            delayText[id].text = $"<b>delay (<color=\"red\"> {delayStates[id]} </color>)";
        }
        
        // Always update the full code text
        UpdateFullCodeText();
    }

    private void UpdateFullCodeText()
    {
        if (fullCodeText == null)
            return;

        System.Text.StringBuilder code = new System.Text.StringBuilder();

        // void setup()
        code.AppendLine("void setup()");
        code.AppendLine("{");
        
        // Add all pinMode statements
        foreach (PinMode pm in pinModeStates)
        {
            string mode = pm.high ? "OUTPUT" : "INPUT";
            code.AppendLine($"    pinMode( {pm.pin}, {mode});");
        }
        
        code.AppendLine("}");
        code.AppendLine();

        // void loop()
        code.AppendLine("void loop()");
        code.AppendLine("{");
        
        // Add digitalWrite and delay pairs
        for (int i = 0; i < digitalWriteStates.Count; i++)
        {
            DigitalWrite dw = digitalWriteStates[i];
            string mode = dw.high ? "HIGH" : "LOW";
            code.AppendLine($"    digitalWrite( {dw.pin}, {mode});");
            
            // Add corresponding delay if exists
            if (IsValidIndex(delayStates, i))
            {
                code.AppendLine($"    delay ({delayStates[i]});");
            }
        }
        
        code.AppendLine("}");

        fullCodeText.text = code.ToString();
    }

    public void editPinMode(int id)
    {
        if (!IsValidIndex(pinModeStates, id))
        {
            Debug.LogWarning($"Invalid PinMode index: {id}");
            return;
        }

        activeId = id;
        currentEditMode = EditMode.PinMode;
        pinModeSelector.SetActive(true);

        PinMode state = pinModeStates[activeId];
        UpdateUI(state.pin, state.high);
    }

    public void editDigitalWrite(int id)
    {
        if (!IsValidIndex(digitalWriteStates, id))
        {
            Debug.LogWarning($"Invalid DigitalWrite index: {id}");
            return;
        }

        activeId = id;
        currentEditMode = EditMode.DigitalWrite;
        digitalWriteSelector.SetActive(true);

        DigitalWrite state = digitalWriteStates[activeId];
        UpdateUIDW(state.pin, state.high);
    }

    public void editDelay(int id)
    {
        if (!IsValidIndex(delayStates, id))
        {
            Debug.LogWarning($"Invalid Delay index: {id}");
            return;
        }

        activeId = id;
        currentEditMode = EditMode.Delay;
        delaySelector.SetActive(true);

        delaySlider.value = delayStates[activeId];
        delayValueText.text = delayStates[activeId].ToString();
    }

    private void UpdateUI(int pin, bool high)
    {
        pinSliderPM.value = pin;
        pinTextPM.text = pin.ToString();
        highTogglePM.isOn = high;
        lowTogglePM.isOn = !high;
    }

    private void UpdateUIDW(int pin, bool high)
    {
        pinSliderDW.value = pin;
        pinTextDW.text = pin.ToString();
        highToggleDW.isOn = high;
        lowToggleDW.isOn = !high;
    }

    private bool IsValidIndex<T>(List<T> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }

    public void ClosePinModeSelector()
    {
        pinModeSelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

    public void CloseDigitalWriteSelector()
    {
        digitalWriteSelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

    public void CloseDelaySelector()
    {
        delaySelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

    public void CloseAllSelectors()
    {
        pinModeSelector.SetActive(false);
        digitalWriteSelector.SetActive(false);
        delaySelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

}
