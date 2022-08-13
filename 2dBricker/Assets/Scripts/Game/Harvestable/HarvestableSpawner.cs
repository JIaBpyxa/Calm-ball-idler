using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;


namespace Vorval.CalmBall.Game
{
    [RequireComponent(typeof(HarvestablePool))]
    public class HarvestableSpawner : MonoBehaviour
    {
        [SerializeField] private HarvestableType type;
        [SerializeField] private Object _harvestablePrefab;

        private float _spawnInterval = 3f;
        private System.IDisposable _spawnDisposable;

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
            _harvestablePool.Init(_harvestablePrefab, 5);
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
            _spawnInterval = _harvestableDataService.GetRespawnInterval(type);
            _spawnDisposable?.Dispose();
            var spawnObservable =
                Observable.Interval(System.TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            _spawnDisposable = spawnObservable.Subscribe(_ => SpawnHarvestable());
            //spawnDisposable = spawnObservable.Subscribe(() => { SpawnHarvestable(); });
        }
    }
}