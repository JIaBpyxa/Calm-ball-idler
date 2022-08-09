using System;
using Zenject;

namespace Bricker.Game
{
    public interface IHarvestable
    {
        public Action OnHarvested { get; set; }
        public void Harvest();

        public class Factory : PlaceholderFactory<UnityEngine.Object, IHarvestable>
        {
        }
    }
}