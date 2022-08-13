using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class ConfigService : MonoBehaviour
    {
        // dev environment
        public static string EnvironmentId = "6d025401-3b79-4ffe-8265-fb8cae2e94c6";
        // prod environment
        //public static string EnvironmentId = "b22c9b19-8212-4572-98c3-3e61d9fa8247";
        
        public struct userAttributes
        {
        }

        public struct appAttributes
        {
        }

        private async Task Start()
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

            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        private void ApplyRemoteSettings(ConfigResponse configResponse)
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
        }
    }
}