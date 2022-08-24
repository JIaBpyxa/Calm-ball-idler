using System;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class Harvester : MonoBehaviour
    {
        [SerializeField] protected HarvesterView _harvesterView;
        [SerializeField] protected HarvesterType _harvesterType;
        [SerializeField] protected float _defaultScoreModifier = 1f;

        protected float scoreModifier;
        private IDisposable _scoreDisposable;

        protected AudioService audioService;
        private ScoreModifierService _scoreModifierService;

        [Inject]
        private void Construct(ScoreModifierService scoreModifierService, AudioService audioSvc)
        {
            _scoreModifierService = scoreModifierService;
            _scoreDisposable =
                _scoreModifierService.CurrentScoreModifierCoefficient.Subscribe(HandleScoreModifierChanged);
            audioService = audioSvc;
        }

        private void Awake()
        {
            scoreModifier = _defaultScoreModifier;
        }

        private void OnDestroy()
        {
            _scoreDisposable.Dispose();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                HarvestAction(harvestable);
            }
        }

        protected void HarvestAction(AbstractHarvestable harvestable)
        {
            harvestable.Harvest(scoreModifier, _harvesterType);
            audioService.PlayHarvestedEffect(_harvesterType);
            _harvesterView.HarvestAction(harvestable.transform.position);
        }

        private void HandleScoreModifierChanged(float modifier)
        {
            scoreModifier = _defaultScoreModifier * modifier;
        }

        public enum HarvesterType
        {
            BottomZone = 0,
            Player = 1,
            RedZone = 2,
            BlowingHarvestable = 10,
        }
    }
}