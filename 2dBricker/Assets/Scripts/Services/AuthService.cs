using System;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class AuthService : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }


        [Inject]
        private void Construct(LoadingService loadingService)
        {
            //_loadingService = loadingService;
            loadingService.AddLoadingOperation(this, LoadingService.LoadingType.GameLevel);
        }

        private void Awake()
        {
            GameServices.Instance.LogIn(LoginComplete);
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