using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class Harvester : MonoBehaviour
    {
        [SerializeField] protected HarvesterType _harvesterType;
        [SerializeField] protected float _defaultScoreModifier = 1f;

        protected float scoreModifier;

        protected AudioService audioService;
        private ScoreModifierService _scoreModifierService;

        [Inject]
        private void Construct(ScoreModifierService scoreModifierService, AudioService audioSvc)
        {
            _scoreModifierService = scoreModifierService;
            _scoreModifierService.CurrentScoreModifierCoefficient.Subscribe(HandleScoreModifierChanged);
            audioService = audioSvc;
        }

        private void Awake()
        {
            scoreModifier = _defaultScoreModifier;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                harvestable.Harvest(scoreModifier, _harvesterType);
                audioService.PlayHarvestedEffect(_harvesterType);
            }
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