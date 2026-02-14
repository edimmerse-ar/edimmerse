using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Progress Settings")]
    public int maximum = 3;
    public int current = 0;

    [Header("UI Reference")]
    public Image bar;

    void Start()
    {
                UpdateFill();
    }

    private void UpdateFill()
    {
        if (bar == null) return; // avoid null reference errors
        int unlock = PlayerPrefs.GetInt("unlock", 1); // Default to 1 if not set
current = unlock-1;
        bar.fillAmount = Mathf.Clamp01((float)current / maximum);
    }

    // Optional helper function to update value externally
    public void SetProgress(int value)
    {
        current = Mathf.Clamp(value, 0, maximum);
    }
}
