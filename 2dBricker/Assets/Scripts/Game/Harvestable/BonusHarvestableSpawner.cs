using Unity.Services.RemoteConfig;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class BonusHarvestableSpawner : AbstractHarvestableSpawner
    {
        private AdsService _adsService;

        [Inject]
        private void Construct(AdsService adsSvc)
        {
            _adsService = adsSvc;
        }

        protected override void Init()
        {
            if (Utilities.CheckForInternetConnection())
            {
                InitPool();
                UpdateSpawnInterval();
            }

            OnOperationFinished?.Invoke(this, LoadingService.LoadingType.SceneLevel);
        }

        protected override void SpawnHarvestable()
        {
            if (_adsService.IsRewardedAdLoaded)
            {
                base.SpawnHarvestable();
            }
        }
    }
}