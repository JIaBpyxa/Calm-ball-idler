using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vorval.CalmBall.Service;
using Zenject;
using static Vorval.CalmBall.Game.HarvestableData;

namespace Vorval.CalmBall.Game
{
    public class HarvestableDataService : MonoBehaviour
    {
        public Action OnServiceReady;

        private List<HarvestableData> _harvestableDataList;

        public Action<HarvestableType> OnHarvestableBought;
        public Action<HarvestableType> OnPowerUpgrade;
        public Action<HarvestableType> OnRespawnUpgrade;

        private Dictionary<HarvestableType, HarvestableData> _dataDictionary;
        private Dictionary<HarvestableType, HarvestableUpgradeData> _upgradeDataDictionary;

        private ConfigRemoteService _configRemoteService;

        [Inject]
        private void Construct(ConfigRemoteService configRemoteService)
        {
            _configRemoteService = configRemoteService;
        }

        private void Awake()
        {
            _dataDictionary = new Dictionary<HarvestableType, HarvestableData>();
            _upgradeDataDictionary = new Dictionary<HarvestableType, HarvestableUpgradeData>();
        }

        private void OnEnable()
        {
            _configRemoteService.OnRemoteDataLoaded += InitData;
        }

        private void OnDisable()
        {
            _configRemoteService.OnRemoteDataLoaded -= InitData;
        }

        public bool IsBought(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            return upgradeData.IsBought;
        }

        public void BuyHarvestable(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            upgradeData.BuyHarvestable();
            SaveService.SaveHarvestableUpgradeData(upgradeData);
            OnHarvestableBought?.Invoke(harvestableType);
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

        public BigInteger GetBuyPrice(HarvestableType harvestableType)
        {
            return _dataDictionary[harvestableType].GetBuyPrice();
        }

        private void InitData(ConfigRemoteService.RemoteData remoteData)
        {
            _harvestableDataList = new List<HarvestableData>(4)
            {
                new(GetDataFromJson(remoteData.SimpleHarvestableJson)),
                new(GetDataFromJson(remoteData.LittleHarvestableJson)),
                new(GetDataFromJson(remoteData.BlowHarvestableJson)),
                new(GetDataFromJson(remoteData.SlowHarvestableJson)),
            };

            foreach (var harvestableData in _harvestableDataList)
            {
                var harvestableType = harvestableData.Type;
                _dataDictionary.Add(harvestableType, harvestableData);
                var upgradeData = SaveService.GetHarvestableUpgradeData(harvestableType);
                _upgradeDataDictionary.Add(harvestableType, upgradeData);
            }

            OnServiceReady?.Invoke();


            RawData GetDataFromJson(string json)
            {
                return JsonUtility.FromJson<RawData>(json);
            }
        }
    }
}