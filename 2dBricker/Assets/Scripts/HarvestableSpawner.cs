using UniRx;
using UnityEngine;
using Zenject;
using TimeSpan = System.TimeSpan;

namespace Bricker.Game
{
    public class HarvestableSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private GameObject _harvestablePrefab;

        private IHarvestable.Factory _factory;

        [Inject]
        private void Construct(IHarvestable.Factory factory)
        {
            _factory = factory;
        }

        private void Start()
        {
            var aloe = Observable.Interval(TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            aloe.Subscribe(l => SpawnHarvestable());
        }

        private void SpawnHarvestable()
        {
            var newHarvestable = (AbstractBall)_factory.Create(_harvestablePrefab);
            newHarvestable.transform.position = transform.position;
        }
    }
}