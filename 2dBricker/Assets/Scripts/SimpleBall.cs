using UnityEngine;

namespace Bricker.Game
{
    public class SimpleBall : AbstractBall
    {
        public override void Harvest()
        {
            Debug.Log("Harvested ball");
            OnHarvested?.Invoke();
        }
    }
}