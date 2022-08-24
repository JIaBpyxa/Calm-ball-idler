using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class BonusHarvestablePanelUI : AbstractPanelUI
    {
        [SerializeField] private Button _simpleGetButton;
        [SerializeField] private Button _adGetButton;
        [SerializeField] private LocalizationParamsManager _simpleParamsManager;
        [SerializeField] private LocalizationParamsManager _adsParamsManager;

        private float _adModifier = 1f;

        private PanelController _panelController;
        private ScoreModifierService _scoreModifierService;
        private AdsService _adsService;

        [Inject]
        private void Construct(PanelController panelController, AdsService adsService,
            ScoreModifierService scoreModifierService)
        {
            _panelController = panelController;
            _adsService = adsService;
            _scoreModifierService = scoreModifierService;
        }

        private void Start()
        {
            _simpleGetButton.onClick.AddListener(HandleSimpleGetButton);
            _adGetButton.onClick.AddListener(HandleAdGetButton);
            _simpleParamsManager.SetParameterValue("BONUS",
                $"{ScoreService.GetStringFromValue(_scoreModifierService.GetMeanEarnings())}");
            _adsParamsManager.SetParameterValue("AD_BONUS",
                $"{ScoreService.GetStringFromValue(_scoreModifierService.GetBonusMeanEarnings())}");
        }

        private void HandleSimpleGetButton()
        {
            _panelController.ForceClosePanel();
            _scoreModifierService.AddMeanEarnings();
        }

        private void HandleAdGetButton()
        {
            _adsService.ShowRewardedAd(AdsService.RewardedType.ScoreBonus);
            _panelController.ForceClosePanel();
        }
    }
}