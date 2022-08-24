using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class HarvestableSpawnerView : MonoBehaviour
    {
        [SerializeField] private Transform _lockerHolder;
        [SerializeField] private ParticleSystem _particleSystem;

        private bool _isParticlesEnabled;
        private IDisposable _qualityDisposable;

        private GraphicsService _graphicsService;

        [Inject]
        private void Construct(GraphicsService graphicsService)
        {
            _graphicsService = graphicsService;
        }

        private void Awake()
        {
            _isParticlesEnabled = _particleSystem != null;
        }

        private void Start()
        {
            _qualityDisposable = _graphicsService.CurrentQuality.Subscribe(OnQualityUpdate);
        }

        public void Lock()
        {
            _lockerHolder.localPosition = Vector3.up * 1000f;
            _lockerHolder.gameObject.SetActive(true);
            _lockerHolder.DOLocalMoveY(0f, .5f);
        }

        public void Unlock()
        {
            _lockerHolder.DOLocalMoveY(1000f, .5f).SetEase(Ease.InOutExpo).onComplete += () =>
            {
                _lockerHolder.gameObject.SetActive(false);
            };
        }

        public void SpawnAction()
        {
            if (_isParticlesEnabled)
            {
                _particleSystem.Play();
            }
        }

        private void OnQualityUpdate(GraphicsService.Quality quality)
        {
            _isParticlesEnabled = quality == GraphicsService.Quality.Fancy && _particleSystem != null;
        }
    }
}