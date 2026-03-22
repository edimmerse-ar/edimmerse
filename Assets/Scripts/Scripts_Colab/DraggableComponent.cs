using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableComponent : MonoBehaviour,
	IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public ComponentCategory category;

	private RectTransform rectTransform;
	private Canvas canvas;
	private CanvasGroup canvasGroup;
	private Vector2 startPosition;

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();

		canvasGroup = GetComponent<CanvasGroup>();

		if (canvasGroup == null)
			canvasGroup = gameObject.AddComponent<CanvasGroup>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		startPosition = rectTransform.anchoredPosition;
		canvasGroup.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		canvasGroup.blocksRaycasts = true;
		rectTransform.anchoredPosition = startPosition;
	}
}