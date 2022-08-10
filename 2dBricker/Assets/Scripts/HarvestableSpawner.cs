using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace Bricker.Game
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
            var aloe = Observable.Interval(TimeSpan.FromSeconds(_spawnInterval)).TakeUntilDisable(this);
            ObservableExtensions.Subscribe(aloe, _ => SpawnHarvestable());
        }

        private void SpawnHarvestable()
        {
            var newHarvestable = _harvestablePool.GetHarvestable();
            newHarvestable.Activate(transform.position);
        }
    }
}