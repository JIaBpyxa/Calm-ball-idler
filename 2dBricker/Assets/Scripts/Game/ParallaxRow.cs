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
        [SerializeField] private Color _nightColor;
        [SerializeField] private Color _festivalColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space] [SerializeField] private float _meanMoveDuration = 90f;
        [Space] [SerializeField] private float _parallaxY;

        [Space] [SerializeField] private float _leftEnd;

        [SerializeField] private float rightEnd;
        //[Space] [Range(-1f,1f)] [SerializeField] private float _test = 1f;

        private GyroService _gyroService;
        private DayTimeService _dayTimeService;

        [Inject]
        private void Construct(ScoreModifierService scoreModifierService, GyroService gyroService,
            DayTimeService dayTimeService)
        {
            _gyroService = gyroService;
            _dayTimeService = dayTimeService;
            scoreModifierService.IsActive.Subscribe(HandleScoreModifierActivation);

            _dayTimeService.CurrentDayTime.Subscribe(ChangeColorByDayTime);
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
            ChangeColor(_festivalColor);
        }

        private void EndFestival()
        {
            ChangeColorByDayTime(_dayTimeService.CurrentDayTime.Value);
        }

        private void ChangeColor(Color color)
        {
            _spriteRenderer.DOColor(color, 1f);
        }

        private void ChangeColorByDayTime(DayTimeService.DayTime dayTime)
        {
            var color = _dayTimeService.CurrentDayTime.Value == DayTimeService.DayTime.Day
                ? _defaultColor
                : _nightColor;
            ChangeColor(color);
        }
    }
}