using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vorval.CalmBall.Service
{
    public class WindService : MonoBehaviour
    {
        [SerializeField] private ParticleSystemForceField _particleSystemForceField;
        [SerializeField] private AreaEffector2D _areaEffector;
        [Space] [SerializeField] private float _speedChangeInterval = 25f;
        [Space] [SerializeField] private float _minSpeed = -10f;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _minChangeValue = .5f;
        [Range(0f, 1f)] [SerializeField] private float _changeRatioValue = .2f;

        private float _windSpeed = 1f;

        private void Start()
        {
            var spawnObservable =
                Observable.Interval(TimeSpan.FromSeconds(_speedChangeInterval)).TakeUntilDisable(this);
            ObservableExtensions.Subscribe(spawnObservable, _ => ChangeWindSpeed());

            ChangeWindSpeed();
        }

        private void ChangeWindSpeed()
        {
            var changeValue = Mathf.Max(_minChangeValue,
                Mathf.Abs(_windSpeed * Random.Range(-_changeRatioValue, _changeRatioValue)));
            _windSpeed += changeValue * (Random.Range(0, 2) * 2 - 1);
            _windSpeed = Mathf.Clamp(_windSpeed, _minSpeed, _maxSpeed);

            _areaEffector.forceMagnitude = _windSpeed;
            _particleSystemForceField.directionX = _windSpeed;
        }
    }
}