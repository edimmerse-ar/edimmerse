using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ComponentManager;

public class ProjectManager : MonoBehaviour
{
	public static ProjectManager Instance;

	public List<ProjectData> projects = new List<ProjectData>();
	
	public List<ProjectPanelUI> projectPanels = new List<ProjectPanelUI>();

	private HashSet<int> completedProjects = new HashSet<int>();

	public ProjectListUI projectListUI;

	public GameObject gameComplete;
	public GameObject loadingObject;

	public SceneHandler sceneHandler;
	void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		LoadProjectsFromFile();
		
		ComponentManager.Instance.onComponentPlaced += OnComponentPlaced;
	}

	void OnEnable()
	{
		if (ComponentManager.Instance != null)
			ComponentManager.Instance.onComponentPlaced += OnComponentPlaced;
	}

	void OnDisable()
	{
		if (ComponentManager.Instance != null)
			ComponentManager.Instance.onComponentPlaced -= OnComponentPlaced;
	}

	public void LoadProjectsFromFile()
	{
		TextAsset file = Resources.Load<TextAsset>("Colab_Projects/projects-list");

		if (file == null)
		{
			return;
		}

		projects.Clear();

		string[] lines = file.text.Split('\n');

		foreach (string line in lines)
		{
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
				continue;

			string[] parts = line.Split('|');

			if (parts.Length < 4)
			{
				continue;
			}

			ProjectData project = new ProjectData();

			project.projectId = int.Parse(parts[0]);

			if (Enum.TryParse(parts[1], out ProjectNames enumName))
			{
				project.projectName = enumName;
			}
			else
			{
				continue;
			}

			project.projectUIName = parts[2];

			string[] components = parts[3].Split(',');

			foreach (string comp in components)
			{
				if (Enum.TryParse(comp.Trim(), out ComponentCategory category))
				{
					project.requiredComponents.Add(category);
				}
				else
				{
					Debug.LogWarning($"Invalid component: {comp}");
				}
			}

			projects.Add(project);
		}

		projectListUI.GenerateProjects();
	}

	void OnComponentPlaced()
	{
		bool allCompleted = true;

		foreach (var project in projects)
		{
			bool completed = IsProjectCompleted(project);

			if (!completed)
			{
				allCompleted = false;
				continue;
			}

			foreach (var item in projectPanels)
			{
				if (item.project.projectName == project.projectName)
				{
					item.SetCompleted();
				}
			}
		}

		if (allCompleted)
		{
			gameComplete.SetActive(true);
		}
	}

	public bool IsProjectCompleted(ProjectData project)
	{
		var requiredCounts = new Dictionary<ComponentCategory, int>();

		foreach (ComponentCategory comp in project.requiredComponents)
		{
			if (!requiredCounts.ContainsKey(comp))
				requiredCounts[comp] = 0;

			requiredCounts[comp]++;
		}

		foreach (var kv in requiredCounts)
		{
			ComponentCategory comp = kv.Key;
			int required = kv.Value;

			int placed = ComponentManager.Instance.GetPlacedCount(comp, project.projectId);

			if (placed < required)
			{
				return false;
			}
		}

		return true;
	}

	public void RegisterPanel(ProjectPanelUI panel)
	{
		projectPanels.Add(panel);
	}


	public void SafeExitPhoton()
	{
		loadingObject.SetActive(true);

		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.Disconnect();
		}
		else if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
		else
		{
			sceneHandler.GoToScene("DIY");
		}
	}
}