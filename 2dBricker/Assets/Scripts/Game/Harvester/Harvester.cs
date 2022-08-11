using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class Harvester : MonoBehaviour
    {
        [SerializeField] protected float _scoreModifier = 1f;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                harvestable.Harvest(_scoreModifier);
            }
        }
    }
}