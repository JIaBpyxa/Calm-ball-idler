using Vorval.CalmBall.Service;

namespace Vorval.CalmBall.Game
{
    public class SimpleHarvestableSpawner : AbstractHarvestableSpawner
    {
        private HarvestableSpawnerView _spawnerView;

        protected override void Awake()
        {
            base.Awake();
            _spawnerView = GetComponentInChildren<HarvestableSpawnerView>();
        }

        protected void OnEnable()
        {
            //base.OnEnable();
            _harvestableDataService.OnRespawnUpgrade += HandleUpgrade;
            _harvestableDataService.OnHarvestableBought += HandleBuy;
        }

        protected void OnDisable()
        {
            //base.OnDisable();
            _harvestableDataService.OnRespawnUpgrade -= HandleUpgrade;
            _harvestableDataService.OnHarvestableBought -= HandleBuy;
        }

        protected override void Init()
        {
            if (_harvestableDataService.IsBought(type))
            {
                InitPool();
                UpdateSpawnInterval();
                _spawnerView.Unlock();
            }
            else
            {
                _spawnerView.Lock();
            }

            OnOperationFinished?.Invoke(this, LoadingService.LoadingType.SceneLevel);
        }

        protected override void SpawnHarvestable()
        {
            base.SpawnHarvestable();
            _spawnerView.SpawnAction();
        }


        private void HandleBuy(HarvestableData.HarvestableType harvestableType)
        {
            if (harvestableType == type) Init();
        }

        private void HandleUpgrade(HarvestableData.HarvestableType harvestableType)
        {
            if (harvestableType == type) UpdateSpawnInterval();
        }
    }
}