using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
	public static ComponentManager Instance;
	public List<ComponentData> components = new List<ComponentData>();


	private Dictionary<int, Dictionary<ComponentCategory, int>> projectPlacedCount
		= new Dictionary<int, Dictionary<ComponentCategory, int>>();

	public delegate void OnComponentPlaced();
	public event OnComponentPlaced onComponentPlaced;

	public UpdateScore updateScore;
	void Awake()
	{
		Instance = this;
	}

	public bool CanPlace(ComponentCategory component, int projectId)
	{
		int count = GetPlacedCount(component, projectId);
		int max = GetMaxCount(component);

		return count < max;
	}

	public bool CanPlace(ComponentCategory component, ProjectNames projectName)
	{
		if (ProjectManager.Instance == null)
			return false;

		var proj = ProjectManager.Instance.projects.Find(p => p.projectName == projectName);
		if (proj == null)
			return false;

		return CanPlace(component, proj.projectId);
	}

	public void MarkPlaced(ComponentCategory component, int projectId)
	{
		if (!projectPlacedCount.ContainsKey(projectId))
		{
			projectPlacedCount[projectId] = new Dictionary<ComponentCategory, int>();
		}

		var compDict = projectPlacedCount[projectId];

		if (!compDict.ContainsKey(component))
		{
			compDict[component] = 0;
		}

		compDict[component]++;

		if (!IsAvailableAnywhere(component))
		{
			RemoveButton(component);
		}

		onComponentPlaced?.Invoke();

		if (Photon.Pun.PhotonNetwork.LocalPlayer.NickName == GlobalVariables.PlayerName)
		{
			updateScore.Submit(1);
		}
	}

	bool IsAvailableAnywhere(ComponentCategory component)
	{
		int totalPlaced = 0;

		foreach (var project in projectPlacedCount.Values)
		{
			if (project.ContainsKey(component))
				totalPlaced += project[component];
		}

		return totalPlaced < GetMaxCount(component);
	}

	void RemoveButton(ComponentCategory component)
	{
		DraggableComponent[] buttons = FindObjectsOfType<DraggableComponent>();

		foreach (var btn in buttons)
		{
			if (btn.category == component)
			{
				Destroy(btn.gameObject);
			}
		}
	}

	public int GetMaxCount(ComponentCategory component)
	{
		foreach (var comp in components)
		{
			if (comp.category == component)
				return comp.maxCount;
		}
		return 0;
	}

	public GameObject GetPlacedPrefab(ComponentCategory component)
	{
		foreach (var comp in components)
		{
			if (comp.category == component)
				return comp.placedPrefab;
		}
		return null;
	}

	public int GetPlacedCount(ComponentCategory component, int projectId)
	{
		if (!projectPlacedCount.ContainsKey(projectId))
			return 0;

		var compDict = projectPlacedCount[projectId];

		if (!compDict.ContainsKey(component))
			return 0;

		return compDict[component];
	}

	public int GetPlacedCount(ComponentCategory component, ProjectNames projectName)
	{
		if (ProjectManager.Instance == null)
			return 0;

		var proj = ProjectManager.Instance.projects.Find(p => p.projectName == projectName);
		if (proj == null)
			return 0;

		return GetPlacedCount(component, proj.projectId);
	}


}