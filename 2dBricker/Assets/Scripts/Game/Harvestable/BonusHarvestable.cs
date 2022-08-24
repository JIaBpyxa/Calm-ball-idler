using UnityEngine;
using Vorval.CalmBall.UI;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class BonusHarvestable : AbstractHarvestable
    {
        private PanelController _panelController;

        [Inject]
        private void Construct(PanelController panelController)
        {
            _panelController = panelController;
        }

        public override void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            harvestableView.Activate();
            IsActive.Value = true;
        }

        public override void Deactivate()
        {
            IsActive.Value = false;
            harvestableView.Deactivate(() => gameObject.SetActive(false));
        }

        public override void Harvest(float scoreModifier, Harvester.HarvesterType harvesterType)
        {
            if (!IsActive.Value) return;
            if (harvesterType == Harvester.HarvesterType.Player)
            {
                _panelController.OpenPanel(PanelController.PanelType.BonusHarvestablePanel);
            }

            Deactivate();
        }

        protected override void UpdatePower()
        {
        }
    }
}