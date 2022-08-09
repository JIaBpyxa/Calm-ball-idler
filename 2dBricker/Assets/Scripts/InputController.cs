using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Space] [SerializeField] private float _magnitudeMultiplier = 1f;

    public Vector3ReactiveProperty PointerWorldPosition;
    public Vector3ReactiveProperty PointerPosition;

    private Vector2 _realPointerPosition;
    private Vector2 _smoothedPointerPosition;

    private Camera _camera;
    private Vector3 _cameraOffset;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraOffset = _camera.transform.position;

        PointerPosition = new Vector3ReactiveProperty(Vector3.zero);
        PointerWorldPosition = new Vector3ReactiveProperty(Vector3.zero);

        PointerPosition.Subscribe(UpdatePointerWorldPosition);
    }

    private void LateUpdate()
    {
        var delta = _realPointerPosition - _smoothedPointerPosition;
        _smoothedPointerPosition += delta * Time.deltaTime * _magnitudeMultiplier;
        PointerPosition.Value = _smoothedPointerPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //PointerPosition.Value = eventData.position;
        _realPointerPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _realPointerPosition = eventData.position;
        //PointerPosition.Value = eventData.position;
    }

    private void UpdatePointerWorldPosition(Vector3 screenPosition)
    {
        var worldPosition = _camera.ScreenToWorldPoint(screenPosition) - _cameraOffset;
        PointerWorldPosition.Value = worldPosition;
    }
}