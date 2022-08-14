using static Vorval.CalmBall.Game.HarvestableData;

namespace Vorval.CalmBall.Game
{
    public class HarvestableUpgradeData
    {
        public struct RawData
        {
            public int typeId;
            public bool isBought;
            public int powerUpgradeLevel;
            public int respawnIntervalUpgradeLevel;
        }

        public HarvestableType Type { get; }
        public bool IsBought { get; private set; }
        public int PowerUpgradeLevel { get; private set; }
        public int RespawnUpgradeLevel { get; private set; }

        public HarvestableUpgradeData(RawData rawData)
        {
            Type = (HarvestableType)rawData.typeId;
            IsBought = rawData.isBought || Type == HarvestableType.Simple;
            PowerUpgradeLevel = rawData.powerUpgradeLevel;
            RespawnUpgradeLevel = rawData.respawnIntervalUpgradeLevel;
        }

        public void BuyHarvestable()
        {
            IsBought = true;
        }

        public void UpgradePower()
        {
            PowerUpgradeLevel++;
        }

        public void UpgradeRespawn()
        {
            RespawnUpgradeLevel++;
        }

        public RawData GetRawData()
        {
            return new RawData
            {
                typeId = (int)Type,
                isBought = IsBought,
                powerUpgradeLevel = PowerUpgradeLevel,
                respawnIntervalUpgradeLevel = RespawnUpgradeLevel
            };
        }
    }
}