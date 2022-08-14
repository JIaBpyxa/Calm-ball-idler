using System.Numerics;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    public class HarvestableData
    {
        public HarvestableType Type { get; }
        public string HarvestableName { get; }
        public string Description { get; }
        private readonly string BasePower;
        private readonly float BasePowerMultiplier;
        private readonly string BasePowerUpgradePrice;
        private readonly float BasePowerUpgradePriceMultiplier;
        private readonly float BaseRespawnTime;
        private readonly float BaseRespawnTimeMultiplier;
        private readonly string BaseRespawnUpgradePrice;
        private readonly float BaseRespawnUpgradePriceMultiplier;
        private readonly string BuyPrice;

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
            BuyPrice = rawData.buyPrice;
        }

        public float GetPower(int upgradeLevel)
        {
            if (!float.TryParse(BasePower, out var basePowerInt))
            {
                basePowerInt = 1;
            }

            //return basePowerInt + BasePowerMultiplier * Mathf.Log10(1 + upgradeLevel);
            return basePowerInt * Mathf.Pow(BasePowerMultiplier, upgradeLevel);
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

            //return basePowerUpgradePriceInt +
            //       (BigInteger)(BasePowerUpgradePriceMultiplier * Mathf.Log10(1 + upgradeLevel));
            return basePowerUpgradePriceInt * (BigInteger)Mathf.Pow(BasePowerUpgradePriceMultiplier, upgradeLevel);
        }

        public BigInteger GetRespawnPrice(int upgradeLevel)
        {
            if (!BigInteger.TryParse(BaseRespawnUpgradePrice, out var baseRespawnUpgradePriceInt))
            {
                baseRespawnUpgradePriceInt = 1;
            }

            //return baseRespawnUpgradePriceInt +
            //       (BigInteger)(BaseRespawnUpgradePriceMultiplier * Mathf.Log10(1 + upgradeLevel));
            return baseRespawnUpgradePriceInt * (BigInteger)Mathf.Pow(BaseRespawnUpgradePriceMultiplier, upgradeLevel);
        }

        public BigInteger GetBuyPrice()
        {
            if (!BigInteger.TryParse(BuyPrice, out var buyPrice))
            {
                buyPrice = 1;
            }

            return buyPrice;
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
            public string buyPrice;
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