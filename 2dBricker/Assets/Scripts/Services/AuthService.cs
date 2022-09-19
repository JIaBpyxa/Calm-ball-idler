using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class AuthService : MonoBehaviour, ILoadingOperation
    {
        public Action OnUnityServicesInitialized { get; set; }
        public Action OnNoInternetInitialized { get; set; }
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }
        private string _playerId;


        [Inject]
        private void Construct(LoadingService loadingService)
        {
            //_loadingService = loadingService;
            loadingService.AddLoadingOperation(this, LoadingService.LoadingType.GameLevel);
        }

        private async Task Awake()
        {
            if (Utilities.CheckForInternetConnection())
            {
                await UnityServices.InitializeAsync();
                await SignInAnonymously();
                OnUnityServicesInitialized?.Invoke();

                GameServices.Instance.LogIn(LoginComplete);
            }
            else
            {
                OnNoInternetInitialized?.Invoke();
            }
        }

        private async Task SignInAnonymously()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                _playerId = AuthenticationService.Instance.PlayerId;

                Debug.Log("Signed in as: " + _playerId);
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                // Take some action here...
                Debug.Log(s);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private void LoginComplete(bool success)
        {
            if (success)
            {
                Debug.Log("Login success");
            }
            else
            {
                Debug.Log("Login failed");
            }

            OnOperationFinished?.Invoke(this, LoadingService.LoadingType.GameLevel);
        }
    }
}