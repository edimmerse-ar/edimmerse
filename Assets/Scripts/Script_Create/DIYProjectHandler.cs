using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Loads two text files (title and description) that may contain simple HTML tags
/// and displays them in TextMeshProUGUI fields. Supports loading from a TextAsset
/// assigned in the inspector or from Resources by filename.
/// </summary>
public class DIYProjectHandler : MonoBehaviour
{
    [Header("Display")]
    public Text titleText;
    public Text descriptionText;

    [Header("Titles")]
    [Tooltip("Optional list of titles. If provided, the title for a project will be taken from this array by index.")]
    public string[] titles;

    [Header("Descriptions")]
    [Tooltip("If a TextAsset is provided at an index it will be used. Otherwise the corresponding resource name will be used.")]
    public TextAsset[] descriptionAssets;
    public string[] descriptionResourceNames;

    [Header("Options")]
    public bool loadOnStart = true;

    private void Start()
    {
        if (loadOnStart) Load();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        LoadProject(0);
    }

    /// <summary>
    /// Load project content by index. Title is taken from the `titles` array if present;
    /// description is taken from `descriptionAssets[index]` if present, otherwise it will
    /// attempt to load a TextAsset from Resources using `descriptionResourceNames[index]`.
    /// </summary>
    public void LoadProject(int index)
    {
        if (titleText == null && descriptionText == null)
        {
            Debug.LogWarning("DIYProjectHandler: No TextMeshProUGUI targets assigned.");
            return;
        }

        string rawTitle = null;
        string rawDesc = null;

        if (titles != null && index >= 0 && index < titles.Length)
            rawTitle = titles[index];

        if (descriptionAssets != null && index >= 0 && index < descriptionAssets.Length && descriptionAssets[index] != null)
            rawDesc = descriptionAssets[index].text;
        else if (descriptionResourceNames != null && index >= 0 && index < descriptionResourceNames.Length && !string.IsNullOrEmpty(descriptionResourceNames[index]))
        {
            var ta = Resources.Load<TextAsset>(descriptionResourceNames[index]);
            if (ta != null) rawDesc = ta.text;
            else Debug.LogWarning($"DIYProjectHandler: description resource '{descriptionResourceNames[index]}' not found in Resources.");
        }

        if (rawTitle != null && titleText != null)
            titleText.text = rawTitle;

        if (rawDesc != null && descriptionText != null)
            descriptionText.text = rawDesc;
    }
}
