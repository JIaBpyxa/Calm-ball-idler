using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class BlowingHarvester : Harvester
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                if (harvestable.gameObject.Equals(transform.parent.gameObject)) return;

                harvestable.Harvest(_scoreModifier, _harvesterType);
            }
        }
    }
}