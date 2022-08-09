using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Space] [SerializeField] private float _magnitudeMultiplier = 1f;
    [SerializeField] private float _smoothSpeed = 5f;

    private bool _isPointerDown;
    public Vector2 PointerDownPosition { get; private set; }
    public Vector2 CurrentPointerPosition { get; private set; }

    private void LateUpdate()
    {
        if (_isPointerDown)
        {
            PointerDownPosition += (CurrentPointerPosition - PointerDownPosition) * Time.deltaTime * _smoothSpeed;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        PointerDownPosition = eventData.position;
        CurrentPointerPosition = PointerDownPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerDownPosition = Vector2.zero;
        //_currentPointerPosition = Vector2.zero;

        _isPointerDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        CurrentPointerPosition = eventData.position;
    }
}