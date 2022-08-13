using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public abstract class AbstractHarvestable : MonoBehaviour
    {
        [SerializeField] private HarvestableType _harvestableType;
        public BoolReactiveProperty IsActive;

        public Action OnHarvested { get; set; }
        public abstract void Activate(Vector3 position);
        public abstract void Deactivate();
        public abstract void Harvest(float scoreModifier = 1f);
        protected abstract void UpdatePower();
        public HarvestableType Type => _harvestableType;

        protected AbstractHarvestableView harvestableView;
        protected HarvestableDataService harvestableDataService;


        public class Factory : PlaceholderFactory<UnityEngine.Object, AbstractHarvestable>
        {
        }

        [Inject]
        private void Construct(HarvestableDataService dataService)
        {
            harvestableDataService = dataService;
        }

        protected virtual void Awake()
        {
            harvestableView = GetComponentInChildren<AbstractHarvestableView>();
            IsActive = new BoolReactiveProperty(false);
            Deactivate();
        }

        private void Start()
        {
            harvestableDataService.OnPowerUpgrade += HandleUpgrade;
            UpdatePower();
        }

        private void OnDestroy()
        {
            harvestableDataService.OnPowerUpgrade -= HandleUpgrade;
        }


        private void HandleUpgrade(HarvestableType harvestableType, int power)
        {
            if (harvestableType == Type)
            {
                UpdatePower();
            }
        }
    }
}