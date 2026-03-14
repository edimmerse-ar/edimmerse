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

		this.ApplySettings();
    }

    public void SetMusic(bool isEnabled)
    {
        PlayerPrefs.SetInt(MUSIC_PREF, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

		bool musicEnabled = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;

		if (musicEnabled) musicController.Play();
		else musicController.Stop();
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

		if (musicController != null)
            musicController.SetVolume(volume);
	}

	public void ApplySettings()
	{
		bool musicEnabled = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
		bool vibrationEnabled = PlayerPrefs.GetInt(VIBRATION_PREF, 1) == 1;

        if (musicEnabled) musicController.Play();
        else musicController.Stop();

		if (vibrationEnabled) Vibrate();
	}

	public void Vibrate()
	{
        #if UNITY_ANDROID || UNITY_IOS
		        Handheld.Vibrate();
        #endif
	}

	public void ResetData()
	{
		PlayerPrefs.DeleteAll();
	}
}
