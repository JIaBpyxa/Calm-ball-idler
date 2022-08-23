using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class BlowingHarvester : Harvester
    {
        [SerializeField] private BlowingHarvestable _blowingHarvestable;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!_blowingHarvestable.IsActive.Value) return;

            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                if (harvestable.gameObject.Equals(_blowingHarvestable.gameObject)) return;
                if (harvestable.Type == HarvestableData.HarvestableType.Bonus) return;

                HarvestAction(harvestable);
            }
        }
    }
}