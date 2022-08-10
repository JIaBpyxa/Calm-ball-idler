using UniRx;
using UnityEngine;
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
            IsActive = new BoolReactiveProperty(false);
            Deactivate();
        }

        public override void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            IsActive.Value = true;
        }

        public override void Deactivate()
        {
            IsActive.Value = false;
            gameObject.SetActive(false);
        }

        public override void Harvest()
        {
            Debug.Log("Harvested ball");
            OnHarvested?.Invoke();
            _scoreService.AddScore(_score);
            Deactivate();
        }
    }
}