using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadingCanvasGroup;
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private TextMeshProUGUI _versionText;
        [Space] [SerializeField] private Image _backBalls;
        [SerializeField] private Image _frontBall;

        private Vector3 _backBallsDestination;
        private Vector3 _frontBallDestination;

        private float _currentLoadingProgress = 0f;

        private LoadingService _loadingService;

        [Inject]
        private void Construct(LoadingService loadingService)
        {
            _loadingService = loadingService;
        }

        private void Awake()
        {
            _loadingCanvasGroup.gameObject.SetActive(true);
            _loadingCanvasGroup.alpha = 1f;
            _backBallsDestination = _backBalls.transform.position;
            _frontBallDestination = _frontBall.transform.position;

            var backSequence = DOTween.Sequence();
            var frontSequence = DOTween.Sequence();

            backSequence.Append(_backBalls.transform.DOLocalMove(_backBallsDestination + Vector3.right * 300f, 0f));
            backSequence.Append(_backBalls.transform.DOLocalMove(_backBallsDestination, 1.5f));

            frontSequence.Append(_frontBall.transform.DOLocalMove(_frontBallDestination + Vector3.left * 300f, 0f));
            frontSequence.Append(_frontBall.transform.DOLocalMove(_frontBallDestination, 1.5f));

            _versionText.text = $"v.{Application.version}";
        }

        private void Start()
        {
            _loadingService.LoadingProgress.Subscribe(HandleLoadingProgressChanged);
        }

        private void HandleLoadingProgressChanged(float progress)
        {
            if (progress < _currentLoadingProgress)
            {
                return;
            }

            var duration = progress - _currentLoadingProgress;

            _progressBarImage.DOComplete();
            _progressBarImage.DOFillAmount(progress, duration).SetEase(Ease.InOutExpo).onComplete += () =>
            {
                HandleBarMoved(progress);
            };
        }

        private void HandleBarMoved(float progress)
        {
            if (progress < 1f)
            {
                return;
            }

            _loadingCanvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutExpo).onComplete += () =>
            {
                _loadingCanvasGroup.gameObject.SetActive(false);
            };
        }
    }
}