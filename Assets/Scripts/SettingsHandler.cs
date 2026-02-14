using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public AudioSource musicSource;

    private const string MUSIC_PREF = "MusicEnabled";
    private const string SOUND_PREF = "SoundEnabled";
    private const string VIBRATION_PREF = "VibrationEnabled";

    public void ApplySettings()
    {
        bool musicEnabled = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        bool soundEnabled = PlayerPrefs.GetInt(SOUND_PREF, 1) == 1;
        bool vibrationEnabled = PlayerPrefs.GetInt(VIBRATION_PREF, 1) == 1;

Debug.Log(musicEnabled);
        // Handle music
        if (musicEnabled)
        {
                musicSource.Play();
        }
        else
        {
                musicSource.Stop();
        }

        // Handle sound
        AudioListener.volume = soundEnabled ? 1f : 0f;

        // Handle vibration (vibrate once if enabled just as feedback)
        if (vibrationEnabled)
        {
            Vibrate();
        }
    }

    public void Vibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }
}
