using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Game;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class StatsButtonUI : MonoBehaviour
    {
        [SerializeField] private HarvestableData.HarvestableType _harvestableType;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _button;

        private HarvestableDataService _harvestableDataService;
        private StatsService _statsService;
        private PanelController _panelController;

        [Inject]
        private void Construct(HarvestableDataService harvestableDataService, StatsService statsService,
            PanelController panelController)
        {
            _harvestableDataService = harvestableDataService;
            _statsService = statsService;
            _panelController = panelController;
        }

        private void Start()
        {
            _titleText.text = _harvestableDataService.GetHarvestableName(_harvestableType);
            _button.onClick.AddListener(OpenStatsDataPanel);
        }

        private void OpenStatsDataPanel()
        {
            _statsService.ChooseHarvestableStats(_harvestableType);
            _panelController.OpenPanel(PanelController.PanelType.StatsData);
        }
    }
}