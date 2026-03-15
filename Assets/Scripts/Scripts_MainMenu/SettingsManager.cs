using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Toggles")]
    public Toggle musicToggle;
    public Toggle vibrationToggle;
    public Slider volumeSlider;

	private const string MUSIC_PREF = "MusicEnabled";
    private static string VOLUME_PREF = "MusicVolume";
	private const string VIBRATION_PREF = "VibrationEnabled";

	public MusicController musicController;

    void Start()
    {
        if (!PlayerPrefs.HasKey(MUSIC_PREF))
            PlayerPrefs.SetInt(MUSIC_PREF, 1);
        if(!PlayerPrefs.HasKey(VOLUME_PREF))
            PlayerPrefs.SetFloat(VOLUME_PREF, 1f);
		if (!PlayerPrefs.HasKey(VIBRATION_PREF))
            PlayerPrefs.SetInt(VIBRATION_PREF, 1);

        musicToggle.isOn = PlayerPrefs.GetInt(MUSIC_PREF) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt(VIBRATION_PREF) == 1;
        volumeSlider.value = PlayerPrefs.GetFloat(VOLUME_PREF, 1f);

        musicToggle.onValueChanged.AddListener(SetMusic);
        vibrationToggle.onValueChanged.AddListener(SetVibration);
        volumeSlider.onValueChanged.AddListener(setVolume);

        // Ensure we reference the persistent singleton if available. When switching
        // scenes a scene-local MusicController may be destroyed by the singleton's
        // Awake(), leaving the serialized reference null. Prefer the singleton
        // instance when present.
        if (musicController == null && MusicController.Instance != null)
            musicController = MusicController.Instance;

        // If the persistent MusicController isn't available yet (order of Awake across scenes),
        // wait for it and then apply settings. This prevents race conditions when switching scenes.
        if (musicController == null && MusicController.Instance == null)
        {
            StartCoroutine(WaitForMusicControllerThenApply());
        }
        else
        {
            // apply immediately if available
            this.ApplySettings();
        }
    }

    public void SetMusic(bool isEnabled)
    {
        PlayerPrefs.SetInt(MUSIC_PREF, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

        bool musicEnabled = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;

        var mc = musicController ?? MusicController.Instance;
        if (mc == null)
        {
            Debug.Log("SettingsManager: MusicController not yet available; will apply music setting when ready.");
            StartCoroutine(WaitForMusicControllerThenApply());
            return;
        }

        if (musicEnabled) mc.Play();
        else mc.Stop();
	}

    public void SetVibration(bool isEnabled)
    {
        PlayerPrefs.SetInt(VIBRATION_PREF, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

		bool vibrationEnabled = PlayerPrefs.GetInt(VIBRATION_PREF, 1) == 1;
		if (vibrationEnabled) Vibrate();
	}

    public void setVolume(float volume)
    {
        PlayerPrefs.SetFloat(VOLUME_PREF, volume);
        PlayerPrefs.Save();

        var mc = musicController ?? MusicController.Instance;
        if (mc != null)
        {
            mc.SetVolume(volume);
        }
        else
        {
            Debug.Log("SettingsManager: MusicController not yet available; will apply volume when ready.");
            StartCoroutine(WaitForMusicControllerThenApply());
        }
	}

	public void ApplySettings()
	{
		bool musicEnabled = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
		bool vibrationEnabled = PlayerPrefs.GetInt(VIBRATION_PREF, 1) == 1;
        float volume = PlayerPrefs.GetFloat(VOLUME_PREF, 1f);

        var mc = musicController ?? MusicController.Instance;
        if (mc != null)
        {
            if (musicEnabled) mc.Play();
            else mc.Stop();

            mc.SetVolume(volume);
        }
        else
        {
            Debug.Log("SettingsManager: MusicController instance not found when applying settings; will wait for it.");
            StartCoroutine(WaitForMusicControllerThenApply());
        }

        if (vibrationEnabled) Vibrate();
	}

    private System.Collections.IEnumerator WaitForMusicControllerThenApply()
    {
        // Wait up to 2 seconds for the singleton to become available
        float timeout = 2f;
        float elapsed = 0f;
        while (MusicController.Instance == null && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (MusicController.Instance != null)
        {
            musicController = MusicController.Instance;
            ApplySettings();
        }
        else
        {
            Debug.LogWarning("SettingsManager: MusicController not available after waiting; settings not applied to audio.");
        }
    }

	public void Vibrate()
	{
        #if UNITY_ANDROID || UNITY_IOS
		        Handheld.Vibrate();
        #endif
	}

	public void ResetData()
	{
		// Reset stored preferences to defaults
		PlayerPrefs.DeleteAll();

		PlayerPrefs.SetInt(MUSIC_PREF, 1);
		PlayerPrefs.SetFloat(VOLUME_PREF, 1f);
		PlayerPrefs.SetInt(VIBRATION_PREF, 1);
		PlayerPrefs.Save();

		// Reset UI elements
		if (musicToggle != null) musicToggle.isOn = true;
		if (vibrationToggle != null) vibrationToggle.isOn = true;
		if (volumeSlider != null) volumeSlider.value = 1f;

		// Apply settings to the audio controller (ApplySettings will wait for the singleton if needed)
		ApplySettings();
	}
}
