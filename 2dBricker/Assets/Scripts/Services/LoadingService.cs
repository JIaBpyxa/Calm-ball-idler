using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Vorval.CalmBall.Service
{
    public class LoadingService : MonoBehaviour
    {
        public FloatReactiveProperty LoadingProgress { get; } = new(0f);
        public Action OnGameLevelReady { get; set; }

        private List<ILoadingOperation> _gameLevelLoadingOperations;
        private List<ILoadingOperation> _sceneLevelLoadingOperations;

        private int _gameLevelOperationsLeft = 0;
        private int _sceneLevelOperationsLeft = 0;

        private void Awake()
        {
            _gameLevelLoadingOperations = new List<ILoadingOperation>();
            _sceneLevelLoadingOperations = new List<ILoadingOperation>();

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void AddLoadingOperation(ILoadingOperation loadingOperation, LoadingType loadingType)
        {
            loadingOperation.OnOperationFinished += HandleOperationLoaded;
            if (loadingType == LoadingType.GameLevel)
            {
                _gameLevelLoadingOperations ??= new List<ILoadingOperation>();
                _gameLevelLoadingOperations.Add(loadingOperation);
                _gameLevelOperationsLeft++;
            }
            else
            {
                _sceneLevelLoadingOperations ??= new List<ILoadingOperation>();
                _sceneLevelLoadingOperations.Add(loadingOperation);
                _sceneLevelOperationsLeft++;
            }
        }

        private void HandleOperationLoaded(ILoadingOperation loadingOperation, LoadingType loadingType)
        {
            if (loadingType == LoadingType.GameLevel)
            {
                _gameLevelOperationsLeft--;

                if (_gameLevelOperationsLeft == 0)
                {
                    Debug.Log($"Game level operations completed");
                    OnGameLevelReady?.Invoke();
                }
            }
            else
            {
                _sceneLevelOperationsLeft--;
            }

            Debug.Log(
                $"Game operations left {_gameLevelOperationsLeft} Scene operations left {_sceneLevelOperationsLeft}");

            var progressPercent = Mathf.Clamp01(1f - (float)(_gameLevelOperationsLeft + _sceneLevelOperationsLeft) /
                (_gameLevelLoadingOperations.Count + _sceneLevelLoadingOperations.Count));

            LoadingProgress.Value = progressPercent;

            loadingOperation.OnOperationFinished -= HandleOperationLoaded;
        }

        public enum LoadingType
        {
            GameLevel,
            SceneLevel
        }
    }
}