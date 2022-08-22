using System;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class HarvesterView : MonoBehaviour
    {
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

        private void OnDestroy()
        {
            _qualityDisposable.Dispose();
        }

        public void HarvestAction(Vector2 position)
        {
            if (_isParticlesEnabled)
            {
                _particleSystem.transform.position = position;
                _particleSystem.Play();
            }
        }

        private void OnQualityUpdate(GraphicsService.Quality quality)
        {
            _isParticlesEnabled = quality == GraphicsService.Quality.Fancy && _particleSystem != null;
        }
    }
}