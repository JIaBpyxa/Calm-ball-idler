using DG.Tweening;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private float _startHeightPoint = 15f;
        [SerializeField] private float _animationDuration = 3f;
        [SerializeField] private float _heightModifier = 5f;
        [SerializeField] private Camera _camera;

        public Camera MainCamera => _camera;

        private void Awake()
        {
            var safeArea = Screen.safeArea;
            safeArea.yMin = 0f;
            var yOffset = (Screen.height - safeArea.height) / Screen.height * _heightModifier;

            if (_camera != null)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(_camera.transform.DOLocalMoveY(_startHeightPoint, .1f));
                sequence.Append(_camera.transform.DOLocalMoveY(yOffset, _animationDuration));
            }
        }
    }
}