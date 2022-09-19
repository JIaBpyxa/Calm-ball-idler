using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Vorval.CalmBall.Game;
using Zenject;
using static Vorval.CalmBall.Service.AnalyticsConstants;

namespace Vorval.CalmBall.Service
{
    public class AnalyticsEventService : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }
        private HarvestableDataService _harvestableDataService;
        private AdsService _adsService;
        private ScoreModifierService _scoreModifierService;
        private AuthService _authService;

        private bool _isInited = false;

        [Inject]
        private void Construct(HarvestableDataService harvestableDataService,
            AdsService adsService, ScoreModifierService scoreModifierService, AuthService authService)
        {
            //loadingService.AddLoadingOperation(this, LoadingService.LoadingType.GameLevel);
            _harvestableDataService = harvestableDataService;
            _adsService = adsService;
            _scoreModifierService = scoreModifierService;
            _authService = authService;
        }

        /*private async void Start()
        {
            if (Utilities.CheckForInternetConnection())
            {
                try
                {
                    await UnityServices.InitializeAsync();

                    var consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
                }
                catch (ConsentCheckException e)
                {
                    // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
                }
            }

            OnOperationFinished?.Invoke(this, LoadingService.LoadingType.GameLevel);
        }*/

        private void OnEnable()
        {
            _authService.OnUnityServicesInitialized += Init;
            _harvestableDataService.OnHarvestableBought += HandleHarvestableBought;
            _harvestableDataService.OnPowerUpgrade += HandleHarvestablePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade += HandleHarvestableRespawnUpgrade;
            _adsService.OnRewardedCompleted += HandleRewardedAdsCompleted;
            _scoreModifierService.OnBonusEarned += HandleBonusEarned;
        }

        private void OnDisable()
        {
            _authService.OnUnityServicesInitialized -= Init;
            _harvestableDataService.OnHarvestableBought -= HandleHarvestableBought;
            _harvestableDataService.OnPowerUpgrade -= HandleHarvestablePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade -= HandleHarvestableRespawnUpgrade;
            _adsService.OnRewardedCompleted -= HandleRewardedAdsCompleted;
            _scoreModifierService.OnBonusEarned -= HandleBonusEarned;
        }

        private void Init()
        {
            try
            {
                var consentIdentifiers = AnalyticsService.Instance.CheckForRequiredConsents();
                _isInited = true;
            }
            catch (ConsentCheckException e)
            {
                // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            }

            //OnOperationFinished?.Invoke(this, LoadingService.LoadingType.GameLevel);
        }

        private void HandleHarvestableBought(HarvestableData.HarvestableType harvestableType)
        {
            if (!_isInited) return;

            var parameters = new Dictionary<string, object>
            {
                { HarvestableTypeIdParam, (int)harvestableType }
            };

            AnalyticsService.Instance.CustomData(BoughtHarvestableEvent, parameters);
        }

        private void HandleHarvestablePowerUpgrade(HarvestableData.HarvestableType harvestableType)
        {
            var upgradeLevel = _harvestableDataService.GetPowerLevel(harvestableType);
            HandleHarvestableUpgrade(harvestableType, PowerUpgradeType, upgradeLevel);
        }

        private void HandleHarvestableRespawnUpgrade(HarvestableData.HarvestableType harvestableType)
        {
            var upgradeLevel = _harvestableDataService.GetRespawnIntervalLevel(harvestableType);
            HandleHarvestableUpgrade(harvestableType, RespawnUpgradeType, upgradeLevel);
        }

        private void HandleHarvestableUpgrade(HarvestableData.HarvestableType harvestableType, string upgradeType,
            int upgradeLevel)
        {
            if (!_isInited) return;

            var availableUpgradeLevel = new[] { 1, 5, 10, 15, 20, 30, 40, 50, 75, 100 };

            if (!availableUpgradeLevel.Contains(upgradeLevel)) return;

            var parameters = new Dictionary<string, object>
            {
                { HarvestableTypeIdParam, (int)harvestableType },
                { UpgradeLevelParam, upgradeLevel },
                { UpgradeTypeParam, upgradeType }
            };

            AnalyticsService.Instance.CustomData(UpgradedHarvestableEvent, parameters);
        }

        private void HandleRewardedAdsCompleted(AdsService.RewardedType rewardedType)
        {
            if (!_isInited) return;

            if (rewardedType == AdsService.RewardedType.ScoreModifier)
            {
                AnalyticsService.Instance.CustomData(GetScoreModifierViaADEvent, new Dictionary<string, object>());
            }
        }

        private void HandleBonusEarned(bool isWatchedAd)
        {
            if (!_isInited) return;

            var parameters = new Dictionary<string, object>
            {
                { IsWatchedAdParam, isWatchedAd },
            };

            AnalyticsService.Instance.CustomData(BonusScoreEarnedEvent, parameters);
        }
    }

    public static class AnalyticsConstants
    {
        public const string BoughtHarvestableEvent = "boughtHarvestable";
        public const string UpgradedHarvestableEvent = "upgradedHarvestable";
        public const string GetScoreModifierViaADEvent = "getScoreModifierViaAD";
        public const string BonusScoreEarnedEvent = "bonusScoreEarned";
        public const string IsWatchedAdParam = "isWatchedAd";
        public const string HarvestableTypeIdParam = "harvestableTypeId";
        public const string UpgradeLevelParam = "upgradeLevel";
        public const string UpgradeTypeParam = "upgradeType";
        public const string PowerUpgradeType = "power";
        public const string RespawnUpgradeType = "respawn";
    }
}