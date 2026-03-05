using UnityEngine;

public class ARDragOnTargetPlane : MonoBehaviour
{
    private Camera arCamera;
    private Transform imageTarget;
    private Plane dragPlane;
    private Vector3 offset;
    private float fixedLocalY;
    private bool dragging;

    void Start()
    {
        // Auto-find ARCamera
        arCamera = Camera.main;

        // Auto-find ImageTarget: this object is a child of it
        imageTarget = transform.parent;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleInput(touch.position,
                        touch.phase == TouchPhase.Began,
                        touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary,
                        touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled);
        }
        else
        {
            HandleInput(Input.mousePosition,
                        Input.GetMouseButtonDown(0),
                        Input.GetMouseButton(0),
                        Input.GetMouseButtonUp(0));
        }
    }

    void HandleInput(Vector2 screenPos, bool began, bool moved, bool ended)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (began)
        {
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                dragging = true;
                dragPlane = new Plane(imageTarget.up, transform.position);
                fixedLocalY = imageTarget.InverseTransformPoint(transform.position).y;

                if (dragPlane.Raycast(ray, out float enter))
                    offset = transform.position - ray.GetPoint(enter);
            }
        }

        if (dragging && moved)
        {
            dragPlane = new Plane(imageTarget.up, imageTarget.TransformPoint(
                new Vector3(0, fixedLocalY, 0)));

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 worldTarget = ray.GetPoint(enter) + offset;
                Vector3 localPos = imageTarget.InverseTransformPoint(worldTarget);
                localPos.y = fixedLocalY;
                transform.position = imageTarget.TransformPoint(localPos);
            }
        }

        if (ended)
            dragging = false;
    }
}