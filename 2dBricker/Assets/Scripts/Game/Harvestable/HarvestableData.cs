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

<<<<<<< HEAD
        public float GetPower(int upgradeLevel)
=======
        /*
        public float GetFloatPower(int upgradeLevel)
>>>>>>> develop
        {
            if (!float.TryParse(BasePower, out var basePowerInt))
            {
                basePowerInt = 1;
            }

            return basePowerInt * Mathf.Pow(BasePowerMultiplier, upgradeLevel);
<<<<<<< HEAD
=======
        }*/

        public BigInteger GetPower(int upgradeLevel)
        {
            if (!BigInteger.TryParse(BasePower, out var basePowerInt))
            {
                basePowerInt = 1;
            }

            return basePowerInt * (BigInteger)(Mathf.Pow(BasePowerMultiplier, upgradeLevel) * 1000) / 1000 +
                   upgradeLevel;
>>>>>>> develop
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

<<<<<<< HEAD
            return basePowerUpgradePriceInt * (BigInteger)Mathf.Pow(BasePowerUpgradePriceMultiplier, upgradeLevel);
=======
            return basePowerUpgradePriceInt *
                (BigInteger)(Mathf.Pow(BasePowerUpgradePriceMultiplier, upgradeLevel) * 1000) / 1000 + upgradeLevel;
>>>>>>> develop
        }

        public BigInteger GetRespawnPrice(int upgradeLevel)
        {
            if (!BigInteger.TryParse(BaseRespawnUpgradePrice, out var baseRespawnUpgradePriceInt))
            {
                baseRespawnUpgradePriceInt = 1;
            }

<<<<<<< HEAD
            return baseRespawnUpgradePriceInt * (BigInteger)Mathf.Pow(BaseRespawnUpgradePriceMultiplier, upgradeLevel);
=======
            return baseRespawnUpgradePriceInt *
                (BigInteger)(Mathf.Pow(BaseRespawnUpgradePriceMultiplier, upgradeLevel) * 1000) / 1000 + upgradeLevel;
>>>>>>> develop
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
            Slow = 4,
            Bonus = 5
        }
    }
}