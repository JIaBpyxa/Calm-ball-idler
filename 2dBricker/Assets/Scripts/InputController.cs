using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Space] [SerializeField] private float _magnitudeMultiplier = 1f;
    
    private Vector2 _realPointerPosition;
    private Vector2 _smoothedPointerPosition;

    private void LateUpdate()
    {
        var delta = _realPointerPosition - _smoothedPointerPosition;
        _smoothedPointerPosition += delta * Time.deltaTime * _magnitudeMultiplier;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _realPointerPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        _realPointerPosition = eventData.position;
    }
}