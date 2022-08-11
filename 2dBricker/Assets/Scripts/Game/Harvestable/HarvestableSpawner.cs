using UniRx;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    [RequireComponent(typeof(HarvestablePool))]
    public class HarvestableSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private Object _harvestablePrefab;

        private HarvestablePool _harvestablePool;

        private void Awake()
        {
            _harvestablePool = GetComponent<HarvestablePool>();
        }

        private void Start()
        {
            _harvestablePool.Init(_harvestablePrefab, 10);
            var spawnObservable =
                Observable.Interval(System.TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            spawnObservable.Subscribe(_ => SpawnHarvestable());
        }

        private void SpawnHarvestable()
        {
            var newHarvestable = _harvestablePool.GetHarvestable();
            newHarvestable.Activate(transform.position);
        }
    }
}