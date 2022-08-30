using DG.Tweening;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class BlowingHarvestable : AbstractHarvestable
    {
        [SerializeField] private Harvester _harvester;
        [SerializeField] private float _blowDuration = .5f;

        private float _harvesterScaleModifier = 3f;
        private Vector3 _initialHarvesterScale;
        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
            _initialHarvesterScale = _harvester.transform.localScale;
        }

        public override void Activate(Vector3 position)
        {
            transform.position = position;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
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
            Debug.Log("Harvested ball");
            _rigidbody.bodyType = RigidbodyType2D.Static;
            //OnHarvested?.Invoke();
            BlowHarvester();
        }

        protected override void UpdatePower()
        {
<<<<<<< HEAD
            _harvesterScaleModifier = harvestableDataService.GetPower(Type);
=======
            //_harvesterScaleModifier = harvestableDataService.GetFloatPower(Type);
            var power = harvestableDataService.GetFloatPower(Type);
            _harvester.UpdateScoreModifier(power);
>>>>>>> develop
        }

        private void BlowHarvester()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_harvester.transform.DOScale(_initialHarvesterScale * _harvesterScaleModifier,
                _blowDuration * .75f));
            sequence.Append(_harvester.transform.DOScale(_initialHarvesterScale, _blowDuration * .25f));
            sequence.onComplete += Deactivate;
            sequence.Play();
        }
    }
}