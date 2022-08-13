using System;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Vorval.CalmBall.Game
{
    [RequireComponent(typeof(HarvestablePool))]
    public class HarvestableSpawner : MonoBehaviour
    {
        [SerializeField] private HarvestableType type;
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private Object _harvestablePrefab;

        private HarvestablePool _harvestablePool;
        private HarvestableDataService _harvestableDataService;

        [Inject]
        private void Construct(HarvestableDataService harvestableDataService)
        {
            _harvestableDataService = harvestableDataService;
        }

        private void Awake()
        {
            _harvestablePool = GetComponent<HarvestablePool>();
        }

        private void Start()
        {
            _harvestablePool.Init(_harvestablePrefab, 10);
            var spawnObservable =
                Observable.Interval(TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            ObservableExtensions.Subscribe(spawnObservable, _ => SpawnHarvestable());

            _harvestableDataService.OnRespawnUpgrade += HandleUpgrade;
            UpdateSpawnInterval();
        }

        private void OnDestroy()
        {
            _harvestableDataService.OnRespawnUpgrade -= HandleUpgrade;
        }

        private void SpawnHarvestable()
        {
            var newHarvestable = _harvestablePool.GetHarvestable();
            newHarvestable.Activate(transform.position);
        }

        private void HandleUpgrade(HarvestableType harvestableType)
        {
            if (harvestableType == type)
            {
                UpdateSpawnInterval();
            }
        }

        private void UpdateSpawnInterval()
        {
            _spawnInterval = _harvestableDataService.GetRespawnDelay(type);
        }
    }
}