using System;
using TMPro;
using UniRx;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _statsButton;
        [SerializeField] private Button _rewardedScoreModifierButton;
        [SerializeField] private TextMeshProUGUI _rewardedScoreModifierCoefficientText;

        private bool _isAdReady;

        private PanelController _panelController;
        private AdsService _adsService;
        private ScoreModifierService _scoreModifierService;

        [Inject]
        private void Construct(PanelController panelController, AdsService adsService,
            ScoreModifierService scoreModifierService)
        {
            _panelController = panelController;
            _adsService = adsService;
            _scoreModifierService = scoreModifierService;
            _scoreModifierService.IsActive.Subscribe(HandleRewardedScoreActivation);
            _scoreModifierService.RewardedScoreModifierCoefficient.Subscribe(UpdateScoreModifierText);
        }

        private void Awake()
        {
            if (_settingsButton != null) _settingsButton.onClick.AddListener(OpenSettings);
            if (_shopButton != null) _shopButton.onClick.AddListener(OpenShop);
            if (_statsButton != null) _statsButton.onClick.AddListener(OpenStats);
            if (_rewardedScoreModifierButton != null)
                _rewardedScoreModifierButton.onClick.AddListener(OpenRewardedScorePanel);

            _rewardedScoreModifierButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _adsService.OnRewardedAdLoaded += HandleAdLoaded;
        }


        private void OnDisable()
        {
            _adsService.OnRewardedAdLoaded -= HandleAdLoaded;
        }

        private void UpdateScoreModifierText(float coefficient)
        {
            _rewardedScoreModifierCoefficientText.text = $"x{coefficient:f1}";
        }

        private void HandleAdLoaded()
        {
            var isActiveScoreModifier = _scoreModifierService.IsActive.Value;
            var isShow = !isActiveScoreModifier;
            _rewardedScoreModifierButton.gameObject.SetActive(isShow);
        }

        private void HandleRewardedScoreActivation(bool isActive)
        {
            var isAdLoaded = _adsService.IsRewardedAdLoaded;
            var isShow = !isActive && isAdLoaded;
            _rewardedScoreModifierButton.gameObject.SetActive(isShow);
        }

        private void OpenSettings()
        {
            _panelController.OpenPanel(PanelController.PanelType.Settings);
        }

        private void OpenShop()
        {
            _panelController.OpenPanel(PanelController.PanelType.Shop);
        }

        private void OpenStats()
        {
            _panelController.OpenPanel(PanelController.PanelType.Stats);
        }

        private void OpenRewardedScorePanel()
        {
            _panelController.OpenPanel(PanelController.PanelType.RewardedAdPanel);
        }
    }
}