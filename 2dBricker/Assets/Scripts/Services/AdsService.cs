﻿using System;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class AdsService : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }
        public Action OnRewardedAdLoaded { get; set; }
        public Action<RewardedType> OnRewardedCompleted { get; set; }

        public bool IsRewardedAdLoaded => _mediationService?.IsRewardedLoaded ?? false;

        private UnityMediationService _mediationService;
        private RewardedType _chosenRewardedType;

        [Inject]
        private void Construct(LoadingService loadingService)
        {
            loadingService.AddLoadingOperation(this, LoadingService.LoadingType.GameLevel);
        }

        private async void Start()
        {
            if (Utilities.CheckForInternetConnection())
            {
                await InitServices();
            }

            OnOperationFinished?.Invoke(this, LoadingService.LoadingType.GameLevel);


            async Task InitServices()
            {
                _mediationService = new UnityMediationService();
                await _mediationService.InitServices();
                InitEvents();
            }

            void InitEvents()
            {
                _mediationService.OnAdLoaded += OnRewardedAdLoaded;
                _mediationService.OnUserRewarded += HandleRewardedCompleted;
            }
        }

        public void ShowRewardedAd(RewardedType rewardedType)
        {
            _chosenRewardedType = rewardedType;
            _mediationService.ShowAd();
        }

        private void HandleRewardedCompleted()
        {
            OnRewardedCompleted?.Invoke(_chosenRewardedType);
        }

        public enum RewardedType
        {
            ScoreModifier,
            ScoreBonus
        }
    }
}