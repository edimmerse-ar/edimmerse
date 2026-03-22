using UnityEngine;

[System.Serializable]
public class ComponentData
{
	public GameObject buttonPrefab;
	public GameObject placedPrefab;

	public ComponentCategory category;

	public int maxCount = 1;
}