<<<<<<< HEAD
﻿using System.Collections.Generic;
=======
﻿using System;
using System.Collections.Generic;
>>>>>>> develop
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vorval.CalmBall.Service
{
    public class LoadingService : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadingCanvasGroup;
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private TextMeshProUGUI _versionText;
<<<<<<< HEAD
=======
        [Space] [SerializeField] private Image _backBalls;
        [SerializeField] private Image _frontBall;

        private Vector3 _backBallsDestination;
        private Vector3 _frontBallDestination;
>>>>>>> develop

        private List<ILoadingOperation> _loadingOperations;
        private int _loadedOperations = 0;

        private void Awake()
        {
            _loadingCanvasGroup.gameObject.SetActive(true);
            _loadingCanvasGroup.alpha = 1f;
<<<<<<< HEAD
=======
            _backBallsDestination = _backBalls.transform.position;
            _frontBallDestination = _frontBall.transform.position;

            var backSequence = DOTween.Sequence();
            var frontSequence = DOTween.Sequence();

            backSequence.Append(_backBalls.transform.DOLocalMove(_backBallsDestination + Vector3.right * 300f, 0f));
            backSequence.Append(_backBalls.transform.DOLocalMove(_backBallsDestination, 1.5f));

            frontSequence.Append(_frontBall.transform.DOLocalMove(_frontBallDestination + Vector3.left * 300f, 0f));
            frontSequence.Append(_frontBall.transform.DOLocalMove(_frontBallDestination, 1.5f));

>>>>>>> develop
            _versionText.text = $"v.{Application.version}";
        }

        public void AddLoadingOperation(ILoadingOperation loadingOperation)
        {
            loadingOperation.OnOperationFinished += HandleOperationLoaded;
            _loadingOperations ??= new List<ILoadingOperation>();
            _loadingOperations.Add(loadingOperation);
        }

        private void HandleOperationLoaded(ILoadingOperation loadingOperation)
        {
            _loadedOperations++;
            var progressPercent = (float)_loadedOperations / _loadingOperations.Count;

            _progressBarImage.DOComplete();
            _progressBarImage.DOFillAmount(progressPercent, .2f).SetEase(Ease.InOutExpo).onComplete +=
                HandleLoadingFinished;

            loadingOperation.OnOperationFinished -= HandleOperationLoaded;
        }

        private void HandleLoadingFinished()
        {
            if (_loadedOperations != _loadingOperations.Count) return;

            _loadedOperations = 0;

            _loadingCanvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutExpo).onComplete += () =>
            {
                _loadingCanvasGroup.gameObject.SetActive(false);
            };
        }
    }
}