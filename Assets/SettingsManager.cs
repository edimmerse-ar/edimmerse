using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Toggles")]
    public Toggle musicToggle;
    public Toggle soundToggle;
    public Toggle vibrationToggle;

    private const string MUSIC_PREF = "MusicEnabled";
    private const string SOUND_PREF = "SoundEnabled";
    private const string VIBRATION_PREF = "VibrationEnabled";

    public SettingsHandler settingsHandler; // Reference to SettingsHandler

    void Start()
    {
        // Initialize prefs only if they don't exist
        if (!PlayerPrefs.HasKey(MUSIC_PREF))
            PlayerPrefs.SetInt(MUSIC_PREF, 1); // Default ON
        if (!PlayerPrefs.HasKey(SOUND_PREF))
            PlayerPrefs.SetInt(SOUND_PREF, 1);
        if (!PlayerPrefs.HasKey(VIBRATION_PREF))
            PlayerPrefs.SetInt(VIBRATION_PREF, 1);

        // Load states from prefs
        musicToggle.isOn = PlayerPrefs.GetInt(MUSIC_PREF) == 1;
        soundToggle.isOn = PlayerPrefs.GetInt(SOUND_PREF) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt(VIBRATION_PREF) == 1;

        // Add listeners
        musicToggle.onValueChanged.AddListener(SetMusic);
        soundToggle.onValueChanged.AddListener(SetSound);
        vibrationToggle.onValueChanged.AddListener(SetVibration);

        // Apply settings on start
        settingsHandler.ApplySettings();
    }

    public void SetMusic(bool isEnabled)
    {
        PlayerPrefs.SetInt(MUSIC_PREF, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        settingsHandler.ApplySettings();
        Debug.Log("Music set to: " + isEnabled);
    }

    public void SetSound(bool isEnabled)
    {
        PlayerPrefs.SetInt(SOUND_PREF, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        settingsHandler.ApplySettings();
        Debug.Log("Sound set to: " + isEnabled);
    }

    public void SetVibration(bool isEnabled)
    {
        PlayerPrefs.SetInt(VIBRATION_PREF, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        settingsHandler.ApplySettings();
        Debug.Log("Vibration set to: " + isEnabled);
    }
}
