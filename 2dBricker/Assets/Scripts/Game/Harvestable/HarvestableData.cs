using System.Numerics;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class HarvestableData
    {
        public HarvestableType Type { get; }
        public string HarvestableName { get; }
        public string Description { get; }
        public string BasePower { get; }
        public float BasePowerMultiplier { get; }
        public string BasePowerUpgradePrice { get; }
        public float BasePowerUpgradePriceMultiplier { get; }
        public float BaseRespawnTime { get; }
        public float BaseRespawnTimeMultiplier { get; }
        public string BaseRespawnUpgradePrice { get; }
        public float BaseRespawnUpgradePriceMultiplier { get; }

        public HarvestableData(RawData rawData)
        {
            Type = (HarvestableType)rawData.typeId;
            HarvestableName = rawData.name;
            Description = rawData.description;
            BasePower = rawData.basePower;
            BasePowerMultiplier = rawData.basePowerMultiplier;
            BasePowerUpgradePrice = rawData.basePowerUpgradePrice;
            BasePowerUpgradePriceMultiplier = rawData.basePowerUpgradePriceMultiplier;
            BaseRespawnTime = rawData.baseRespawnTime;
            BaseRespawnTimeMultiplier = rawData.baseRespawnTimeMultiplier;
            BaseRespawnUpgradePrice = rawData.baseRespawnUpgradePrice;
            BaseRespawnUpgradePriceMultiplier = rawData.baseRespawnUpgradePriceMultiplier;
        }

        public float GetPower(int upgradeLevel)
        {
            if (!int.TryParse(BasePower, out var basePowerInt))
            {
                basePowerInt = 1;
            }

            return basePowerInt + BasePowerMultiplier * Mathf.Log10(1 + upgradeLevel);
        }

        public float GetRespawnInterval(int upgradeLevel)
        {
            return BaseRespawnTime * Mathf.Pow(BaseRespawnTimeMultiplier, upgradeLevel);
        }

        public BigInteger GetPowerPrice(int upgradeLevel)
        {
            if (!BigInteger.TryParse(BasePowerUpgradePrice, out var basePowerUpgradePriceInt))
            {
                basePowerUpgradePriceInt = 1;
            }

            return basePowerUpgradePriceInt +
                   (BigInteger)(BasePowerUpgradePriceMultiplier * Mathf.Log10(1 + upgradeLevel));
        }

        public BigInteger GetRespawnPrice(int upgradeLevel)
        {
            if (!BigInteger.TryParse(BaseRespawnUpgradePrice, out var baseRespawnUpgradePriceInt))
            {
                baseRespawnUpgradePriceInt = 1;
            }

            return baseRespawnUpgradePriceInt +
                   (BigInteger)(BaseRespawnUpgradePriceMultiplier * Mathf.Log10(1 + upgradeLevel));
        }

        public struct RawData
        {
            public int typeId;
            public string name;
            public string description;
            public string basePower;
            public float basePowerMultiplier;
            public string basePowerUpgradePrice;
            public float basePowerUpgradePriceMultiplier;
            public float baseRespawnTime;
            public float baseRespawnTimeMultiplier;
            public string baseRespawnUpgradePrice;
            public float baseRespawnUpgradePriceMultiplier;
        }

        public enum HarvestableType
        {
            Simple = 1,
            Little = 2,
            Blow = 3,
            Slow = 4
        }
    }
}