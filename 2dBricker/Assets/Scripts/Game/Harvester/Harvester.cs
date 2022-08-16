using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class Harvester : MonoBehaviour
    {
        [SerializeField] protected HarvesterType _harvesterType;
        [SerializeField] protected float _scoreModifier = 1f;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AbstractHarvestable>(out var harvestable))
            {
                harvestable.Harvest(_scoreModifier, _harvesterType);
            }
        }

        public enum HarvesterType
        {
            BottomZone = 0,
            Player = 1,
            RedZone = 2,
            BlowingHarvestable = 10,
        }
    }
}