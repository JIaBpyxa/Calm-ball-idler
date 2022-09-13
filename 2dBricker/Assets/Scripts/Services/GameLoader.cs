using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Vorval.CalmBall.Service
{
    public class GameLoader : MonoBehaviour, ILoadingOperation
    {
        public Action<ILoadingOperation> OnOperationFinished { get; set; }

        [Inject]
        private void Construct(LoadingService loadingService)
        {
            loadingService.AddLoadingOperation(this);
        }

        private void Awake()
        {
            SceneManager.LoadSceneAsync("Scenes/IdleScene", LoadSceneMode.Additive).completed += HandleCompleted;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void HandleCompleted(AsyncOperation obj)
        {
            StartCoroutine(WaitAndFinish());


            IEnumerator WaitAndFinish()
            {
                yield return new WaitForSeconds(1f);
                OnOperationFinished.Invoke(this);
                SceneManager.UnloadSceneAsync("InitScene");
            }
        }
    }
}