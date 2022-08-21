using System.Numerics;
using DG.Tweening;
using I2.Loc;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Service;
using Zenject;
using static Vorval.CalmBall.Game.HarvestableData;
using Vector3 = UnityEngine.Vector3;

namespace Vorval.CalmBall.UI
{
    public class UpgradeDataRowUI : MonoBehaviour
    {
        [SerializeField] private HarvestableType _harvestableType;
        [Space] [SerializeField] private Transform _lockObject;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _buyPriceText;
        [Space] [SerializeField] private TextMeshProUGUI _powerText;
        [SerializeField] private TextMeshProUGUI _respawnText;
        [Space] [SerializeField] private Button _powerUpgradeButton;
        [SerializeField] private Button _respawnUpgradeButton;
        [Space] [SerializeField] private TextMeshProUGUI _powerUpgradePriceText;
        [SerializeField] private TextMeshProUGUI _respawnUpgradePriceText;

        private HarvestableDataService _harvestableDataService;
        private ScoreService _scoreService;

        [Inject]
        private void Construct(HarvestableDataService harvestableDataService, ScoreService scoreService)
        {
            _harvestableDataService = harvestableDataService;
            _scoreService = scoreService;
        }

        private void Start()
        {
            _scoreService.Score.Subscribe(UpdateUpgradeButtons);
            _powerUpgradeButton.onClick.AddListener(BuyPowerUpgrade);
            _respawnUpgradeButton.onClick.AddListener(BuyRespawnUpgrade);
            _buyButton.onClick.AddListener(BuyHarvestable);

            SetupLocker();
            UpdatePowerData();
            UpdateRespawnData();


            void SetupLocker()
            {
                var isBought = _harvestableDataService.IsBought(_harvestableType);
                if (isBought)
                {
                    Unlock(0f);
                }
                else
                {
                    var buyPrice = _harvestableDataService.GetBuyPrice(_harvestableType);
                    _buyPriceText.text = $"{ScriptLocalization.Buy}: {ScoreService.GetStringFromValue(buyPrice)}";
                    Lock();
                }
            }
        }

        private void OnEnable()
        {
            _harvestableDataService.OnPowerUpgrade += HandlePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade += HandleRespawnUpgrade;
            _harvestableDataService.OnHarvestableBought += HandleBought;
        }

        private void OnDisable()
        {
            _harvestableDataService.OnPowerUpgrade -= HandlePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade -= HandleRespawnUpgrade;
            _harvestableDataService.OnHarvestableBought -= HandleBought;
        }

        private void HandleBought(HarvestableType harvestableType)
        {
            if (_harvestableType != harvestableType) return;
            Unlock(.5f);
        }

        private void Lock()
        {
            _lockObject.localPosition = Vector3.down * 6000f;
            _lockObject.gameObject.SetActive(true);
            _lockObject.DOLocalMoveY(0, .5f).SetEase(Ease.InOutExpo);
        }

        private void Unlock(float duration)
        {
            _lockObject.DOLocalMoveX(-2000f, duration).SetEase(Ease.InOutExpo).onComplete += () =>
            {
                _lockObject.gameObject.SetActive(false);
            };
        }

        private void HandlePowerUpgrade(HarvestableType harvestableType)
        {
            if (_harvestableType != harvestableType) return;
            UpdatePowerData();
        }

        private void HandleRespawnUpgrade(HarvestableType harvestableType)
        {
            if (_harvestableType != harvestableType) return;
            UpdateRespawnData();
        }

        private void UpdatePowerData()
        {
            var power = _harvestableDataService.GetPower(_harvestableType);
            _powerText.text = $"{ScriptLocalization.Power}: {power:F}";

            var powerUpgradePrice = _harvestableDataService.GetPowerPrice(_harvestableType);
            _powerUpgradePriceText.text = $"{ScoreService.GetStringFromValue(powerUpgradePrice)}";
        }

        private void UpdateRespawnData()
        {
            var respawnInterval = _harvestableDataService.GetRespawnInterval(_harvestableType);
            _respawnText.text = $"{ScriptLocalization.Interval}: {respawnInterval:F}{ScriptLocalization.Sec}";

            var respawnUpgradePrice = _harvestableDataService.GetRespawnIntervalPrice(_harvestableType);
            _respawnUpgradePriceText.text = $"{ScoreService.GetStringFromValue(respawnUpgradePrice)}";
        }

        private void UpdateUpgradeButtons(BigInteger currentScore)
        {
            var buyPrice = _harvestableDataService.GetBuyPrice(_harvestableType);
            var powerUpgradePrice = _harvestableDataService.GetPowerPrice(_harvestableType);
            var respawnUpgradePrice = _harvestableDataService.GetRespawnIntervalPrice(_harvestableType);

            _buyButton.interactable = _scoreService.IsPurchaseAvailable(buyPrice);
            _powerUpgradeButton.interactable = _scoreService.IsPurchaseAvailable(powerUpgradePrice);
            _respawnUpgradeButton.interactable = _scoreService.IsPurchaseAvailable(respawnUpgradePrice);
        }

        private void BuyHarvestable()
        {
            var buyPrice = _harvestableDataService.GetBuyPrice(_harvestableType);
            _scoreService.ReduceScore(buyPrice);
            _harvestableDataService.BuyHarvestable(_harvestableType);
        }

        private void BuyPowerUpgrade()
        {
            var powerUpgradePrice = _harvestableDataService.GetPowerPrice(_harvestableType);
            _scoreService.ReduceScore(powerUpgradePrice);
            _harvestableDataService.UpgradePower(_harvestableType);
        }

        private void BuyRespawnUpgrade()
        {
            var respawnUpgradePrice = _harvestableDataService.GetRespawnIntervalPrice(_harvestableType);
            _scoreService.ReduceScore(respawnUpgradePrice);
            _harvestableDataService.UpgradeRespawn(_harvestableType);
        }
    }
}