using UnityEngine;
using UnityEngine.UI;

public class ProjectPanelUI : MonoBehaviour
{
	public ProjectData project;

	public Image background;

	public Sprite normalImage;
	public Sprite completedImage;

	void Start()
	{
		ProjectManager.Instance.RegisterPanel(this);
		background.sprite = normalImage;
	}

	public void SetCompleted()
	{
		background.sprite = completedImage;

		Debug.Log($"Project {project.projectName} completed 🎉");
	}
}