using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("QA Source")]
    [Tooltip("Optional TextAsset containing questions for all projects. If empty, questionsResourceName will be used to load from Resources.")]
    public TextAsset questionsAsset;
    [Tooltip("Resource path (relative to Resources/) to load questions from if questionsAsset is not set. Example: 'DIY_Questions/questions'")]
    public string questionsResourceName = "DIY_Questions/questions";

    [Header("QA UI")]
    [Tooltip("Container transform under which question buttons will be instantiated.")]
    public Transform questionsContainer;
    [Tooltip("Button prefab to instantiate per question. Should contain a child Text component for the label.")]
    public Button questionButtonPrefab;

    [Header("Navigation")]
    [Tooltip("Optional scene name to load after a question is selected.")]
    public SceneHandler sceneHandler;
    public string nextSceneName;

    public static int projectId = -1;
	public static string selectedQuestion = null;

    [Header("Panels")]
	public GameObject diyMenuPanel;
	public GameObject projectPanel;
    public GameObject qaPanel;

    // parsed question blocks (each entry is an array of questions for a project)
    private readonly List<string[]> projectQuestions = new List<string[]>();

	private void Start()
	{
        SceneHandler.lastSceneName = "DIY";

		int index = DIYProjectHandler.projectId;
        if (index == -1)
        {
			selectedQuestion = null;
			return;
        }

        if (DIYProjectHandler.selectedQuestion !=null)
            showQA();
        else
            LoadProject(DIYProjectHandler.projectId);

		DIYProjectHandler.selectedQuestion = null;
	}

	public void resetProjectId()
    {
		DIYProjectHandler.projectId = -1;
	}

    public void resetQuestion()
    {
        DIYProjectHandler.selectedQuestion = null;
	}

	// Previously hardcoded project arrays removed. Questions are now loaded from a single text file.

	/// <summary>
	/// Load project content by index. Title is taken from the `titles` array if present;
	/// description is taken from `descriptionAssets[index]` if present, otherwise it will
	/// attempt to load a TextAsset from Resources using `descriptionResourceNames[index]`.
	/// </summary>
	public void LoadProject(int index)
    {
		qaPanel.SetActive(false);
		diyMenuPanel.SetActive(false);
		projectPanel.SetActive(true);

		if (titleText == null && descriptionText == null)
        {
            Debug.LogWarning("DIYProjectHandler: No TextMeshProUGUI targets assigned.");
            return;
        }
        DIYProjectHandler.projectId = index;
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

    private void EnsureQuestionsParsed()
    {
        if (projectQuestions.Count > 0) return;

        string text = null;
        if (questionsAsset != null) text = questionsAsset.text;
        else if (!string.IsNullOrEmpty(questionsResourceName))
        {
            var ta = Resources.Load<TextAsset>(questionsResourceName);
            if (ta != null) text = ta.text;
            else Debug.LogWarning($"DIYProjectHandler: questions resource '{questionsResourceName}' not found in Resources.");
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            Debug.LogWarning("DIYProjectHandler: questions source is empty or not provided.");
            return;
        }

        text = text.Replace("\r\n", "\n").Replace("\r", "\n");
        var lines = text.Split('\n').Select(l => l.Trim()).ToArray();

        // support two formats: Project: N headers or blocks separated by blank lines
        bool hasProjectHeader = lines.Any(l => l.StartsWith("Project:", StringComparison.OrdinalIgnoreCase));

        if (hasProjectHeader)
        {
            var blocks = new SortedDictionary<int, List<string>>();
            int current = -1;
            foreach (var raw in lines)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                if (raw.StartsWith("Project:", StringComparison.OrdinalIgnoreCase))
                {
                    var after = raw.Substring(raw.IndexOf(':') + 1).Trim();
                    if (int.TryParse(after, out int parsed)) current = parsed;
                    else current = blocks.Any() ? blocks.Keys.Max() + 1 : 0;
                    if (!blocks.ContainsKey(current)) blocks[current] = new List<string>();
                }
                else
                {
                    if (current < 0) current = 0;
                    if (!blocks.ContainsKey(current)) blocks[current] = new List<string>();
                    blocks[current].Add(raw);
                }
            }

            foreach (var kv in blocks) projectQuestions.Add(kv.Value.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
        }
        else
        {
            var rawBlocks = text.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var block in rawBlocks)
            {
                var qs = block.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                if (qs.Length > 0) projectQuestions.Add(qs);
            }
        }
    }

    public void showQA()
    {
        qaPanel.SetActive(true);
        diyMenuPanel.SetActive(false);
        projectPanel.SetActive(false);

		int index = DIYProjectHandler.projectId;
        if (index < 0) return;

		EnsureQuestionsParsed();
        if (questionsContainer == null)
        {
            Debug.LogWarning("DIYProjectHandler: questionsContainer not assigned.");
            return;
        }
        if (questionButtonPrefab == null)
        {
            Debug.LogWarning("DIYProjectHandler: questionButtonPrefab not assigned.");
            return;
        }
        if (index < 0 || index >= projectQuestions.Count)
        {
            Debug.LogWarning($"DIYProjectHandler: project index {index} out of range (parsed {projectQuestions.Count}).");
            return;
        }

        // clear existing
        for (int i = questionsContainer.childCount - 1; i >= 0; i--)
        {
            var c = questionsContainer.GetChild(i);
#if UNITY_EDITOR
            DestroyImmediate(c.gameObject);
#else
            Destroy(c.gameObject);
#endif
        }

        var questions = projectQuestions[index];
        foreach (var q in questions)
        {
            var btn = Instantiate(questionButtonPrefab, questionsContainer);

            // Try to find a TextMeshPro UI label first (TMP_Text covers TextMeshProUGUI).
            var tmpLabel = btn.GetComponentInChildren<TMPro.TMP_Text>();
            if (tmpLabel != null)
            {
                tmpLabel.text = q;
            }

            btn.onClick.RemoveAllListeners();
            string captured = q;
            btn.onClick.AddListener(() => {
                DIYProjectHandler.selectedQuestion = captured;
                Debug.Log($"DIYProjectHandler: selected question set to: {captured}");
                sceneHandler.GoToScene(nextSceneName);
                SceneHandler.lastSceneName = "DIY";
			});
        }
    }
}
