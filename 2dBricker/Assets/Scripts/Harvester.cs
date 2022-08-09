using UnityEngine;

namespace Bricker.Game
{
    public class Harvester : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IHarvestable>(out var harvestable))
            {
                harvestable.Harvest();
            }
        }
    }
}