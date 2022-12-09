using System;
using System.Collections.Generic;
using System.Numerics;
using I2.Loc;
using UnityEngine;
using Vorval.CalmBall.Game;
using Zenject;
using static Vorval.CalmBall.Game.HarvestableData;

namespace Vorval.CalmBall.Service
{
    public class HarvestableDataService : MonoBehaviour
    {
        public Action OnServiceReady { get; set; }
        public Action<HarvestableType> OnHarvestableBought { get; set; }
        public Action<HarvestableType> OnPowerUpgrade { get; set; }
        public Action<HarvestableType> OnRespawnUpgrade { get; set; }

        private Dictionary<HarvestableType, HarvestableData> _dataDictionary;
        private Dictionary<HarvestableType, HarvestableUpgradeData> _upgradeDataDictionary;

        private ConfigRemoteService _configRemoteService;
        private ScoreService _scoreService;


        [Inject]
        private void Construct(ConfigRemoteService configRemoteService, ScoreService scoreService)
        {
            _configRemoteService = configRemoteService;
            _scoreService = scoreService;
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

        public string GetHarvestableName(HarvestableType harvestableType)
        {
            var localizeName = LocalizationManager.GetTranslation(_dataDictionary[harvestableType].HarvestableName);
            return localizeName;
        }

        #region Buy

        public bool IsBought(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            return upgradeData.IsBought;
        }

        public void BuyHarvestable(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            var buyPrice = GetBuyPrice(harvestableType);
            if (_scoreService.ReduceScore(buyPrice))
            {
                upgradeData.SetBought();
                SaveService.SaveHarvestableUpgradeData(upgradeData);
                OnHarvestableBought?.Invoke(harvestableType);
            }
        }

        #endregion

        #region Upgrade

        public void BuyUpgradePower(HarvestableType harvestableType)
        {
            var powerUpgradePrice = GetPowerPrice(harvestableType);
            if (_scoreService.ReduceScore(powerUpgradePrice))
            {
                UpgradePower(harvestableType);
            }
        }

        public void UpgradePower(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            upgradeData.UpgradePower();
            SaveService.SaveHarvestableUpgradeData(upgradeData);
            OnPowerUpgrade?.Invoke(harvestableType);
        }

        public void BuyUpgradeRespawn(HarvestableType harvestableType)
        {
            var respawnUpgradePrice = GetRespawnIntervalPrice(harvestableType);
            if (_scoreService.ReduceScore(respawnUpgradePrice))
            {
                UpgradeRespawn(harvestableType);
            }
        }

        public void UpgradeRespawn(HarvestableType harvestableType)
        {
            var upgradeData = _upgradeDataDictionary[harvestableType];
            upgradeData.UpgradeRespawn();
            SaveService.SaveHarvestableUpgradeData(upgradeData);
            OnRespawnUpgrade?.Invoke(harvestableType);
        }

        #endregion

        #region Power and level

        public int GetPowerLevel(HarvestableType harvestableType)
        {
            return _upgradeDataDictionary[harvestableType].PowerUpgradeLevel;
        }

        public float GetFloatPower(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].PowerUpgradeLevel;
            var power = (float)_dataDictionary[harvestableType].GetPower(upgradeLevel) / 1000f;
            return power;
        }

        public BigInteger GetPower(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].PowerUpgradeLevel;
            var power = _dataDictionary[harvestableType].GetPower(upgradeLevel);
            return power;
        }

        public int GetRespawnIntervalLevel(HarvestableType harvestableType)
        {
            return _upgradeDataDictionary[harvestableType].RespawnUpgradeLevel;
        }

        public float GetRespawnInterval(HarvestableType harvestableType)
        {
            var upgradeLevel = _upgradeDataDictionary[harvestableType].RespawnUpgradeLevel;
            var respawnDelay = _dataDictionary[harvestableType].GetRespawnInterval(upgradeLevel);
            return respawnDelay;
        }

        #endregion

        #region Prices

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

        #endregion

        public BigInteger GetMeanEarnings(HarvestableType harvestableType)
        {
            if (harvestableType is HarvestableType.Bonus or HarvestableType.Blow)
            {
                return BigInteger.Zero;
            }

            var countInMinute = 60f / GetRespawnInterval(harvestableType);
            var meanEarned = new BigInteger(countInMinute * 1000) * GetPower(harvestableType) / 1000;
            return meanEarned;
        }

        public BigInteger GetOpenMeanEarnings(float modifier = 1f)
        {
            var meanEarnings = BigInteger.Zero;

            foreach (var (type, upgradeData) in _upgradeDataDictionary)
            {
                if (upgradeData.IsBought)
                {
                    meanEarnings += GetMeanEarnings(type);
                }
            }

            meanEarnings *= (BigInteger)modifier;

            return meanEarnings;
        }

        private void InitData(ConfigRemoteService.RemoteData remoteData)
        {
            var harvestableDataList = new List<HarvestableData>(5)
            {
                new(GetDataFromJson(remoteData.SimpleHarvestableJson)),
                new(GetDataFromJson(remoteData.LittleHarvestableJson)),
                new(GetDataFromJson(remoteData.BlowHarvestableJson)),
                new(GetDataFromJson(remoteData.SlowHarvestableJson)),
                new(GetDataFromJson(remoteData.BonusHarvestableJson))
            };

            foreach (var harvestableData in harvestableDataList)
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