using UnityEngine;

public class DragOnPlaneLimitAxisConstraint : MonoBehaviour
{
    private Camera arCamera;
    private Transform imageTarget;
    private Plane dragPlane;
    private Vector3 offset;
    private Vector3 initialLocalPos;
    private bool dragging;
    public enum Axis { X, Y, Z }
    public Axis allowedAxis = Axis.X;

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
                // store initial local position so we can lock two axes later
                initialLocalPos = imageTarget.InverseTransformPoint(transform.position);

                // choose a drag plane that allows movement along the requested axis
                if (allowedAxis == Axis.Y)
                    dragPlane = new Plane(imageTarget.forward, transform.position);
                else
                    dragPlane = new Plane(imageTarget.up, transform.position);

                if (dragPlane.Raycast(ray, out float enter))
                    offset = transform.position - ray.GetPoint(enter);
            }
        }

        if (dragging && moved)
        {
            // choose a drag plane centered on the initial local position so movement is consistent
            if (allowedAxis == Axis.Y)
                dragPlane = new Plane(imageTarget.forward, imageTarget.TransformPoint(initialLocalPos));
            else
                dragPlane = new Plane(imageTarget.up, imageTarget.TransformPoint(initialLocalPos));

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 worldTarget = ray.GetPoint(enter) + offset;
                Vector3 localPos = imageTarget.InverseTransformPoint(worldTarget);

                // lock the two axes that are not allowed
                switch (allowedAxis)
                {
                    case Axis.X:
                        localPos.y = initialLocalPos.y;
                        localPos.z = initialLocalPos.z;
                        break;
                    case Axis.Y:
                        localPos.x = initialLocalPos.x;
                        localPos.z = initialLocalPos.z;
                        break;
                    case Axis.Z:
                        localPos.x = initialLocalPos.x;
                        localPos.y = initialLocalPos.y;
                        break;
                }

                transform.position = imageTarget.TransformPoint(localPos);
            }
        }

        if (ended)
            dragging = false;
    }
}