using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vorval.CalmBall.Service;

namespace Vorval.CalmBall.Game
{
    public class HarvestableDataService : MonoBehaviour
    {
        [SerializeField] private List<HarvestableData> _harvestableDataList;

        public Action<HarvestableType> OnPowerUpgrade;
        public Action<HarvestableType> OnRespawnUpgrade;

        private Dictionary<HarvestableType, HarvestableData> _dataDictionary;
        private Dictionary<HarvestableType, HarvestableUpgradeData> _upgradeDataDictionary;

        private void Awake()
        {
            _dataDictionary = new Dictionary<HarvestableType, HarvestableData>();
            _upgradeDataDictionary = new Dictionary<HarvestableType, HarvestableUpgradeData>();
        }

        private void Start()
        {
            foreach (var harvestableData in _harvestableDataList)
            {
                var harvestableType = harvestableData.type;
                _dataDictionary.Add(harvestableType, harvestableData);
                var upgradeData = SaveService.GetHarvestableUpgradeData(harvestableType);
                _upgradeDataDictionary.Add(harvestableType, upgradeData);
            }
        }

        public void UpgradePower(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            upgradeData.UpgradePower();
            SaveService.SaveHarvestableUpgradeData(upgradeData);
            OnPowerUpgrade?.Invoke(harvestableType);
        }

        public void UpgradeRespawn(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            upgradeData.UpgradeRespawn();
            SaveService.SaveHarvestableUpgradeData(upgradeData);
            OnRespawnUpgrade?.Invoke(harvestableType);
        }

        public float GetPower(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].PowerUpgradeLevel;
            var power = _dataDictionary[harvestableType].GetPower(upgradeLevel);
            return power;
        }

        public float GetRespawnInterval(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].RespawnUpgradeLevel;
            var respawnDelay = _dataDictionary[harvestableType].GetRespawnInterval(upgradeLevel);
            return respawnDelay;
        }

        public BigInteger GetPowerPrice(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].PowerUpgradeLevel;
            var powerPrice = _dataDictionary[harvestableType].GetPowerPrice(upgradeLevel);
            return powerPrice;
        }

        public BigInteger GetRespawnIntervalPrice(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].RespawnUpgradeLevel;
            var respawnIntervalPrice = _dataDictionary[harvestableType].GetRespawnPrice(upgradeLevel);
            return respawnIntervalPrice;
        }
    }
}