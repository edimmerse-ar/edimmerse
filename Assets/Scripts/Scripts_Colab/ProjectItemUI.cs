using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectItemUI : MonoBehaviour
{
    public TMP_Text projectNameText;
    public TMP_Text componentsText;
    public Toggle completedToggle;

    private ProjectData project;

    public void Setup(ProjectData proj)
    {
        project = proj;

        projectNameText.text = proj.projectUIName;

        componentsText.text = string.Join(", ", proj.requiredComponents);

        Refresh();
    }

    void OnEnable()
    {
        if (ComponentManager.Instance != null)
            ComponentManager.Instance.onComponentPlaced += Refresh;
    }

    void OnDisable()
    {
        if (ComponentManager.Instance != null)
            ComponentManager.Instance.onComponentPlaced -= Refresh;
    }

    public void Refresh()
    {
        bool completed = ProjectManager.Instance.IsProjectCompleted(project);
        completedToggle.isOn = completed;

        var parts = new System.Collections.Generic.List<string>();

        var requiredCounts = new System.Collections.Generic.Dictionary<ComponentCategory, int>();
        foreach (var id in project.requiredComponents)
        {
            if (!requiredCounts.ContainsKey(id)) requiredCounts[id] = 0;
            requiredCounts[id]++;
        }

        foreach (var kv in requiredCounts)
        {
            ComponentCategory id = kv.Key;
            int required = kv.Value;
            int placed = ComponentManager.Instance.GetPlacedCount(id, project.projectId);
            int max = ComponentManager.Instance.GetMaxCount(id);

            parts.Add($"{id} ({placed}/{required})");
        }

        componentsText.text = string.Join(", ", parts);
    }
}