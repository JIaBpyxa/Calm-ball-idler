using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Bricker.Game
{
    public abstract class AbstractHarvestable : MonoBehaviour
    {
        public BoolReactiveProperty IsActive;

        public Action OnHarvested { get; set; }
        public abstract void Init();
        public abstract void Activate(Vector3 position);
        public abstract void Deactivate();
        public abstract void Harvest();
        
        public class Factory : PlaceholderFactory<UnityEngine.Object, AbstractHarvestable>
        {
        }
    }
}