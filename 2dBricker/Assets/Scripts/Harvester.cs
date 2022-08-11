using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class Harvester : MonoBehaviour
    {
        [SerializeField] private float _scoreModifier = 1f;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                harvestable.Harvest(_scoreModifier);
            }
        }
    }
}