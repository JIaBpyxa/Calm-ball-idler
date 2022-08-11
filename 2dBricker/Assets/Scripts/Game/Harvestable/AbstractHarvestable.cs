using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public abstract class AbstractHarvestable : MonoBehaviour
    {
        public BoolReactiveProperty IsActive;

        public Action OnHarvested { get; set; }
        public abstract void Init();
        public abstract void Activate(Vector3 position);
        public abstract void Deactivate();
        public abstract void Harvest(float scoreModifier = 1f);

        protected AbstractHarvestableView harvestableView;
        
        public class Factory : PlaceholderFactory<UnityEngine.Object, AbstractHarvestable>
        {
        }

        protected virtual void Awake()
        {
            harvestableView = GetComponentInChildren<AbstractHarvestableView>();
        }
    }
}