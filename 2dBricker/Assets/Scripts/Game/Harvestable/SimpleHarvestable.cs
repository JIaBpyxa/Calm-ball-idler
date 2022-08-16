using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class SimpleHarvestable : AbstractHarvestable
    {
        private float _score;
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
        }

        public override void Deactivate()
        {
            IsActive.Value = false;
            harvestableView.Deactivate(() => gameObject.SetActive(false));
        }

        public override void Harvest(float scoreModifier = 1f)
        {
            if (!IsActive.Value) return;

            Debug.Log("Harvested ball");
            OnHarvested?.Invoke();
            var score = Mathf.RoundToInt(_score * scoreModifier);
            _scoreService.AddScore(score);
            Deactivate();
        }

        protected override void UpdatePower()
        {
            _score = harvestableDataService.GetPower(Type);
        }
    }
}