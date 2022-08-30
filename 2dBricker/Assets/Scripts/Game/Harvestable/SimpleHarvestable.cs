using System.Numerics;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Vorval.CalmBall.Game
{
    public class SimpleHarvestable : AbstractHarvestable
    {
        private BigInteger _score;
        private ScoreService _scoreService;

        [Inject]
        private void Construct(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public override void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            harvestableView.Activate();
            IsActive.Value = true;
            statsService.AddSpawned(Type);
        }

        public override void Deactivate()
        {
            IsActive.Value = false;
            harvestableView.Deactivate(() => gameObject.SetActive(false));
        }

        public override void Harvest(float scoreModifier, Harvester.HarvesterType harvesterType)
        {
            if (!IsActive.Value) return;

            //Debug.Log("Harvested ball");
            //OnHarvested?.Invoke();
            //var score = Mathf.RoundToInt(_score * scoreModifier);
            var score = _score * (BigInteger)(scoreModifier * 1000) / 1000;
            _scoreService.AddScore(score);
            statsService.AddEarnedScore(Type, score);

            if (harvesterType == Harvester.HarvesterType.BlowingHarvestable)
            {
                statsService.AddEarnedScore(HarvestableData.HarvestableType.Blow, score);
            }

            Deactivate();
        }

        protected override void UpdatePower()
        {
            _score = harvestableDataService.GetPower(Type);
        }
    }
}