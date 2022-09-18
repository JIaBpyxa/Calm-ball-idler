using System;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;
using Object = UnityEngine.Object;

namespace Vorval.CalmBall.Game
{
    [RequireComponent(typeof(HarvestablePool))]
    public abstract class AbstractHarvestableSpawner : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }

        [SerializeField] protected HarvestableData.HarvestableType type;
        [SerializeField] protected Object _harvestablePrefab;
        [SerializeField] private int _initCount;

        protected float spawnInterval = 3f;
        protected IDisposable spawnDisposable;

        private HarvestablePool _harvestablePool;
        protected HarvestableDataService _harvestableDataService;
        protected AudioService _audioService;

        [Inject]
        private void Construct(HarvestableDataService harvestableDataSvc, LoadingService loadingSvc,
            AudioService audioSvc)
        {
            _harvestableDataService = harvestableDataSvc;
            _audioService = audioSvc;
            loadingSvc.AddLoadingOperation(this, LoadingService.LoadingType.SceneLevel);
        }

        protected virtual void Awake()
        {
            _harvestablePool = GetComponent<HarvestablePool>();
        }

        private void Start()
        {
            Init();
        }

        protected void InitPool()
        {
            _harvestablePool.Init(_harvestablePrefab, _initCount);
        }

        protected virtual void SpawnHarvestable()
        {
            var newHarvestable = _harvestablePool.GetHarvestable();
            newHarvestable.Activate(transform.position);
            _audioService.PlayHarvestableSpawnedEffect(type);
        }

        protected void UpdateSpawnInterval()
        {
            spawnInterval = _harvestableDataService.GetRespawnInterval(type);
            spawnDisposable?.Dispose();
            var spawnObservable =
                Observable.Interval(TimeSpan.FromSeconds(spawnInterval)).TakeUntilDisable(this);
            spawnDisposable = ObservableExtensions.Subscribe(spawnObservable, _ => SpawnHarvestable());
        }

        protected abstract void Init();
    }
}