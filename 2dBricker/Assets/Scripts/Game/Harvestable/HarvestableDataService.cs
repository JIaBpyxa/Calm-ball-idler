using System;
using System.Collections.Generic;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;

namespace Vorval.CalmBall.Game
{
    public class HarvestableDataService : MonoBehaviour
    {
        [SerializeField] private List<HarvestableData> _harvestableDataList;

        public Action<HarvestableType, int> OnPowerUpgrade;

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
            OnPowerUpgrade?.Invoke(harvestableType, upgradeData.PowerUpgradeLevel);
        }

        public float GetPower(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].PowerUpgradeLevel;
            var power = _dataDictionary[harvestableType].GetPower(upgradeLevel);
            return power;
        }
    }
}