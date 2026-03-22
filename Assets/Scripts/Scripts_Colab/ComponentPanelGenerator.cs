using UnityEngine;
using TMPro;

public class ComponentPanelGenerator : MonoBehaviour
{
	void Start()
	{
		foreach (ComponentData comp in ComponentManager.Instance.components)
		{
			GameObject btn = Instantiate(comp.buttonPrefab, transform);

			TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

			if (txt != null)
				txt.text = comp.category.ToString();

			DraggableComponent drag = btn.GetComponent<DraggableComponent>();
			drag.category = comp.category;
		}
	}
}