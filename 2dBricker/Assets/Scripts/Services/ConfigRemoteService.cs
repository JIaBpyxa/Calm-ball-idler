using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class ConfigRemoteService : MonoBehaviour, ILoadingOperation
    {
        public Action<RemoteData> OnRemoteDataLoaded { get; set; }
        public Action<ILoadingOperation> OnOperationFinished { get; set; }

        // dev environment
        //private const string EnvironmentId = "6d025401-3b79-4ffe-8265-fb8cae2e94c6";

        // prod environment
        private const string EnvironmentId = "b22c9b19-8212-4572-98c3-3e61d9fa8247";

        private const string SimpleHarvestableDataKey = "simpleHarvestable";
        private const string LittleHarvestableDataKey = "littleHarvestable";
        private const string BlowHarvestableDataKey = "blowHarvestable";
        private const string SlowHarvestableDataKey = "slowHarvestable";
        private const string BonusHarvestableDataKey = "bonusHarvestable";
        private const string RewardedScoreModifierDurationKey = "rewardedScoreModifierDuration";
        private const string RewardedScoreModifierCoefKey = "rewardedScoreModifierCoef";

        private AuthService _authService;
        
        [Inject]
        private void Construct(LoadingService loadingService, AuthService authService)
        {
            loadingService.AddLoadingOperation(this);
            _authService = authService;
        }

        public struct userAttributes
        {
        }

        public struct appAttributes
        {
        }

        private async void Start()
        {
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfigAsync();
            }
            else
            {
                LoadCachedData();
            }

            OnOperationFinished?.Invoke(this);


            void LoadCachedData()
            {
                Debug.Log("Loading Cache data");
                var cacheRemoteData = SaveService.GetRemoteDataCache();
                OnRemoteDataLoaded?.Invoke(cacheRemoteData);
            }
        }

        private async Task InitializeRemoteConfigAsync()
        {
            /*await UnityServices.InitializeAsync();

            // options can be passed in the initializer, e.g if you want to set analytics-user-id use two lines from below:
            // var options = new InitializationOptions().SetOption("com.unity.services.core.analytics-user-id", "my-user-id-123");
            // await UnityServices.InitializeAsync(options);
            // for all valid settings and options, check
            // https://pages.prd.mz.internal.unity3d.com/mz-developer-handbook/docs/sdk/operatesolutionsdk/settings-and-options-for-services

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }*/
            await UniTask.WaitUntil(() => _authService.IsServiceInitialized);
            await UniTask.WaitUntil(() => _authService.IsAnonymousAuthed);

            RemoteConfigService.Instance.FetchCompleted += RemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID(EnvironmentId);

            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        private void RemoteConfigLoaded(ConfigResponse configResponse)
        {
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    Debug.Log("Default values will be returned");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("Cached values loaded");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("Remote Values changed");
                    Debug.Log("RemoteConfigService.Instance.appConfig fetched: " +
                              RemoteConfigService.Instance.appConfig.config.ToString());
                    break;
            }

            LoadRemoteData();


            void LoadRemoteData()
            {
                var simpleJson = RemoteConfigService.Instance.appConfig.GetJson(SimpleHarvestableDataKey);
                var littleJson = RemoteConfigService.Instance.appConfig.GetJson(LittleHarvestableDataKey);
                var blowJson = RemoteConfigService.Instance.appConfig.GetJson(BlowHarvestableDataKey);
                var slowJson = RemoteConfigService.Instance.appConfig.GetJson(SlowHarvestableDataKey);
                var bonusJson = RemoteConfigService.Instance.appConfig.GetJson(BonusHarvestableDataKey);
                var rewardedScoreModifierDuration =
                    RemoteConfigService.Instance.appConfig.GetFloat(RewardedScoreModifierDurationKey);
                var rewardedScoreModifierCoefficient =
                    RemoteConfigService.Instance.appConfig.GetFloat(RewardedScoreModifierCoefKey);


                var remoteData = new RemoteData
                {
                    SimpleHarvestableJson = simpleJson,
                    LittleHarvestableJson = littleJson,
                    BlowHarvestableJson = blowJson,
                    SlowHarvestableJson = slowJson,
                    BonusHarvestableJson = bonusJson,
                    RewardedScoreModifierDuration = rewardedScoreModifierDuration,
                    RewardedScoreModifierCoefficient = rewardedScoreModifierCoefficient
                };

                SaveService.SaveRemoteDataCache(remoteData);
                OnRemoteDataLoaded?.Invoke(remoteData);
            }
        }

        public struct RemoteData
        {
            public string SimpleHarvestableJson;
            public string LittleHarvestableJson;
            public string BlowHarvestableJson;
            public string SlowHarvestableJson;
            public string BonusHarvestableJson;
            public float RewardedScoreModifierDuration;
            public float RewardedScoreModifierCoefficient;
        }
    }
}