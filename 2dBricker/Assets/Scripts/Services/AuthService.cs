using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class AuthService : MonoBehaviour
    {
        [SerializeField] private FacebookService _facebookService;
        public bool IsServiceInitialized { get; private set; } = false;
        public bool IsAnonymousAuthed { get; private set; } = false;

        private async void Start()
        {
            if (Utilities.CheckForInternetConnection())
            {
                // UnityServices.InitializeAsync() will initialize all services that are subscribed to Core.
                await UnityServices.InitializeAsync();
                IsServiceInitialized = true;
                Debug.Log(UnityServices.State);

                SetupEvents();

                await SignInAnonymouslyAsync();
                IsAnonymousAuthed = true;
            }


            void SetupEvents()
            {
                AuthenticationService.Instance.SignedIn += () =>
                {
                    // Shows how to get a playerID
                    Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                    // Shows how to get an access token
                    Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
                };

                AuthenticationService.Instance.SignInFailed += err => { Debug.LogError(err); };

                AuthenticationService.Instance.SignedOut += () => { Debug.Log("Player signed out."); };

                AuthenticationService.Instance.Expired += () =>
                {
                    Debug.Log("Player session could not be refreshed and expired.");
                };
            }
        }

        /// <summary>
        /// Signs in anonymously: uses the session token to login to an existing account if it exists, otherwise creates an account and caches the session token.
        /// </summary>
        private async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        public void LoginFacebook()
        {
            if (IsServiceInitialized)
            {
                _facebookService.Login();
            }
        }


        /// <summary>
        /// Signs the player out.
        /// </summary>
        void SignOut()
        {
            AuthenticationService.Instance.SignOut();
            Debug.Log("Signed out!");
        }

        /// <summary>
        /// Deletes the session token from the cache to allow logging in to a new account.
        /// </summary>
        void ClearSessionToken()
        {
            try
            {
                AuthenticationService.Instance.ClearSessionToken();
                Debug.Log("Session Token Cleared!");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Debug.Log("Failed to clear session token!");
            }
        }

        /// <summary>
        /// Retrieves the player info, including the external ids.
        /// </summary>
        async Task GetPlayerInfoAsync()
        {
            try
            {
                await AuthenticationService.Instance.GetPlayerInfoAsync();
                Debug.Log("Obtained PlayerInfo!");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Debug.Log("Failed to get PlayerInfo!");
            }
        }
    }
}