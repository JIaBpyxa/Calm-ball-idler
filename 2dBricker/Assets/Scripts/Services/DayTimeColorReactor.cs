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
        [SerializeField] private bool _isFestival;
        [SerializeField] private Color _festivalColor;
        [SerializeField] private float _colorChangeDuration = 1f;

        private SpriteRenderer _spriteRenderer;

        private DayTimeService _dayTimeService;
        private ScoreModifierService _scoreModifierService;

        [Inject]
        private void Construct(DayTimeService dayTimeService, ScoreModifierService scoreModifierService)
        {
            _dayTimeService = dayTimeService;
            _scoreModifierService = scoreModifierService;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _dayTimeService.CurrentDayTime.Subscribe(ChangeColorByDayTime);
            if (_isFestival)
            {
                _scoreModifierService.IsActive.Subscribe(ChangeColorByFestival);
            }
        }

        private void ChangeColor(Color color)
        {
            _spriteRenderer.DOColor(color, _colorChangeDuration).SetEase(Ease.InOutCirc);
        }

        private void ChangeColorByFestival(bool isActive)
        {
            if (isActive)
            {
                ChangeColor(_festivalColor);
            }
            else
            {
                var dayTime = _dayTimeService.CurrentDayTime.Value;
                ChangeColorByDayTime(dayTime);
            }
        }

        private void ChangeColorByDayTime(DayTimeService.DayTime dayTime)
        {
            if (_scoreModifierService.IsActive.Value && _isFestival) return;

            var color = dayTime == DayTimeService.DayTime.Day ? _dayColor : _nightColor;
            ChangeColor(color);
        }
    }
}