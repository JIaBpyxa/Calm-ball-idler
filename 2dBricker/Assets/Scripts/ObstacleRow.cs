using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using ObservableExtensions = UniRx.ObservableExtensions;
using Random = UnityEngine.Random;

namespace Vorval.CalmBall.Game
{
    public class ObstacleRow : MonoBehaviour
    {
        [SerializeField] private List<ObstaclePack> _obstaclePackPrefabs;
        [Space] [SerializeField] private float _leftEnd;
        [SerializeField] private float _rightEnd;
        [Space] [SerializeField] private float _moveDuration;
        [SerializeField] private float _spawnInterval;

        private Vector3 _position;

        private ObstaclePack.Factory _obstacleFactory;

        [Inject]
        private void Construct(ObstaclePack.Factory obstacleFactory)
        {
            _obstacleFactory = obstacleFactory;
        }

        private void Awake()
        {
            _position = transform.position;
        }

        private void Start()
        {
            SpawnRandomObstacle();
            var spawnObservable =
                Observable.Interval(System.TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            ObservableExtensions.Subscribe(spawnObservable, _ => SpawnRandomObstacle());
        }

        private void SpawnRandomObstacle()
        {
            var randomPrefab = _obstaclePackPrefabs[Random.Range(0, _obstaclePackPrefabs.Count)];
            var obstaclePack = _obstacleFactory.Create(randomPrefab);

            var movingOffsets = GetMovingOffsets();
            var startPoint = _position + Vector3.right * movingOffsets.Item1;
            var endPoint = _position + Vector3.right * movingOffsets.Item2;

            var duration = _moveDuration * Random.Range(.75f, 1.25f);

            obstaclePack.Move(startPoint, endPoint, duration);
        }

        private (float, float) GetMovingOffsets()
        {
            var isLeftToRight = Random.value > .5f;
            return isLeftToRight ? (_leftEnd, _rightEnd) : (_rightEnd, _leftEnd);
        }
    }
}