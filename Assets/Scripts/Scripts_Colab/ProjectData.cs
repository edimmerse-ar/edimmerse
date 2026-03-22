using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProjectData
{
    public ProjectNames projectName;
    public string projectUIName;
	public int projectId = -1;
	public List<ComponentCategory> requiredComponents = new List<ComponentCategory>();
}