using System;

namespace Bricker.Game
{
    public interface IHarvestable
    {
        public Action OnHarvested { get; set; }
        public void Harvest();
    }
}