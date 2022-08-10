using UniRx;
using UnityEngine;

namespace Bricker.Game
{
    public class SimpleHarvestable : AbstractHarvestable
    {
        public override void Init()
        {
            IsActive = new BoolReactiveProperty(false);
            Deactivate();
        }

        public override void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            IsActive.Value = true;
        }

        public override void Deactivate()
        {
            IsActive.Value = false;
            gameObject.SetActive(false);
        }

        public override void Harvest()
        {
            Debug.Log("Harvested ball");
            OnHarvested?.Invoke();
            Deactivate();
        }
    }
}