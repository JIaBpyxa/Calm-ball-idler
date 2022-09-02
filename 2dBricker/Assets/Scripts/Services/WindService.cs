using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Vorval.CalmBall.Service
{
    public class WindService : MonoBehaviour
    {
        [SerializeField] private ParticleSystemForceField _particleSystemForceField;
        [SerializeField] private AreaEffector2D _areaEffector;
        [Space] [SerializeField] private float _speedChangeInterval = 10f;
        [Space] [SerializeField] private float _minSpeed = -10f;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _minChangeValue = 1f;
        [Range(0f, 1f)] [SerializeField] private float _changeRatioValue = .5f;

        public FloatReactiveProperty WindSpeed { get; } = new(1f);

        [Inject]
        private void Construct(DayTimeService dayTimeService)
        {
            dayTimeService.CurrentDayTime.Subscribe(HandleDayTimeChanged);
        }

        private void Start()
        {
            var spawnObservable =
                Observable.Interval(TimeSpan.FromSeconds(_speedChangeInterval)).TakeUntilDisable(this);
            ObservableExtensions.Subscribe(spawnObservable, _ => ChangeWindSpeed());

            ChangeWindSpeed();
        }

        private void ChangeWindSpeed()
        {
            var curValue = WindSpeed.Value;
            var changeValue = Mathf.Max(_minChangeValue,
                Mathf.Abs(curValue * Random.Range(-_changeRatioValue, _changeRatioValue)));
            curValue += changeValue * (Random.Range(0, 2) * 2 - 1);
            curValue = Mathf.Clamp(curValue, _minSpeed, _maxSpeed);

            _areaEffector.forceMagnitude = curValue;
            _particleSystemForceField.directionX = curValue;

            WindSpeed.Value = curValue;
        }

        private void HandleDayTimeChanged(DayTimeService.DayTime dayTime)
        {
            _minChangeValue = dayTime == DayTimeService.DayTime.Day ? 1f : 3f;
        }
    }
}