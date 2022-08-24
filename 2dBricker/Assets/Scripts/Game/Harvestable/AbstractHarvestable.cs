using System;
using UniRx;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;
using Object = UnityEngine.Object;

namespace Vorval.CalmBall.Game
{
    public abstract class AbstractHarvestable : MonoBehaviour
    {
        [SerializeField] private HarvestableData.HarvestableType _harvestableType;
        [SerializeField] private float _maxMagnitude = 10f;
        public BoolReactiveProperty IsActive;

        //public Action OnHarvested { get; set; }
        public abstract void Activate(Vector3 position);
        public abstract void Deactivate();
        public abstract void Harvest(float scoreModifier, Harvester.HarvesterType harvesterType);
        protected abstract void UpdatePower();
        public HarvestableData.HarvestableType Type => _harvestableType;

        protected Rigidbody2D rb;
        
        protected AbstractHarvestableView harvestableView;
        protected HarvestableDataService harvestableDataService;
        protected StatsService statsService;


        public class Factory : PlaceholderFactory<Object, AbstractHarvestable>
        {
        }

        [Inject]
        private void Construct(HarvestableDataService dataSvc, StatsService statsSvc)
        {
            harvestableDataService = dataSvc;
            statsService = statsSvc;
        }

        protected virtual void Awake()
        {
            harvestableView = GetComponentInChildren<AbstractHarvestableView>();
            rb = GetComponent<Rigidbody2D>();
            IsActive = new BoolReactiveProperty(false);
            Deactivate();
        }

        private void Start()
        {
            harvestableDataService.OnPowerUpgrade += HandleUpgrade;
            UpdatePower();
        }

        private void FixedUpdate()
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, _maxMagnitude);
        }

        private void OnDestroy()
        {
            harvestableDataService.OnPowerUpgrade -= HandleUpgrade;
        }


        private void HandleUpgrade(HarvestableData.HarvestableType harvestableType)
        {
            if (harvestableType == Type)
            {
                UpdatePower();
            }
        }
    }
}