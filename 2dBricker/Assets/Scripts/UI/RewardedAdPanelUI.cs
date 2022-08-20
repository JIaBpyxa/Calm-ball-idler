using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class RewardedAdPanelUI : AbstractPanelUI
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _watchButton;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private PanelController _panelController;
        private ScoreModifierService _scoreModifierService;
        private AdsService _adsService;

        [Inject]
        private void Construct(PanelController panelController, ScoreModifierService scoreModifierService,
            AdsService adsService)
        {
            _panelController = panelController;
            _scoreModifierService = scoreModifierService;
            _adsService = adsService;
        }

        private void Awake()
        {
            _exitButton.onClick.AddListener(_panelController.ForceClosePanel);
        }

        private void Start()
        {
            _watchButton.onClick.AddListener(ShowRewarded);
            var localizationParamsManager = _descriptionText.GetComponent<LocalizationParamsManager>();

            var coefficient = _scoreModifierService.RewardedScoreModifierCoefficient;
            var duration = Mathf.RoundToInt(_scoreModifierService.RewardedScoreModifierDuration / 60);

            localizationParamsManager.SetParameterValue("DURATION", $"{duration}");
            localizationParamsManager.SetParameterValue("MODIFIER", $"{coefficient:f1}");
            //_descriptionText.text = $"Get x{coefficient:f1} for {duration} minutes by watching ad";
        }

        private void ShowRewarded()
        {
            _panelController.ForceClosePanel();
            _adsService.ShowRewardedAd(AdsService.RewardedType.ScoreModifier);
        }
    }
}