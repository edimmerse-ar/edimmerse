using UnityEngine;
using Vuforia;
using Lean.Touch;

public class SimpleImageTargetDrag : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] float _dampening = 5f;
    [SerializeField] LayerMask _touchLayer;

    private Camera _arCamera;
    private bool _isDragging;
    private Vector3 _targetPosition;
    private Vector3 _offset;

    void Start()
    {
        _arCamera = VuforiaBehaviour.Instance.GetComponent<Camera>();
        _targetPosition = transform.position;
    }

    void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }

    void Update()
    {
        if (_isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _dampening * Time.deltaTime);
        }
    }

    void HandleFingerDown(LeanFinger finger)
    {
        if (IsTouchOnObject(finger.ScreenPosition))
        {
            _isDragging = true;
            CalculateOffset(finger.ScreenPosition);
        }
    }

    void HandleFingerUpdate(LeanFinger finger)
    {
        if (!_isDragging) return;

        // Convert screen touch to image target plane
        Ray ray = _arCamera.ScreenPointToRay(finger.ScreenPosition);
        Plane imagePlane = new Plane(transform.parent.forward, transform.parent.position);

        if (imagePlane.Raycast(ray, out float enter))
        {
            _targetPosition = ray.GetPoint(enter) + _offset;
        }
    }

    void HandleFingerUp(LeanFinger finger) => _isDragging = false;

    void CalculateOffset(Vector2 screenPosition)
    {
        Ray ray = _arCamera.ScreenPointToRay(screenPosition);
        Plane imagePlane = new Plane(transform.parent.forward, transform.parent.position);

        if (imagePlane.Raycast(ray, out float enter))
        {
            _offset = transform.position - ray.GetPoint(enter);
        }
    }

    bool IsTouchOnObject(Vector2 screenPosition)
    {
        Ray ray = _arCamera.ScreenPointToRay(screenPosition);
        return Physics.Raycast(ray, out RaycastHit hit, 100f, _touchLayer) && hit.transform == transform;
    }
}