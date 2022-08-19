using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class UnityMediationService
    {
        public Action OnAdLoaded;
        public Action OnUserRewarded;

        public bool IsRewardedLoaded => _rewardedAd.AdState == AdState.Loaded;
        
        private IRewardedAd _rewardedAd;
        private const string adUnitId = "Rewarded_Android";
        private const string gameId = "4889981";

        public async Task InitServices()
        {
            try
            {
                InitializationOptions initializationOptions = new InitializationOptions();
                initializationOptions.SetGameId(gameId);
                await UnityServices.InitializeAsync(initializationOptions);

                InitializationComplete();
            }
            catch (Exception e)
            {
                InitializationFailed(e);
            }
        }

        public async void ShowAd()
        {
            if (_rewardedAd.AdState == AdState.Loaded)
            {
                try
                {
                    var showOptions = new RewardedAdShowOptions { AutoReload = true };
                    await _rewardedAd.ShowAsync(showOptions);
                    AdShown();
                }
                catch (ShowFailedException e)
                {
                    AdFailedShow(e);
                }
            }
        }

        private void SetupAd()
        {
            //Create
            _rewardedAd = MediationService.Instance.CreateRewardedAd(adUnitId);

            //Subscribe to events
            _rewardedAd.OnClosed += RewardedAdClosed;
            _rewardedAd.OnClicked += RewardedAdClicked;
            _rewardedAd.OnLoaded += RewardedAdLoaded;
            _rewardedAd.OnFailedLoad += RewardedAdFailedLoad;
            _rewardedAd.OnUserRewarded += UserRewarded;

            // Impression Event
            MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
        }

        private void InitializationComplete()
        {
            SetupAd();
            LoadAd();
        }

        private async void LoadAd()
        {
            try
            {
                await _rewardedAd.LoadAsync();
            }
            catch (LoadFailedException)
            {
                // We will handle the failure in the OnFailedLoad callback
            }
        }

        private void InitializationFailed(Exception e)
        {
            Debug.Log("Initialization Failed: " + e.Message);
        }

        private void RewardedAdLoaded(object sender, EventArgs e)
        {
            Debug.Log("Ad loaded");
            OnAdLoaded?.Invoke();
        }

        private void RewardedAdFailedLoad(object sender, LoadErrorEventArgs e)
        {
            Debug.Log("Failed to load ad");
            Debug.Log(e.Message);
        }

        private void AdShown()
        {
            Debug.Log("Ad shown!");
        }

        private void RewardedAdClosed(object sender, EventArgs e)
        {
            Debug.Log("Ad has closed");
            // Execute logic after an ad has been closed.
        }

        private void RewardedAdClicked(object sender, EventArgs e)
        {
            Debug.Log("Ad has been clicked");
            // Execute logic after an ad has been clicked.
        }

        private void AdFailedShow(ShowFailedException e)
        {
            Debug.Log(e.Message);
        }

        private void ImpressionEvent(object sender, ImpressionEventArgs args)
        {
            var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
            Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
        }

        private void UserRewarded(object sender, RewardEventArgs e)
        {
            Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");
            OnUserRewarded?.Invoke();
        }
    }
}