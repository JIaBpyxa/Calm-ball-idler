using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class SimpleHarvestable : AbstractHarvestable
    {
        [SerializeField] private int _score = 1;

        private ScoreService _scoreService;

        [Inject]
        private void Construct(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public override void Init()
        {
            //IsActive = new BoolReactiveProperty(false);
            //Deactivate();
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
            var score = (int)(_score * scoreModifier);
            _scoreService.AddScore(score);
            Deactivate();
        }
    }
}