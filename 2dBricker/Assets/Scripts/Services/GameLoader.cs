using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class GameLoader : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation, LoadingService.LoadingType> OnOperationFinished { get; set; }

        private LoadingService _loadingService;
        
        [Inject]
        private void Construct(LoadingService loadingService)
        {
            _loadingService = loadingService;
            loadingService.AddLoadingOperation(this, LoadingService.LoadingType.SceneLevel);
        }

        private void OnEnable()
        {
            _loadingService.OnGameLevelReady += HandleGameLevelReady;
        }

        private void OnDisable()
        {
            _loadingService.OnGameLevelReady -= HandleGameLevelReady;
        }

        private void HandleGameLevelReady()
        {
            SceneManager.LoadSceneAsync("Scenes/IdleScene", LoadSceneMode.Additive).completed += HandleCompleted;
        }
        
        private void HandleCompleted(AsyncOperation obj)
        {
            StartCoroutine(WaitAndFinish());


            IEnumerator WaitAndFinish()
            {
                yield return new WaitForSeconds(1f);
                OnOperationFinished.Invoke(this, LoadingService.LoadingType.SceneLevel);
                SceneManager.UnloadSceneAsync("InitScene");
            }
        }
    }
}