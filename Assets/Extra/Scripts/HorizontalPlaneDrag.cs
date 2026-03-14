using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HorizontalPlaneDrag : MonoBehaviour
{
    private Vector3 _dragOffset;
    private float _originalY;
    private Camera _mainCamera;
    private Plane _movementPlane;

    void Start()
    {
        _originalY = transform.position.y;
        _mainCamera = Camera.main;
        CreateMovementPlane();
    }

    void CreateMovementPlane()
    {
        _movementPlane = new Plane(Vector3.up, new Vector3(0, _originalY, 0));
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleTouch(touch.position, touch.phase);
        }
        else if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0)) HandleTouch(Input.mousePosition, TouchPhase.Began);
            if (Input.GetMouseButton(0)) HandleTouch(Input.mousePosition, TouchPhase.Moved);
            if (Input.GetMouseButtonUp(0)) HandleTouch(Input.mousePosition, TouchPhase.Ended);
        }
    }

    void HandleTouch(Vector2 screenPosition, TouchPhase phase)
    {
        switch (phase)
        {
            case TouchPhase.Began:
                if (IsTouchingObject(screenPosition))
                {
                    StoreDragOffset(screenPosition);
                }
                break;

            case TouchPhase.Moved:
                if (IsDragging)
                {
                    UpdatePosition(screenPosition);
                }
                break;

            case TouchPhase.Ended:
                IsDragging = false;
                break;
        }
    }

    bool IsTouchingObject(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        return Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform;
    }

    void StoreDragOffset(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (_movementPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPosition = ray.GetPoint(enter);
            _dragOffset = transform.position - worldPosition;
            IsDragging = true;
        }
    }

    void UpdatePosition(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (_movementPlane.Raycast(ray, out float enter))
        {
            Vector3 newPosition = ray.GetPoint(enter) + _dragOffset;
            newPosition.y = _originalY; // Maintain original Y position
            transform.position = newPosition;
        }
    }

    private bool IsDragging = false;
}