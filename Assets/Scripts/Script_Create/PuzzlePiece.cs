using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    public RectTransform targetSlot;
    public float snapDistance = 50f;
    private bool placed = false;

    public PuzzleManager puzzleManager;
    private Canvas canvas; // Reference to the main canvas
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (placed) return;
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (placed) return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (placed) return;

        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetSlot.anchoredPosition);
        if (distance < snapDistance)
        {
            rectTransform.anchoredPosition = targetSlot.anchoredPosition;
            placed = true;
            puzzleManager.PiecePlacedCorrectly();
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }
}
