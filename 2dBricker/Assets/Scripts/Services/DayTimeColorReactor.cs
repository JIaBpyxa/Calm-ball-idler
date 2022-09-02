using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Service
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DayTimeColorReactor : MonoBehaviour
    {
        [SerializeField] private Color _dayColor;
        [SerializeField] private Color _nightColor;
        [SerializeField] private float _colorChangeDuration = 1f;

        private SpriteRenderer _spriteRenderer;

        private DayTimeService _dayTimeService;

        [Inject]
        private void Construct(DayTimeService dayTimeService)
        {
            _dayTimeService = dayTimeService;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _dayTimeService.CurrentDayTime.Subscribe(ChangeColorByDayTime);
        }

        private void ChangeColorByDayTime(DayTimeService.DayTime dayTime)
        {
            var color = dayTime == DayTimeService.DayTime.Day ? _dayColor : _nightColor;
            _spriteRenderer.DOColor(color, _colorChangeDuration).SetEase(Ease.InOutCirc);
        }
    }
}