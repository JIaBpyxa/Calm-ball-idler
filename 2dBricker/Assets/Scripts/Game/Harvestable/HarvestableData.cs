using System.Numerics;
using UnityEngine;

namespace Vorval.CalmBall.Game
{
    [CreateAssetMenu(menuName = "Create HarvestableData", fileName = "HarvestableData", order = 0)]
    public class HarvestableData : ScriptableObject
    {
        public HarvestableType type;
        public string harvestableName;
        public string description;
        [Space] public string basePower;
        public float basePowerMultiplier;
        public string basePowerUpgradePrice;
        public float basePowerUpgradePriceMultiplier;
        [Space] public float baseRespawnTime;
        public float baseRespawnTimeMultiplier;
        public string baseRespawnUpgradePrice;
        public float baseRespawnUpgradePriceMultiplier;

        public float GetPower(int upgradeLevel)
        {
            if (!int.TryParse(basePower, out var basePowerInt))
            {
                basePowerInt = 1;
            }

            return basePowerInt + basePowerMultiplier * Mathf.Log10(1 + upgradeLevel);
        }

        public float GetRespawnInterval(int upgradeLevel)
        {
            return baseRespawnTime * Mathf.Pow(baseRespawnTimeMultiplier, upgradeLevel);
        }

        public BigInteger GetPowerPrice(int upgradeLevel)
        {
            if (!BigInteger.TryParse(basePowerUpgradePrice, out var basePowerUpgradePriceInt))
            {
                basePowerUpgradePriceInt = 1;
            }

            return basePowerUpgradePriceInt +
                   (BigInteger)(basePowerUpgradePriceMultiplier * Mathf.Log10(1 + upgradeLevel));
        }

        public BigInteger GetRespawnPrice(int upgradeLevel)
        {
            if (!BigInteger.TryParse(baseRespawnUpgradePrice, out var baseRespawnUpgradePriceInt))
            {
                baseRespawnUpgradePriceInt = 1;
            }

            return baseRespawnUpgradePriceInt +
                   (BigInteger)(baseRespawnUpgradePriceMultiplier * Mathf.Log10(1 + upgradeLevel));
        }
    }

    public enum HarvestableType
    {
        Simple = 1,
        Little = 2,
        Blow = 3,
        Slow = 4
    }
}