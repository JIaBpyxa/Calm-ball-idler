using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;
using Random = UnityEngine.Random;

namespace Vorval.CalmBall.Game
{
    public class ParallaxRow : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _festivalColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space] [SerializeField] private float _meanMoveDuration = 90f;
        [Space] [SerializeField] private float _parallaxY;

        [Space] [SerializeField] private float _leftEnd;
        [SerializeField] private float rightEnd;
        //[Space] [Range(-1f,1f)] [SerializeField] private float _test = 1f;

        private GyroService _gyroService;

        [Inject]
        private void Construct(ScoreModifierService scoreModifierService, GyroService gyroService)
        {
            _gyroService = gyroService;
            scoreModifierService.IsActive.Subscribe(HandleScoreModifierActivation);
        }

        private void Awake()
        {
            StartMoving();
            EndFestival();
        }

        private void Update()
        {
            var gravity = _gyroService.Gravity;
            //var gravityY = gravity.y;
            var gravityZ = gravity.z;

            //var offset = Mathf.LerpUnclamped(0, _parallaxY, _test);
            var offset = Mathf.LerpUnclamped(0, _parallaxY, gravityZ);

            transform.localPosition = Vector3.up * offset;
        }

        private void StartMoving()
        {
            var isLeftToRight = Random.value > .5f;
            var startPoint = isLeftToRight ? _leftEnd : rightEnd;
            var endPoint = isLeftToRight ? rightEnd : _leftEnd;

            var moveDuration = _meanMoveDuration * Random.Range(.75f, 1.25f);

            var sequence = DOTween.Sequence();
            sequence.Append(_spriteRenderer.transform.DOLocalMoveX(startPoint, .01f).SetEase(Ease.Linear));
            sequence.Append(_spriteRenderer.transform.DOLocalMoveX(endPoint, moveDuration).SetEase(Ease.Linear));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }

        private void HandleScoreModifierActivation(bool isActive)
        {
            if (isActive)
            {
                StartFestival();
            }
            else
            {
                EndFestival();
            }
        }

        private void StartFestival()
        {
            _spriteRenderer.DOColor(_festivalColor, 1f);
        }

        private void EndFestival()
        {
            _spriteRenderer.DOColor(_defaultColor, 1f);
        }
    }
}