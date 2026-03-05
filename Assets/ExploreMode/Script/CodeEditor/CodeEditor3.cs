using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeEditor3 : MonoBehaviour
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

    [Header("Digital Read Selector UI")]
    public GameObject digitalReadSelector;
    public Slider pinSliderDR;
    public TextMeshProUGUI pinTextDR;

    [Header("Condition Selector UI")]   
    public GameObject conditionSelector;
    public Toggle value0Toggle;
    public Toggle value1Toggle;

    [Header("Toast Reference")]
    public Explore3 explore3;

    [Header("State")]
    private int activeId = 0;
    private EditMode currentEditMode = EditMode.None;
    private bool hasShownFirstTimeToast = false;

    [Header("Sensor Simulation (Optional)")]
    [SerializeField] private bool simulatedSensorHigh = false;
    private bool isProgramRunning = false;
    private int sensor = 0;

    private List<PinMode> pinModeStates;
    private DigitalRead digitalReadState;
    private List<Condition> conditionStates;
    private List<DigitalWrite> digitalWriteStates;

    [Tooltip("pinModeText[0] = first pinMode line, pinModeText[1] = second. Each must reference a different UI Text. Buttons must call editPinMode(0) and editPinMode(1) so the correct line updates.")]
    public TextMeshProUGUI[] pinModeText;
    public TextMeshProUGUI digitalReadText;
    public TextMeshProUGUI[] conditionText;
    public TextMeshProUGUI[] digitalWriteText;

    public  TextMeshProUGUI fullCodeText;
    
    [Header("Light Blinking")]
    public Material lightMaterial;
    public Light lightSource;
    [SerializeField] private float maxLightIntensity = 3f;

    public GameObject sensorPowerLed;
	public GameObject sensorSignalLed;
	public GameObject arduino13Led;

    public Transform sensorTransform;
    public BoxCollider obstacleBoxCollider;
    public GameObject obstacle;

	private enum EditMode
    {
        None,
        PinMode,
        DigitalRead,
        Condition,
        DigitalWrite
    }

    void Start()
    {
        InitializeStates();
        SetupListeners();
    }

    void Update()
    {
        if (RayFromTransformHitsBox(sensorTransform, obstacleBoxCollider, out RaycastHit hit, 10f))
        {
            sensorSignalLed.SetActive(true);
            if (explore3.isToastActive()) return;
            ShowToast("Obstacle detected! The sensor reads HIGH.");
		}
        else
        {
            sensorSignalLed.SetActive(false);
		}

		if (!isProgramRunning) return;

        sensor = simulatedSensorHigh ? 1 : 0;
	}

	public bool RayFromTransformHitsBox(Transform originTransform, BoxCollider box, out RaycastHit hit, float maxDistance = Mathf.Infinity)
	{
		hit = default;

		if (originTransform == null || box == null) return false;

		Ray ray = new Ray(originTransform.position, -1*originTransform.forward);
		return box.Raycast(ray, out hit, maxDistance);
	}

	void OnDestroy()
    {
        RemoveListeners();
        isProgramRunning = false;
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
        if (explore3 != null && !explore3.IsHardwareSetupComplete())
        {
            ShowToast("Please complete the hardware connection first before running the program.");
            return;
        }

        // Validate pinMode: one must be 7,INPUT and one 13,OUTPUT (order does not matter)
        if (!IsValidIndex(pinModeStates, 0) || !IsValidIndex(pinModeStates, 1))
        {
            ShowToast("Error: pinMode configuration is missing.");
            return;
        }

        PinMode pm0 = pinModeStates[0];
        PinMode pm1 = pinModeStates[1];
        bool pm0IsSensor = (pm0.pin == 7 && !pm0.high);  // 7, INPUT
        bool pm0IsLed = (pm0.pin == 13 && pm0.high);     // 13, OUTPUT
        bool pm1IsSensor = (pm1.pin == 7 && !pm1.high);
        bool pm1IsLed = (pm1.pin == 13 && pm1.high);

        if (!((pm0IsSensor && pm1IsLed) || (pm0IsLed && pm1IsSensor)))
        {
            ShowToast("One pinMode must be 7, INPUT and the other 13, OUTPUT. Please fix it.");
            return;
        }

        if (digitalReadState == null || digitalReadState.pin != 7)
        {
            ShowToast($"digitalRead pin should be 7, not {(digitalReadState?.pin ?? -1)}. Please fix it.");
            return;
        }

        if (!IsValidIndex(conditionStates, 0) || conditionStates[0].compareValue != 1)
        {
            ShowToast("First condition should be if (sensor == 1). Please fix it.");
            return;
        }
        if (!IsValidIndex(conditionStates, 1) || conditionStates[1].compareValue != 0)
        {
            ShowToast("Second condition should be if (sensor == 0). Please fix it.");
            return;
        }

        if (!IsValidIndex(digitalWriteStates, 0))
        {
            ShowToast("Error: First digitalWrite configuration is missing.");
            return;
        }

        DigitalWrite dw0 = digitalWriteStates[0];
        if (dw0.pin != 13)
        {
            ShowToast($"First digitalWrite pin should be 13, not {dw0.pin}. Please fix it.");
            return;
        }
        if (!dw0.high)
        {
            ShowToast("First digitalWrite should be HIGH, not LOW. Please fix it.");
            return;
        }

        // Validate digitalWrite[1]: should be digitalWrite(13, LOW)
        if (!IsValidIndex(digitalWriteStates, 1))
        {
            ShowToast("Error: Second digitalWrite configuration is missing.");
            return;
        }

        DigitalWrite dw1 = digitalWriteStates[1];
        if (dw1.pin != 13)
        {
            ShowToast($"Second digitalWrite pin should be 13, not {dw1.pin}. Please fix it.");
            return;
        }
        if (dw1.high)
        {
            ShowToast("Second digitalWrite should be LOW, not HIGH. Please fix it.");
            return;
        }

        // All validations passed - code is correct!
        ShowToast("Great job! Your code is correct. Program running, next you can move obstacle.");

		arduino13Led.SetActive(true);
        sensorPowerLed.SetActive(true);
		obstacle.SetActive(true);
		lightMaterial.SetFloat("_Alpha", 0.5f);

		isProgramRunning = true;
    }



    private void ShowToast(string message)
    {
        if (explore3 != null)
        {
            explore3.ShowToastMessage(message);
        }
        else
        {
            Debug.LogWarning("Explore3 reference is not set. Toast message: " + message);
        }
    }


    private void InitializeStates()
    {
        pinModeStates = new List<PinMode>
        {
            new PinMode() { pin = 1, high = false },
            new PinMode() { pin = 1, high = false },
        };

        digitalReadState = new DigitalRead { pin = 0 };

        conditionStates = new List<Condition>
        {
            new Condition() { compareValue = 0 },     
            new Condition() { compareValue = 0 },      
        };

        digitalWriteStates = new List<DigitalWrite>
        {
            new DigitalWrite() { pin = 1, high = false },
            new DigitalWrite() { pin = 1, high = false }
        };

        UpdatePinModeText(0);
        UpdatePinModeText(1);
        UpdateDigitalReadText();
        UpdateConditionText(0);
        UpdateConditionText(1);
        UpdateDigitalWriteText(0);
        UpdateDigitalWriteText(1);
        UpdateFullCodeText();
    }

    private void SetupListeners()
    {
        highTogglePM.onValueChanged.AddListener(OnHighTogglePMChanged);
        lowTogglePM.onValueChanged.AddListener(OnLowTogglePMChanged);
        pinSliderPM.onValueChanged.AddListener(OnPinSliderPMChanged);

        if (pinSliderDR != null)
            pinSliderDR.onValueChanged.AddListener(OnPinSliderDRChanged);

        if (value0Toggle != null)
            value0Toggle.onValueChanged.AddListener(OnValue0ToggleChanged);
        if (value1Toggle != null)
            value1Toggle.onValueChanged.AddListener(OnValue1ToggleChanged);

        highToggleDW.onValueChanged.AddListener(OnHighToggleDWChanged);
        lowToggleDW.onValueChanged.AddListener(OnLowToggleDWChanged);
        pinSliderDW.onValueChanged.AddListener(OnPinSliderDWChanged);
    }

    private void RemoveListeners()
    {
        if (highTogglePM != null)
            highTogglePM.onValueChanged.RemoveListener(OnHighTogglePMChanged);
        if (lowTogglePM != null)
            lowTogglePM.onValueChanged.RemoveListener(OnLowTogglePMChanged);
        if (pinSliderPM != null)
            pinSliderPM.onValueChanged.RemoveListener(OnPinSliderPMChanged);

        if (pinSliderDR != null)
            pinSliderDR.onValueChanged.RemoveListener(OnPinSliderDRChanged);
        if (value0Toggle != null)
            value0Toggle.onValueChanged.RemoveListener(OnValue0ToggleChanged);
        if (value1Toggle != null)
            value1Toggle.onValueChanged.RemoveListener(OnValue1ToggleChanged);

        if (highToggleDW != null)
            highToggleDW.onValueChanged.RemoveListener(OnHighToggleDWChanged);
        if (lowToggleDW != null)
            lowToggleDW.onValueChanged.RemoveListener(OnLowToggleDWChanged);
        if (pinSliderDW != null)
            pinSliderDW.onValueChanged.RemoveListener(OnPinSliderDWChanged);
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

    #region DigitalRead Handlers

    private void OnPinSliderDRChanged(float value)
    {
        int intValue = (int)value;
        if (pinTextDR != null)
            pinTextDR.text = intValue.ToString();
        if (currentEditMode == EditMode.DigitalRead && digitalReadState != null)
        {
            digitalReadState.pin = intValue;
            UpdateDigitalReadText();
        }
    }

    #endregion

    #region Condition Handlers

    private void OnValue0ToggleChanged(bool isOn)
    {
        if (!isOn || currentEditMode != EditMode.Condition) return;
        if (IsValidIndex(conditionStates, activeId))
        {
            conditionStates[activeId].compareValue = 0;
            UpdateConditionText(activeId);
        }
    }

    private void OnValue1ToggleChanged(bool isOn)
    {
        if (!isOn || currentEditMode != EditMode.Condition) return;
        if (IsValidIndex(conditionStates, activeId))
        {
            conditionStates[activeId].compareValue = 1;
            UpdateConditionText(activeId);
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

    private void UpdateDigitalReadText()
    {
        if (digitalReadState == null) return;
        if (digitalReadText != null)
            digitalReadText.text = $"<b>sensor = digitalRead (<color=\"blue\"> {digitalReadState.pin} </color>)</b>";
        UpdateFullCodeText();
    }

    private void UpdateConditionText(int id)
    {
        if (!IsValidIndex(conditionStates, id)) return;
        int val = conditionStates[id].compareValue;
        if (conditionText != null && id < conditionText.Length && conditionText[id] != null)
            conditionText[id].text = $"<b>if (sensor == <color=\"red\"> {val} </color>)</b>";
        UpdateFullCodeText();
    }

    private void UpdateFullCodeText()
    {
        if (fullCodeText == null)
            return;

        System.Text.StringBuilder code = new System.Text.StringBuilder();

        code.AppendLine("int sensor = 0;");
        code.AppendLine();

        // void setup()
        code.AppendLine("void setup()");
        code.AppendLine("{");

        // pinMode lines (expected: sensor INPUT, LED OUTPUT)
        if (IsValidIndex(pinModeStates, 0))
        {
            PinMode pmSensor = pinModeStates[0];
            code.AppendLine($"    pinMode({pmSensor.pin}, {(pmSensor.high ? "OUTPUT" : "INPUT")});");
        }
        if (IsValidIndex(pinModeStates, 1))
        {
            PinMode pmLed = pinModeStates[1];
            code.AppendLine($"    pinMode({pmLed.pin}, {(pmLed.high ? "OUTPUT" : "INPUT")});");
        }
        
        code.AppendLine("}");
        code.AppendLine();

        // void loop()
        code.AppendLine("void loop()");
        code.AppendLine("{");

        int readPin = digitalReadState != null ? digitalReadState.pin : 7;
        code.AppendLine($"    sensor = digitalRead({readPin});");

        for (int i = 0; i < digitalWriteStates.Count; i++)
        {
            if (!IsValidIndex(conditionStates, i) || !IsValidIndex(digitalWriteStates, i))
                continue;
            int cmp = conditionStates[i].compareValue;
            DigitalWrite dw = digitalWriteStates[i];
            code.AppendLine($"    if (sensor == {cmp}) {{");
            code.AppendLine($"        digitalWrite({dw.pin}, {(dw.high ? "HIGH" : "LOW")});");
            code.AppendLine("    }");
        }
        
        code.AppendLine("}");

        fullCodeText.text = code.ToString();
    }

    // Optional: allow other scripts/UI to drive the sensor value
    public void SetSimulatedSensorHigh(bool isHigh)
    {
        simulatedSensorHigh = isHigh;
    }

    /// <summary>Opens the shared pin mode selector for the given line (id: 0 = first pinMode line, 1 = second). Only pinModeText[id] is updated when the user confirms.</summary>
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

    public void editDigitalRead()
    {
        if (digitalReadState == null) return;
        currentEditMode = EditMode.DigitalRead;
        if (digitalReadSelector != null)
            digitalReadSelector.SetActive(true);
        if (pinSliderDR != null)
        {
            pinSliderDR.value = digitalReadState.pin;
            pinSliderDR.minValue = 0;
            pinSliderDR.maxValue = 13;
        }
        if (pinTextDR != null)
            pinTextDR.text = digitalReadState.pin.ToString();
    }

    public void editCondition(int id)
    {
        if (!IsValidIndex(conditionStates, id))
        {
            Debug.LogWarning($"Invalid Condition index: {id}");
            return;
        }
        activeId = id;
        currentEditMode = EditMode.Condition;
        if (conditionSelector != null)
            conditionSelector.SetActive(true);
        int val = conditionStates[activeId].compareValue;
        if (value0Toggle != null) value0Toggle.isOn = (val == 0);
        if (value1Toggle != null) value1Toggle.isOn = (val == 1);
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

    public void CloseDigitalReadSelector()
    {
        if (digitalReadSelector != null)
            digitalReadSelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

    public void CloseConditionSelector()
    {
        if (conditionSelector != null)
            conditionSelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

    public void CloseAllSelectors()
    {
        pinModeSelector.SetActive(false);
        if (digitalReadSelector != null) digitalReadSelector.SetActive(false);
        if (conditionSelector != null) conditionSelector.SetActive(false);
        digitalWriteSelector.SetActive(false);
        currentEditMode = EditMode.None;
    }

}
