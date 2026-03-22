using UnityEngine;

public class ProjectListUI : MonoBehaviour
{
	public GameObject projectItemPrefab;
	public Transform contentParent;
	public static ProjectListUI Instance;

	void Start()
	{
		Instance = this;
		GenerateProjects();

		ComponentManager.Instance.onComponentPlaced += Refresh;
	}

	public void GenerateProjects()
	{
		foreach (var proj in ProjectManager.Instance.projects)
		{
			GameObject item = Instantiate(projectItemPrefab, contentParent);
			item.name = proj.projectName.ToString();
			ProjectItemUI ui = item.GetComponent<ProjectItemUI>();
			ui.Setup(proj);
		}
	}

	void Refresh()
	{
		foreach (Transform child in contentParent)
		{
			child.GetComponent<ProjectItemUI>().Refresh();
		}
	}

	public static void RefreshAll()
	{
		if (Instance == null)
			return;

		foreach (Transform child in Instance.contentParent)
		{
			child.GetComponent<ProjectItemUI>().Refresh();
		}
	}
}