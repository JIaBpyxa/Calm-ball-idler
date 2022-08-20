using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Game;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class StatsDataPanelUI : AbstractPanelUI
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _titleText;
        [Space] [SerializeField] private TextMeshProUGUI _spawnedText;
        [SerializeField] private TextMeshProUGUI _earnedScoreText;
        [SerializeField] private TextMeshProUGUI _meanEarnedScoreText;
        [SerializeField] private TextMeshProUGUI _powerLevelText;
        [SerializeField] private TextMeshProUGUI _intervalLevelText;

        private HarvestableData.HarvestableType _harvestableType;

        private PanelController _panelController;
        private StatsService _statsService;
        private HarvestableDataService _harvestableDataService;

        [Inject]
        private void Construct(PanelController panelController, StatsService statsService,
            HarvestableDataService harvestableDataService)
        {
            _panelController = panelController;
            _statsService = statsService;
            _harvestableDataService = harvestableDataService;
        }

        private void Start()
        {
            _exitButton.onClick.AddListener(_panelController.GoBackward);
            _harvestableType = _statsService.ChosenHarvestableStats;
            _titleText.text = _harvestableDataService.GetHarvestableName(_harvestableType);
            _powerLevelText.text =
                $"{_harvestableDataService.GetPowerLevel(_harvestableType)} {ScriptLocalization.Level}";
            _intervalLevelText.text =
                $"{_harvestableDataService.GetRespawnIntervalLevel(_harvestableType)} {ScriptLocalization.Level}";

            UpdateData();
        }

        private void OnEnable()
        {
            _statsService.OnHarvestableStatsUpdate += HandleStatsUpdate;
        }

        private void OnDisable()
        {
            _statsService.OnHarvestableStatsUpdate -= HandleStatsUpdate;
        }

        private void HandleStatsUpdate(HarvestableData.HarvestableType harvestableType)
        {
            if (_harvestableType == harvestableType)
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            var harvestableData = _statsService.GetHarvestableStatsData(_harvestableType);
            _spawnedText.text = $"{harvestableData.SpawnedCount}";
            _earnedScoreText.text = $"{harvestableData.ScoreEarned}";

            if (_harvestableType == HarvestableData.HarvestableType.Blow)
            {
                _meanEarnedScoreText.text = "-";
            }
            else
            {
                var meanEarned = 60 / _harvestableDataService.GetRespawnInterval(_harvestableType) *
                                 _harvestableDataService.GetPower(_harvestableType);
                _meanEarnedScoreText.text = $"{meanEarned}/{ScriptLocalization.Min}";
            }
        }
    }
}