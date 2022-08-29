using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using Vorval.CalmBall.Game;
using Zenject;
using static Vorval.CalmBall.Service.AnalyticsConstants;

namespace Vorval.CalmBall.Service
{
    public class AnalyticsEventService : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation> OnOperationFinished { get; set; }
        private HarvestableDataService _harvestableDataService;
        private AdsService _adsService;
        private ScoreModifierService _scoreModifierService;

        [Inject]
        private void Construct(LoadingService loadingService, HarvestableDataService harvestableDataService,
            AdsService adsService, ScoreModifierService scoreModifierService)
        {
            loadingService.AddLoadingOperation(this);
            _harvestableDataService = harvestableDataService;
            _adsService = adsService;
            _scoreModifierService = scoreModifierService;
        }

        private async void Start()
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

            OnOperationFinished?.Invoke(this);
        }

        private void OnEnable()
        {
            _harvestableDataService.OnHarvestableBought += HandleHarvestableBought;
            _harvestableDataService.OnPowerUpgrade += HandleHarvestablePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade += HandleHarvestableRespawnUpgrade;
            _adsService.OnRewardedCompleted += HandleRewardedAdsCompleted;
            _scoreModifierService.OnBonusEarned += HandleBonusEarned;
        }

        private void OnDisable()
        {
            _harvestableDataService.OnHarvestableBought -= HandleHarvestableBought;
            _harvestableDataService.OnPowerUpgrade -= HandleHarvestablePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade -= HandleHarvestableRespawnUpgrade;
            _adsService.OnRewardedCompleted -= HandleRewardedAdsCompleted;
            _scoreModifierService.OnBonusEarned -= HandleBonusEarned;
        }

        private void HandleHarvestableBought(HarvestableData.HarvestableType harvestableType)
        {
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
            if (rewardedType == AdsService.RewardedType.ScoreModifier)
            {
                AnalyticsService.Instance.CustomData(GetScoreModifierViaADEvent, new Dictionary<string, object>());
            }
        }

        private void HandleBonusEarned(bool isWatchedAd)
        {
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