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
        [SerializeField] private HarvestableData.HarvestableType type;
        [SerializeField] private Object _harvestablePrefab;

        private float _spawnInterval = 3f;
        private IDisposable _spawnDisposable;

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

        private void OnEnable()
        {
            _harvestableDataService.OnServiceReady += Init;
            _harvestableDataService.OnRespawnUpgrade += HandleUpgrade;
        }

        private void OnDisable()
        {
            _harvestableDataService.OnServiceReady -= Init;
            _harvestableDataService.OnRespawnUpgrade -= HandleUpgrade;
        }

        private void Init()
        {
            _harvestablePool.Init(_harvestablePrefab, 5);
            UpdateSpawnInterval();
        }


        private void SpawnHarvestable()
        {
            var newHarvestable = _harvestablePool.GetHarvestable();
            newHarvestable.Activate(transform.position);
        }

        private void HandleUpgrade(HarvestableData.HarvestableType harvestableType)
        {
            if (harvestableType == type)
            {
                UpdateSpawnInterval();
            }
        }

        private void UpdateSpawnInterval()
        {
            _spawnInterval = _harvestableDataService.GetRespawnInterval(type);
            _spawnDisposable?.Dispose();
            var spawnObservable =
                Observable.Interval(TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            _spawnDisposable = ObservableExtensions.Subscribe(spawnObservable, _ => SpawnHarvestable());
            //spawnDisposable = spawnObservable.Subscribe(() => { SpawnHarvestable(); });
        }
    }
}