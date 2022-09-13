using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Unity;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class FacebookService : MonoBehaviour
    {
        private void Awake()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
            
            
            void InitCallback()
            {
                if (FB.IsInitialized)
                {
                    // Signal an app activation App Event
                    FB.ActivateApp();
                    // Continue with Facebook SDK
                    // ...
                }
                else
                {
                    Debug.Log("Failed to Initialize the Facebook SDK");
                }
            }
        }

        public void Login()
        {
            var perms = new List<string> { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
            
            
            void AuthCallback(ILoginResult result)
            {
                if (FB.IsLoggedIn)
                {
                    // AccessToken class will have session details
                    var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                    // Print current access token's User ID
                    Debug.Log(aToken.UserId);
                    // Print current access token's granted permissions
                    foreach (string perm in aToken.Permissions)
                    {
                        Debug.Log(perm);
                    }

                    LinkWithFacebookAsync(aToken.TokenString);
                }
                else
                {
                    Debug.Log("User cancelled login");
                }
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// When the player triggers the Facebook login by signing in or by creating a new player profile,
        /// and you have received the Facebook access token, call the following API to authenticate the player
        /// </summary>
        /// <param name="accessToken">The facebook user access token.</param>
        private async Task SignInWithFacebookAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithFacebookAsync(accessToken);
                Debug.Log("Signed in with Facebook!");
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                Debug.Log("Failed to sign in with Facebook!");
            }
        }

        /// <summary>
        /// When the player wants to upgrade from being anonymous to creating a Facebook social account and sign in using Facebook,
        /// the game should prompt the player to trigger the Facebook login and get the access token from Facebook.
        /// Then, call the following API to link the player to the Facebook Access token
        /// </summary>
        /// <param name="accessToken">The facebook user access token.</param>
        async Task LinkWithFacebookAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithFacebookAsync(accessToken);
                Debug.Log("Linked with Facebook!");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                Debug.Log("Linking failed. Account is already linked!");
                Debug.LogException(ex);
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to link with Facebook!");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// The player can be offered to unlink his facebook account.
        /// The game should call this api.
        /// Unlinking requires the facebook player info to be present.
        /// </summary>
        async Task UnlinkFacebookAsync()
        {
            try
            {
                await AuthenticationService.Instance.UnlinkFacebookAsync();
                Debug.Log("Unlinked Facebook account!");
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                Debug.Log("Failed to unlink Facebook account!");
            }
        }
    }
}