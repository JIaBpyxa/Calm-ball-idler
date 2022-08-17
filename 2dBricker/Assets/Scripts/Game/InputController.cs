using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Space] [SerializeField] private float _magnitudeMultiplier = 1f;

        public Vector3ReactiveProperty PointerWorldPosition;
        public Vector3ReactiveProperty PointerPosition;

        private Vector2 _realPointerPosition;
        private Vector2 _smoothedPointerPosition;

        private Camera _camera;
        private Vector3 _cameraOffset;

        private readonly Vector3 _defaultScreenPosition = new(10000, 10000);

        private bool _isPressed;

        private void Awake()
        {
            _camera = Camera.main;
            _cameraOffset = Vector3.forward * _camera.transform.position.z;

            PointerWorldPosition = new Vector3ReactiveProperty(Vector3.zero);
            PointerPosition = new Vector3ReactiveProperty(Vector3.zero);

            PointerPosition.Subscribe(UpdatePointerWorldPosition);
            PointerPosition.Value = _defaultScreenPosition;
        }

        private void LateUpdate()
        {
            if (!_isPressed) return;

            SmoothPointer();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;

            _realPointerPosition = eventData.position;
            _smoothedPointerPosition = _realPointerPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;

            _realPointerPosition = _defaultScreenPosition;
            _smoothedPointerPosition = _realPointerPosition;
            SmoothPointer();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _realPointerPosition = eventData.position;
        }

        private void SmoothPointer()
        {
            var delta = _realPointerPosition - _smoothedPointerPosition;
            _smoothedPointerPosition += delta * Time.deltaTime * _magnitudeMultiplier;
            PointerPosition.Value = _smoothedPointerPosition;
        }

        private void UpdatePointerWorldPosition(Vector3 screenPosition)
        {
            var worldPosition = _camera.ScreenToWorldPoint(screenPosition) - _cameraOffset;
            PointerWorldPosition.Value = worldPosition;
        }
    }
}