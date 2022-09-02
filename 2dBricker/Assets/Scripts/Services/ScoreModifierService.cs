﻿using System;
using System.Numerics;
using UniRx;
using Vorval.CalmBall.Game;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class ScoreModifierService : ILoadingOperation
    {
        public Action<ILoadingOperation> OnOperationFinished { get; set; }
        public Action<bool> OnBonusEarned { get; set; }
        public readonly BoolReactiveProperty IsActive;
        public float RewardedScoreModifierDuration { get; private set; }
        public FloatReactiveProperty RewardedScoreModifierCoefficient { get; }
        public FloatReactiveProperty CurrentScoreModifierCoefficient { get; }

        private AdsService _adsService;
        private HarvestableDataService _harvestableDataService;
        private ScoreService _scoreService;

        [Inject]
        private ScoreModifierService(HarvestableDataService harvestableDataService,
            ConfigRemoteService configRemoteService, AdsService adsService, ScoreService scoreService,
            LoadingService loadingService)
        {
            _harvestableDataService = harvestableDataService;
            _scoreService = scoreService;

            configRemoteService.OnRemoteDataLoaded += UpdateRemoteData;
            adsService.OnRewardedCompleted += HandleRewardedAdCompleted;
            IsActive = new BoolReactiveProperty(false);
            CurrentScoreModifierCoefficient = new FloatReactiveProperty(1f);
            RewardedScoreModifierCoefficient = new FloatReactiveProperty(2f);

            loadingService.AddLoadingOperation(this);
        }

        public BigInteger GetMeanEarnings()
        {
            return _harvestableDataService.GetOpenMeanEarnings();
        }

        public BigInteger GetBonusMeanEarnings()
        {
            var power = _harvestableDataService.GetFloatPower(HarvestableData.HarvestableType.Bonus);
            return _harvestableDataService.GetOpenMeanEarnings(power);
        }

        public void AddMeanEarnings()
        {
            var meanScore = _harvestableDataService.GetOpenMeanEarnings();
            _scoreService.AddScore(meanScore);
            OnBonusEarned?.Invoke(false);
        }

        private void UpdateRemoteData(ConfigRemoteService.RemoteData remoteData)
        {
            RewardedScoreModifierDuration = remoteData.RewardedScoreModifierDuration;
            RewardedScoreModifierCoefficient.Value = remoteData.RewardedScoreModifierCoefficient / 1000f;

            OnOperationFinished?.Invoke(this);
        }

        private void HandleRewardedAdCompleted(AdsService.RewardedType rewardedType)
        {
            if (rewardedType == AdsService.RewardedType.ScoreModifier)
            {
                ActivateRewardedScoreModifier();
            }
            else if (rewardedType == AdsService.RewardedType.ScoreBonus)
            {
                var meanScore = GetBonusMeanEarnings();
                _scoreService.AddScore(meanScore);
                OnBonusEarned?.Invoke(true);
            }
        }

        private void ActivateRewardedScoreModifier()
        {
            IsActive.Value = true;
            CurrentScoreModifierCoefficient.Value = RewardedScoreModifierCoefficient.Value;
            var saveObservable = Observable.Timer(TimeSpan.FromSeconds(RewardedScoreModifierDuration));
            ObservableExtensions.Subscribe(saveObservable, _ => ResetScoreModifier());

            void ResetScoreModifier()
            {
                IsActive.Value = false;
                CurrentScoreModifierCoefficient.Value = 1f;
            }
        }
    }
}