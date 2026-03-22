using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviourPun, IDropHandler
{
	public ComponentCategory acceptedCategory;

	public ProjectData project;
	private bool placed = false;

	public void OnDrop(PointerEventData eventData)
	{
		GameObject dragged = eventData.pointerDrag;

		DraggableComponent comp = dragged.GetComponent<DraggableComponent>();

		if (comp == null) return;

		ComponentCategory componentID = comp.category;

		if (componentID != acceptedCategory)
			return;

		if (project.projectId < 0)
		{
			Debug.LogError("DropArea: projectId not assigned!");
			return;
		}

		if (!ComponentManager.Instance.CanPlace(componentID, project.projectId))
		{
			Destroy(dragged);
			return;
		}

		Vector2 pos;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			transform as RectTransform,
			eventData.position,
			eventData.pressEventCamera,
			out pos
		);

		photonView.RPC(
			"PlaceComponent",
			RpcTarget.AllBuffered,
			componentID,
			project.projectId,
			pos.x,
			pos.y,
			PhotonNetwork.LocalPlayer.ActorNumber
		);
	}

	[PunRPC]
	void PlaceComponent(ComponentCategory componentID, int projectId, float x, float y, int actorNumber)
	{
		if(placed)
			return;

		if (!ComponentManager.Instance.CanPlace(componentID, projectId))
			return;

		ComponentManager.Instance.MarkPlaced(componentID, projectId);

		GameObject prefab = ComponentManager.Instance.GetPlacedPrefab(componentID);

		gameObject.transform.Find(prefab.name)?.gameObject.SetActive(true);
		//if (prefab == null)
		//	return;

		//GameObject obj = Instantiate(prefab, transform);

		//RectTransform rect = obj.GetComponent<RectTransform>();
		//rect.anchoredPosition = new Vector2(x, y);

		if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
		{
			PlayerStats.IncrementDrop();
		}

		ProjectListUI.RefreshAll();

		placed = true;

	}
}