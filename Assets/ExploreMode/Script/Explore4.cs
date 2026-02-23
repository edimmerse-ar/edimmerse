using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Explore4 : MonoBehaviour
{
    #region Enums
    
    private enum ComponentType
    {
        IRSensor,
        Breadboard,
        Wire1,
        Wire2,
        Wire3,
        Wire4,
        Wire5,
        Led,
        Resistor
    }

    private enum ToastMessageType
    {
        ScanMarker = 0,
        PlaceBreadboard = 1,
        PlaceComponents = 2,
        NeedBreadboardFirst = 3,
        PlaceSelectedFirst = 4,
        Success = 5
    }

    #endregion

    #region Serializable Classes

    [System.Serializable]
    private class ComponentData
    {
        public ComponentType type;
        public GameObject component;
        public GameObject blink;
        public GameObject drag;
        public GameObject button;
        public string hintMessage;
        
        [HideInInspector]
        public bool isPlaced;
    }

    #endregion

    #region Fields

    [Header("Components")]
    [SerializeField] private List<ComponentData> components = new List<ComponentData>();
    
    [Header("Toast Settings")]
    [SerializeField] private float toastDisplayDuration = 5f;
    public TextMeshProUGUI toastMsg;
    public GameObject toast;
    
    [Header("Blink Settings")]
    public Material blinkingMaterial;
    public float blinkSpeed = 2f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public bool isBlinking = true;

    [Header("UI")]
    public GameObject componentPanel;
    public GameObject hintButton;

    // Private state
    private string[] toastMessages;
    private ToastMessageType activeToastMessage = ToastMessageType.ScanMarker;
    private ComponentType currentComponentType = ComponentType.IRSensor;
    private bool isComponentPlaced = true;
    private Coroutine _showToastCoroutine;
    private int remainingComponents;

    #endregion

    #region Unity Lifecycle

    void Start()
    {
        InitializeToastMessages();
        remainingComponents = components.Count;
        
        if (_showToastCoroutine != null)
            StopCoroutine(_showToastCoroutine);
        _showToastCoroutine = StartCoroutine(ShowToast());
    }

    void Update()
    {
        if (!isBlinking || blinkingMaterial == null) return;
        
        // Oscillate alpha using sine wave for smooth blinking
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);
        blinkingMaterial.SetFloat("_Alpha", alpha);
    }

    #endregion

    #region Initialization

    private void InitializeToastMessages()
    {
        toastMessages = new string[6];
        toastMessages[(int)ToastMessageType.ScanMarker] = "Scan the Marker to get started with Activity";
        // Use PlaceBreadboard slot to indicate placing the Breadboard in this scene
        toastMessages[(int)ToastMessageType.PlaceBreadboard] = "Place the Breadboard near the Arduino";
        toastMessages[(int)ToastMessageType.PlaceComponents] = "Place all the components (IR Sensor, breadboard, wires, LED and resistor) and connect them as shown";
        toastMessages[(int)ToastMessageType.NeedBreadboardFirst] = "You need to place the Breadboard first";
        toastMessages[(int)ToastMessageType.PlaceSelectedFirst] = "First you need to place selected component";
        toastMessages[(int)ToastMessageType.Success] = "Good Job! You have connected the components successfully. Next start with coding.";
    }

    #endregion

    #region Component Lookup

    private ComponentData GetComponentData(ComponentType type)
    {
        return components.Find(c => c.type == type);
    }

    private ComponentData GetComponentDataByTriggerId(string triggerId)
    {
        if (!Enum.TryParse<ComponentType>(triggerId, true, out ComponentType type))
        {
            Debug.LogWarning("Invalid triggered id: " + triggerId);
            return null;
        }
        return GetComponentData(type);
    }

    #endregion

    #region Marker Events

    public void OnMarkerFound()
    {
        if (activeToastMessage == ToastMessageType.ScanMarker)
        {
            activeToastMessage = ToastMessageType.PlaceBreadboard;
        }
        
        toastMsg.text = toastMessages[(int)activeToastMessage];
        hintButton.SetActive(true);

        RestartToastCoroutine();
    }

    public void OnMarkerLost()
    {
        hintButton.SetActive(false);
        toastMsg.text = toastMessages[(int)ToastMessageType.ScanMarker];
    }

    #endregion

    #region Component Button Click

    public void OnComponentButtonClick(string componentType)
    {
        if (!Enum.TryParse<ComponentType>(componentType, true, out ComponentType type))
        {
            Debug.LogWarning("Invalid component type: " + componentType);
            return;
        }

        if (!isComponentPlaced)
        {
            ShowPlacementWarning();
            return;
        }

        ComponentData data = GetComponentData(type);
        if (data == null)
        {
            Debug.LogWarning("Component data not found for type: " + type);
            return;
        }

        // Only update state after we've confirmed data exists
        currentComponentType = type;
        isComponentPlaced = false;

        // Activate blink and drag, hide button
        if (data.blink != null) data.blink.SetActive(true);
        if (data.drag != null) data.drag.SetActive(true);
        if (data.button != null) data.button.SetActive(false);

        remainingComponents--;

        if (remainingComponents <= 0)
        {
            componentPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if all hardware components have been placed.
    /// Used by CodeEditor to verify hardware setup before running code.
    /// </summary>
    public bool IsHardwareSetupComplete()
    {
        foreach (var component in components)
        {
            if (!component.isPlaced) return false;
        }
        return true;
    }

    private bool AreAllComponentsPlaced()
    {
        return IsHardwareSetupComplete();
    }

    private void ShowPlacementWarning()
    {
        if (activeToastMessage == ToastMessageType.ScanMarker)
        {
            toastMsg.text = toastMessages[(int)ToastMessageType.NeedBreadboardFirst];
        }
        else
        {
            toastMsg.text = toastMessages[(int)ToastMessageType.PlaceSelectedFirst];
        }

        RestartToastCoroutine();
    }

    #endregion

    #region Trigger Handling

    public void OnTriggerEnter()
    {
        string triggeredId = CollisionDetection.lastTriggeredId;

        ComponentData data = GetComponentDataByTriggerId(triggeredId);
        if (data == null) return;

        isComponentPlaced = true;
        data.isPlaced = true;

        // Activate component and clean up blink/drag objects
        if (data.component != null) data.component.SetActive(true);
        
        if (data.blink != null)
        {
            Destroy(data.blink);
            data.blink = null;
        }
        
        if (data.drag != null)
        {
            Destroy(data.drag);
            data.drag = null;
        }

        // Update toast message
        toastMsg.text = toastMessages[(int)ToastMessageType.PlaceComponents];

        if (activeToastMessage == ToastMessageType.PlaceComponents)
        {
            if (remainingComponents > 0) return;

            toastMsg.text = toastMessages[(int)ToastMessageType.Success];
            RestartToastCoroutine();
            return;
        }

        RestartToastCoroutine();
        activeToastMessage = ToastMessageType.PlaceComponents;
    }

    #endregion

    #region Hints

    public void ToggleHints()
    {
        RestartToastCoroutine();

        // If all components are placed, show coding message
        if (AreAllComponentsPlaced())
        {
            toastMsg.text = "Next you can start with coding.";
            return;
        }

        if (isComponentPlaced)
        {
            ShowSelectComponentHint();
            return;
        }

        ShowPlacementHint();
        ActivateCurrentComponentBlink();
    }

    private void ShowSelectComponentHint()
    {
        if (activeToastMessage == ToastMessageType.PlaceBreadboard)
        {
            toastMsg.text = "Select the Breadboard from the component panel";
        }
        else
        {
            toastMsg.text = "Select another component from the component panel";
        }
    }

    private void ShowPlacementHint()
    {
        ComponentData data = GetComponentData(currentComponentType);
        if (data != null && !string.IsNullOrEmpty(data.hintMessage))
        {
            toastMsg.text = data.hintMessage;
        }
        else
        {
            // Fallback messages based on component type
            switch (currentComponentType)
            {
                case ComponentType.IRSensor:
                    toastMsg.text = "Place the IR Sensor near the Arduino";
                    break;
                case ComponentType.Breadboard:
                    toastMsg.text = "Place the breadboard next to the Arduino and align the rails";
                    break;
                case ComponentType.Wire1:
                    toastMsg.text = "Place Wire1 to connect the IR sensor VCC to the breadboard";
                    break;
                case ComponentType.Wire2:
                    toastMsg.text = "Place Wire2 to connect the IR sensor GND to the breadboard";
                    break;
                case ComponentType.Wire3:
                    toastMsg.text = "Place Wire3 to connect sensor signal to Arduino pin";
                    break;
                case ComponentType.Wire4:
                    toastMsg.text = "Place Wire4 to connect the LED anode to the breadboard";
                    break;
                case ComponentType.Wire5:
                    toastMsg.text = "Place Wire5 to connect the LED cathode (via resistor) to Arduino GND";
                    break;
                case ComponentType.Led:
                    toastMsg.text = "Place the LED on the breadboard and orient the legs correctly (long leg = anode)";
                    break;
                case ComponentType.Resistor:
                    toastMsg.text = "Place the resistor between the LED cathode and ground rail to limit current";
                    break;
            }
        }
    }

    private void ActivateCurrentComponentBlink()
    {
        ComponentData data = GetComponentData(currentComponentType);
        if (data == null || data.blink == null) return;

        data.blink.SetActive(true);

        MeshRenderer meshRenderer = data.blink.GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = true;

        MeshRenderer[] childMeshRenderers = data.blink.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer renderer in childMeshRenderers)
        {
            renderer.enabled = true;
        }
    }

    #endregion

    #region Toast Management

    private void RestartToastCoroutine()
    {
        if (_showToastCoroutine != null)
            StopCoroutine(_showToastCoroutine);
        _showToastCoroutine = StartCoroutine(ShowToast());
    }

    public bool isToastActive()
    {
        return _showToastCoroutine != null;
    }

    private IEnumerator ShowToast()
    {
        toast.SetActive(true);
        yield return new WaitForSeconds(toastDisplayDuration);
        toast.SetActive(false);
        _showToastCoroutine = null;
    }

    /// <summary>
    /// Public API to show a custom toast message from other scripts
    /// </summary>
    /// <param name="message">The message to display</param>
    public void ShowToastMessage(string message)
    {
        toastMsg.text = message;
        RestartToastCoroutine();
    }

    #endregion

    #region Legacy Method Names (For Backwards Compatibility)
    
    // These methods maintain backwards compatibility with existing Unity event references
    // Consider updating your Unity event references to use the new PascalCase names
    
    public void onMarkerFound() => OnMarkerFound();
    public void onMarkerLost() => OnMarkerLost();
    public void onComponentButtonClick(string componentType) => OnComponentButtonClick(componentType);
    public void onTriggerEnter() => OnTriggerEnter();

    #endregion
}
