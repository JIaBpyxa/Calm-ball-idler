using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class ConfigRemoteService : MonoBehaviour
    {
        public Action<RemoteData> OnRemoteDataLoaded;

        // dev environment
        private const string EnvironmentId = "6d025401-3b79-4ffe-8265-fb8cae2e94c6";

        // prod environment
        //private const string EnvironmentId = "b22c9b19-8212-4572-98c3-3e61d9fa8247";
        private const string SimpleHarvestableData = "simpleHarvestable";
        private const string LittleHarvestableData = "littleHarvestable";
        private const string BlowHarvestableData = "blowHarvestable";
        private const string SlowHarvestableData = "slowHarvestable";

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
        }

        private async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();

            // options can be passed in the initializer, e.g if you want to set analytics-user-id use two lines from below:
            // var options = new InitializationOptions().SetOption("com.unity.services.core.analytics-user-id", "my-user-id-123");
            // await UnityServices.InitializeAsync(options);
            // for all valid settings and options, check
            // https://pages.prd.mz.internal.unity3d.com/mz-developer-handbook/docs/sdk/operatesolutionsdk/settings-and-options-for-services

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

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

            var simpleJson = RemoteConfigService.Instance.appConfig.GetJson(SimpleHarvestableData);
            var littleJson = RemoteConfigService.Instance.appConfig.GetJson(LittleHarvestableData);
            var blowJson = RemoteConfigService.Instance.appConfig.GetJson(BlowHarvestableData);
            var slowJson = RemoteConfigService.Instance.appConfig.GetJson(SlowHarvestableData);

            var remoteData = new RemoteData(simpleJson, littleJson, blowJson, slowJson);
            OnRemoteDataLoaded?.Invoke(remoteData);
        }

        public class RemoteData
        {
            public string SimpleHarvestableJson { get; }
            public string LittleHarvestableJson { get; }
            public string BlowHarvestableJson { get; }
            public string SlowHarvestableJson { get; }

            public RemoteData(
                string simpleHarvestableJson,
                string littleHarvestableJson,
                string blowHarvestableJson,
                string slowHarvestableJson)
            {
                SimpleHarvestableJson = simpleHarvestableJson;
                LittleHarvestableJson = littleHarvestableJson;
                BlowHarvestableJson = blowHarvestableJson;
                SlowHarvestableJson = slowHarvestableJson;
            }
        }
    }
}