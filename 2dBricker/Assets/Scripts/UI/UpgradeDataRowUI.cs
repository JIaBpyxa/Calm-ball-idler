using System.Numerics;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vorval.CalmBall.Game;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.UI
{
    public class UpgradeDataRowUI : MonoBehaviour
    {
        [SerializeField] private HarvestableType _harvestableType;
        [Space] [SerializeField] private Transform _lockObject;
        [SerializeField] private Button _buyButton;
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

            UpdatePowerData();
            UpdateRespawnData();
        }

        private void OnEnable()
        {
            _harvestableDataService.OnPowerUpgrade += HandlePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade += HandleRespawnUpgrade;
        }

        private void OnDisable()
        {
            _harvestableDataService.OnPowerUpgrade -= HandlePowerUpgrade;
            _harvestableDataService.OnRespawnUpgrade -= HandleRespawnUpgrade;
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
            _powerText.text = $"Power: {power:F}";
        }

        private void UpdateRespawnData()
        {
            var respawnInterval = _harvestableDataService.GetRespawnInterval(_harvestableType);
            _respawnText.text = $"Interval: {respawnInterval:F}";
        }

        private void UpdateUpgradeButtons(BigInteger currentScore)
        {
            var powerUpgradePrice = _harvestableDataService.GetPowerPrice(_harvestableType);
            var respawnUpgradePrice = _harvestableDataService.GetRespawnIntervalPrice(_harvestableType);

            _powerUpgradeButton.interactable = _scoreService.IsPurchaseAvailable(powerUpgradePrice);
            _respawnUpgradeButton.interactable = _scoreService.IsPurchaseAvailable(respawnUpgradePrice);

            _powerUpgradePriceText.text = $"{powerUpgradePrice.ToString()}";
            _respawnUpgradePriceText.text = $"{respawnUpgradePrice.ToString()}";
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