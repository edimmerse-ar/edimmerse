using UnityEngine;

/// <summary>
/// Simple persistent singleton music controller.
/// Attach one in your initial scene and configure the music clip in the inspector.
/// It will survive scene loads and ensure only a single instance exists.
/// </summary>
public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    [Header("Audio")]
    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 1f;
    public bool playOnStart = true;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.clip = musicClip;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.volume = volume;

        if (playOnStart && musicClip != null)
            Play();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Play()
    {
        if (_audioSource == null) return;
        if (_audioSource.clip == null) _audioSource.clip = musicClip;
        if (!_audioSource.isPlaying) _audioSource.Play();
    }

    public void Stop()
    {
        if (_audioSource == null) return;
        if (_audioSource.isPlaying) _audioSource.Stop();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (_audioSource != null) _audioSource.volume = volume;
    }

    public void ToggleMute()
    {
        if (_audioSource == null) return;
        _audioSource.mute = !_audioSource.mute;
    }
}
